using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TwitchClient
{
    public partial class WelcomeForm : Form
    {
        public const string clientId = "po0xacl9h91pa2mfesv2h26l8xwtars";
        public const string clientSecret = "6lyo34hiygifmcjg7mpysel0z5r9b5a";
        private byte[] _buffer;
        private Socket _listener;
        private string _reqCode;
        private string _response;

        public WelcomeForm()
        {
            InitializeComponent();
        }

        public bool Success { get; private set; }
        public string AuthToken { get; private set; }

        public string TwitchUsername
        {
            get { return twitchUsernameTextBox.Text; }
        }

        private void connectTwitchButton_Click(object sender, EventArgs e)
        {
            // Starting the response server listener
            InitializeResponseServer(IPAddress.Parse("127.0.0.1"), 18573);

            // Launching the connect with twitch button
            Process.Start("https://api.twitch.tv/kraken/oauth2/authorize?response_type=code&client_id=" + clientId +
                          "&redirect_uri=http://localhost:18573&scope=channel_editor");
            SetStatus("Waiting for twitch auth...");
        }

        private void SetStatus(string status)
        {
            statusLabel.Invoke((MethodInvoker) (() => statusLabel.Text = "Status: " + status));
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            // Checking if a twitch username was entered
            if (String.IsNullOrWhiteSpace(twitchUsernameTextBox.Text))
            {
                MessageBox.Show("Please enter a twitch username.");
                return;
            }

            // Checking if an auth code was generated
            if (!Success)
            {
                MessageBox.Show("Please connect using twitch.");
                return;
            }

            // Fall-back, good call procedure
            Close();
        }

        private void InitializeResponseServer(IPAddress ip, int port)
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(ip, port));
            _listener.Listen(2);
            _listener.BeginAccept(AcceptSocketAsyncCallback, _listener);
        }

        private void AcceptSocketAsyncCallback(IAsyncResult ar)
        {
            Socket remoteSocket = ((Socket) ar.AsyncState).EndAccept(ar);

            if (remoteSocket.Available == 0)
                return;

            _buffer = new byte[remoteSocket.Available];
            remoteSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveDataAsyncCallback,
                remoteSocket);
        }

        private void ReceiveDataAsyncCallback(IAsyncResult ar)
        {
            var remoteSocket = (Socket) ar.AsyncState;
            int resp = remoteSocket.EndReceive(ar);
            string payload = Encoding.UTF8.GetString(_buffer);
            // Console.WriteLine(payload);

            foreach (string line in Regex.Split(payload, Environment.NewLine))
            {
                // Parsing call-back info
                if (!line.StartsWith("GET"))
                    continue;

                // GET / HTTP/1.1 - /?code=[CODE]&scope=[SCOPE]
                string code = line.Substring(11, line.IndexOf("&", StringComparison.Ordinal) - 11);
                _reqCode = code;

                // Updating data
                Debug.WriteLine("Req. code captured through http twitch api callback.");
                SetStatus("Req. code captured.");

                // Sending back a success message and continuing process
                GenerateResponse();
                remoteSocket.BeginSend(Encoding.UTF8.GetBytes(_response), 0, _response.Length, SocketFlags.None,
                    SendDataAsyncCallback, remoteSocket);
                return;
            }
        }

        private void SendDataAsyncCallback(IAsyncResult ar)
        {
            // Ending connection and send data async
            var remoteSocket = (Socket) ar.AsyncState;
            remoteSocket.Close();

            // Requesting auth token
            Debug.WriteLine("Requesting auth code.");

            using (var client = new WebClient())
            {
                byte[] response =
                    client.UploadValues("https://api.twitch.tv/kraken/oauth2/token", new NameValueCollection
                    {
                        {"client_id", clientId},
                        {"client_secret", clientSecret},
                        {"grant_type", "authorization_code"},
                        {"redirect_uri", "http://localhost:18573"},
                        {"code", _reqCode}
                    });

                // Parsing response
                string json = Encoding.UTF8.GetString(response);
                var resp = JsonConvert.DeserializeObject<ResponseJsonObject>(json);

                // Updating auth token
                AuthToken = resp.access_token;
                Success = true;
            }

            // Updating status, etc
            Debug.WriteLine("Remote http socket closed.");
            SetStatus("Done");
        }

        private void GenerateResponse()
        {
            // Headers
            AppendHeader("HTTP/1.1 200 OK");
            AppendHeader("Connection: close");
            AppendHeader("Date: Sat, 02 May 2015 16:50:00 GMT"); // XXX auto gen?

            // Html
            _response += Environment.NewLine +
                         "<!DOCTYPE html><html><body>twitch-client has received the authorization code, you can safely now close this window.</body></html>";
        }

        private void AppendHeader(string header)
        {
            _response += header + Environment.NewLine;
        }

        public class ResponseJsonObject
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public List<string> scope { get; set; }
        }
    }
}