using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using TwitchClient.Util;

namespace TwitchClient.ChatBot
{
    class IrcBot
    {
        // TODO throttle writing
        // TODO error handling, handle ping timeouts, reconnecting, etc
        // error logging in >> :tmi.twitch.tv NOTICE * :Error logging in
        // TODO limit chat rtb length
        // TODO add ranks to rtb
        // TODO add twitch emotes to rtb
        // TODO cluster join/part messages

        public static Dictionary<string, string> EchoCommands = new Dictionary<string, string>();
        public static Dictionary<string, int> EchoCommandsCooldown = new Dictionary<string, int>();
        public static List<string> RandomNotifications = new List<string>(); 
        private const string CommandPrefix = "!";
        private const int EchoCommandCooldown = 35000;
        private const int RandomNotificationCooldown = 120000;
        private readonly TcpClient _client;
        private readonly RichTextBox _chatRichTextBox;
        private readonly ListBox _chatUsersListBox;
        private int _lastPing;
        private int _lastServerPing;
        private int _lastRandomNotification;
        private Thread _thread;
        private StreamWriter _writer;
        private StreamReader _reader;
        private string _channel;
        private string _nickname;
        private string _password;
        private readonly Random _random = new Random();
        public bool Connected { get; private set; }

        public IrcBot(RichTextBox chatRichTextBox, ListBox chatUsersListBox)
        {
            // Initialising socket
            _client = new TcpClient(); // XXX udp?
            _chatRichTextBox = chatRichTextBox;
            _chatUsersListBox = chatUsersListBox;
        }

        #region Core functionality
        public bool Init(string nickname, string password, string channel, string hostname, int port)
        {
            // Setting variables
            _channel = channel.StartsWith("#") ? channel : "#" + channel;
            _nickname = nickname;
            _password = password;

#if DEBUG
            Debug.WriteLine("Chat bot initialised with settings: (nick:" + _nickname + ",channel:" + _channel + ",host:" + hostname + ",port:" + port + ")");
#endif

            try
            {
                _client.Connect(hostname, port);
                _writer = new StreamWriter(_client.GetStream());
                _reader = new StreamReader(_client.GetStream());
                Connected = true;
            }
            catch (Exception e)
            {
                // XXX msgbox.write(e)
                Connected = false;
                return false;
            }

            // Initialising operational threads
            _thread = new Thread(Run)
            {
                Name = "ChatBot_IrcThread"
            };
            _thread.Start();
            return true;
        }

        public void Run()
        {
            // Initialising session
            _lastPing = _lastServerPing = Environment.TickCount;
            _writer.WriteLine("PASS " + _password);
            _writer.WriteLine("NICK " + _nickname);
            _writer.WriteLine("JOIN " + _channel);
            _writer.Flush();

#if DEBUG
            Debug.WriteLine("Chat bot is now connecting.");
#endif

            // Blocking for on-connect message - >> :tmi.twitch.tv 001 <nickname> :<welcome message>
            while (!_reader.ReadLine().Contains("001"))
            {
            }

#if DEBUG
            Debug.WriteLine("Chat bot is now connected.");
#endif
            AppendText("Chat bot is now connected.");

            // Read loop
            while (Connected)
            {
                // Sleep to reduce CPU stress, at start because some loops are skipped
                Thread.Sleep(1);

                // Ping-pong initiation
                if (Environment.TickCount - _lastPing > 25000)
                {
                    WriteLineFlush("PING LAG" + DateHelper.UnixTimestampNow());
                    _lastPing = Environment.TickCount;
                }

                // Checking if we should spit a random notification
                if (RandomNotifications.Count > 0 &&
                    Environment.TickCount - _lastRandomNotification > RandomNotificationCooldown)
                {
                    SendMessageFlush(RandomNotifications[_random.Next(RandomNotifications.Count + 1)]);
                    _lastRandomNotification = Environment.TickCount;
                }

                // Checking if there is anything to read
                if (_client.Available == 0)
                    continue;
                string line = _reader.ReadLine();

                // Checking if the read line is valid
                if (String.IsNullOrEmpty(line))
                    continue;

#if DEBUG
                Debug.WriteLine(">> " + line);
#endif

                // Normalising line
                if (line.StartsWith(":"))
                {
                    line = line.Substring(1);
                }

                // Ping-pong response
                if (line.StartsWith("PING"))
                {
                    WriteLineFlush(line.Replace("PING", "PONG"));
                    _lastServerPing = Environment.TickCount;
                }

                if (line.StartsWith("PONG"))
                {
                    _lastServerPing = Environment.TickCount;
                }

                // Regular message parsing
                string[] explode = line.Split(' ');

                if (explode.Length < 2)
                {
                    return;
                }
                string sender = explode[0];
                string command = explode[1];

                switch (command) // XXX make an OOP command handler
                {
                    case "JOIN":
                        // Parsing input
                        string nick = sender.Substring(0, sender.IndexOf('!'));

                        // Updating UI
                        AppendTextPrefixNewLine("* Join: " + nick);
                        AddUser(nick);
                        break;
                    case "PART":
                        // Parsing input
                        nick = sender.Substring(0, sender.IndexOf('!'));

                        // Updating UI
                        AppendTextPrefixNewLine("* Part: " + nick);
                        RemoveUser(nick);
                        break;
                    case "QUIT":
                        // XXX impl - i dont think twitch even uses quit?
                        break;
                    case "PRIVMSG":
                        // Parsing input
                        nick = sender.Substring(0, sender.IndexOf('!'));
                        int messageStartIndex = line.IndexOf(':') + 1;
                        string message = line.Substring(messageStartIndex);

                        // Updating UI
                        AppendTextPrefixNewLine(String.Format("<{0}> {1}", nick, message));

                        // Replying to message if necessary
                        if (message.StartsWith(CommandPrefix))
                        {
                            // Echo commands
                            if (!message.Contains(" "))
                            {
                                message = message.Substring(CommandPrefix.Length); // removing the command prefix

                                lock (EchoCommands)
                                {
                                    // Checking if the command exists
                                    if (EchoCommands.ContainsKey(message))
                                    {
                                        int deltaTime = Environment.TickCount - EchoCommandsCooldown[message];

                                        if (deltaTime > EchoCommandCooldown)
                                        {
                                            SendMessageFlush(EchoCommands[message]);
                                            EchoCommandsCooldown[message] = Environment.TickCount;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
        #endregion

        #region Socket Writing
        private void SendMessageFlush(string message)
        {
            // PRIVMSG <channel> :<msg>
            WriteLineFlush("PRIVMSG " + _channel + " :" + message);
        }

        private void WriteLineFlush(string line)
        {
            try
            {
                // Writing line
                _writer.WriteLine(line);
                _writer.Flush();

                // Adding line to rtb if necessary
                if (line.StartsWith("PRIVMSG "))
                {
                    int colonIndex = line.IndexOf(':') + 1;
                    string msg = line.Substring(colonIndex);
                    AppendTextPrefixNewLine(String.Format("<{0}> {1}", _nickname, msg));
                }
            }
            catch (Exception e)
            {
                Connected = false;
                // TODO spit error - this usually means connection has terminated
            }
        }
        #endregion

        #region UI manipulation
        private void AppendText(string text)
        {
            _AppendText(String.Format("[{0}] {1}", DateTime.Now, text));
        }

        private void AppendTextPrefixNewLine(string text)
        {
            _AppendText(String.Format("{2}[{0}] {1}", DateTime.Now, text, Environment.NewLine));
        }

        private void _AppendText(string text)
        {
            _chatRichTextBox.Invoke((MethodInvoker)(() =>
            {
                _chatRichTextBox.AppendText(text);

                // Scrolling to end of rtb if not focused (i.e. if the user is not fiddling with it)
                if (!_chatRichTextBox.Focused) // TODO fix this - its not working
                {
                    _chatRichTextBox.SelectionStart = _chatRichTextBox.Text.Length;
                    _chatRichTextBox.ScrollToCaret();
                }
            }));
        }

        private void AddUser(string nickname)
        {
            _chatUsersListBox.Invoke((MethodInvoker)(() => _chatUsersListBox.Items.Add(nickname)));
        }

        private void RemoveUser(string nickname)
        {
            _chatUsersListBox.Invoke((MethodInvoker)(() => _chatUsersListBox.Items.Remove(nickname)));
        }
        #endregion
    }
}
