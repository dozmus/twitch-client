using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TwitchClient.ChatBot;
using TwitchClient.Json;

namespace TwitchClient
{
    public partial class MainForm : Form
    {
        private readonly string _baseApiUrl = "https://api.twitch.tv/kraken/streams?channel=";
        private readonly string _latestFollowersApiUrl = "https://api.twitch.tv/kraken/channels/{username}/follows?direction=DESC&limit={amount}&offset=0";
        private readonly WebClient _baseChannelJsonObjectClient = new WebClient();
        private readonly WebClient _followersJsonObjectClient = new WebClient();
        private readonly WelcomeForm _welcomeForm = new WelcomeForm();
        private readonly IrcBot _ircBot;
        private bool _updatingFollowers;
        private bool _updatingBaseStats;

        // TODO handle async download failure, i.e. mark flags as available so update occurs next timer tick
        // TODO push all debug.writeline to hidden textbox in dev mode? + dev mode
        // TODO add feature to remove echo commands/random notifications

        #region Initialisation
        public MainForm()
        {
            InitializeComponent();

            // Showing the welcome form
            _welcomeForm.ShowDialog();

            // Terminating application if no data was received
            if (!_welcomeForm.Success)
            {
                Application.Exit();
            }
            _baseApiUrl += _welcomeForm.TwitchUsername;
            _latestFollowersApiUrl =
                _latestFollowersApiUrl.Replace("{username}", _welcomeForm.TwitchUsername).Replace("{amount}", "10");

            // Preparing web client objects
            _followersJsonObjectClient.DownloadStringCompleted += (sender, e) => OnDownloadFollowersJsonObject(e.Result);
            _baseChannelJsonObjectClient.DownloadStringCompleted += (sender, e) => OnDownloadBaseChannelJsonObject(e.Result);

            // Preparing custom controls
            InitializeCustomControls();

            // Starting the update timers and issuing a second-0 update
            updateTimer.Start();
            updateTimer_Tick(this, EventArgs.Empty);

            followersUpdateTimer.Start();
            followersUpdateTimer_Tick(this, EventArgs.Empty);

            // Initialising the irc bot class
            _ircBot = new IrcBot(chatRichTextBox, chatUsersListBox);
        }

        private void InitializeCustomControls()
        {
            // Update broadcasting info button click
            updateBroadcastingInfoPanel.updateButton.Click += updateBroadcastingInfoUpdateButton_Click;

            // Connect irc bot button click
            chatBotCredentialsPanel.connectButton.Click += chatBotCredentialsPanelConnectButton_Click;

            // Loading preview page
#if DEBUG
            watchStreamPanel.SetBrowserHtml("<!DOCTYPE html><html><body style=\"background-color:#000;font-size:32px;color:#fff;\">Stream preview here.</body></html>");
#else
            watchStreamPanel.Navigate("http://www.twitch.tv/" + _welcomeForm.TwitchUsername + "/popout");
#endif
        }
        #endregion

        #region Custom control bindings
        private void chatBotCredentialsPanelConnectButton_Click(object sender, EventArgs e)
        {
            // TODO verify input, listen to init response
            _ircBot.Init(chatBotCredentialsPanel.Nickname, chatBotCredentialsPanel.Password, _welcomeForm.TwitchUsername,
                chatBotCredentialsPanel.Hostname, chatBotCredentialsPanel.Port);
        }

        private void updateBroadcastingInfoUpdateButton_Click(object sender, EventArgs e)
        {
            // Preparing request
            // TODO only send info which is to be updated, ie if textbox is empty
            var url = "https://api.twitch.tv/kraken/channels/" + _welcomeForm.TwitchUsername + "?"
                      + "channel[status]=" + Uri.EscapeDataString(updateBroadcastingInfoPanel.TitleText)
                      + "&channel[game]=" + Uri.EscapeDataString(updateBroadcastingInfoPanel.GameText)
                      + "&oauth_token=" + _welcomeForm.AuthToken + "&_method=put";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1500;
            request.Method = "GET";
            request.Accept = "application/vnd.twitchtv.v2+json";

            // Reading response
            // TODO parse response and check what happened to our request: success, failure, html error code, etc.
            /*
            using (var resp = request.GetResponse())
            {
                var html = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            }
            */
        }
        #endregion

        #region Timer tick/start/stop events
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (_updatingBaseStats)
                return;
            _updatingBaseStats = true;
            DownloadBaseChannelJsonObject();
        }

        private void followersUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (_updatingFollowers)
                return;
            _updatingFollowers = true;
            DownloadFollowersJsonObject();
        }

        private void mainStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toggleMainStatsToolStripMenuItem.Checked)
            {
                updateTimer.Stop();
            }
            else
            {
                updateTimer.Start();
            }
            toggleMainStatsToolStripMenuItem.Checked = !toggleMainStatsToolStripMenuItem.Checked;
        }

        private void followersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toggleFollowersToolStripMenuItem.Checked)
            {
                followersUpdateTimer.Stop();
            }
            else
            {
                followersUpdateTimer.Start();
            }
            toggleFollowersToolStripMenuItem.Checked = !toggleFollowersToolStripMenuItem.Checked;
        }
        #endregion

        #region Recent followers updating
        private void DownloadFollowersJsonObject()
        {
            _followersJsonObjectClient.DownloadStringAsync(new Uri(_latestFollowersApiUrl));
        }

        private void OnDownloadFollowersJsonObject(string source)
        {
            var task = new Task(() => UpdateFollowersInfo(source));
            task.Start();
        }

        private void UpdateFollowersInfo(string source)
        {
            if (source.Length == 0)
            {
                _updatingFollowers = false;
                return;
            }
            var json = JsonConvert.DeserializeObject<FollowersJsonObject.RootObject>(source);

            if (json.follows == null || json.follows.Count == 0)
            {
                _updatingFollowers = false;
                return;
            }

            // Updating recent followers info
            string text = String.Empty;

            foreach (var follower in json.follows)
            {
                // Formatting follow date - format: 2015-05-04T16:56:38Z
                string createdAt = follower.created_at;
                string date = createdAt.Substring(0, 10);
                string time = createdAt.Substring(11, 8);
                createdAt = String.Format("{0} {1}", date, time);

                // Appending to current text block
                text += String.Format("{0} {1}{2}", createdAt, follower.user.display_name, Environment.NewLine);
            }
            text = text.TrimEnd(new[] { '\r', '\n' }); // removing the new line at the end
            followersTextBox.Invoke((MethodInvoker)(() => followersTextBox.Text = text));
            _updatingFollowers = false;

#if DEBUG
            Debug.WriteLine("Completed Task(UPDATE_RECENT_FOLLOWERS)");
#endif
        }
        #endregion

        #region Base stats updating
        private void DownloadBaseChannelJsonObject()
        {
            _baseChannelJsonObjectClient.DownloadStringAsync(new Uri(_baseApiUrl));
        }

        private void OnDownloadBaseChannelJsonObject(string source)
        {
            var task = new Task(() => UpdateBaseChannelInfo(source));
            task.Start();
        }

        private void UpdateBaseChannelInfo(string source)
        {
            if (source.Length == 0)
            {
                _updatingBaseStats = false;
                return;
            }
            var json = JsonConvert.DeserializeObject<BaseChannelJsonObject.RootObject>(source);

            if (json.streams == null || json.streams.Count == 0)
            {
                _updatingBaseStats = false;
                return;
            }
            var stream = json.streams[0];

            // Updating the status main stats label
            UpdateMainStatsLabel(stream);

            // Updating channel broadcasting config panel
            UpdateChannelBroadcastingConfigPanel(stream);

            // Updating other stuff
            // XXX _streamPreviewImageUrl = stream.preview.template;

            _updatingBaseStats = false;

#if DEBUG
            Debug.WriteLine("Completed Task(UPDATE_BASE_STATS)");
#endif
        }
        #endregion

        #region Updating UI info
        private void UpdateMainStatsLabel(BaseChannelJsonObject.Stream stream)
        {
            string text = String.Format("Viewers: {0} | Followers: {1} | Views: {2}", stream.viewers,
                stream.channel.followers, stream.channel.views);
            masterStatusStrip.Invoke((MethodInvoker)(() => mainStatsLabel.Text = text));
        }

        private void UpdateChannelBroadcastingConfigPanel(BaseChannelJsonObject.Stream stream)
        {
            broadcastingInfoPanel.SetTitle(stream.channel.status);
            broadcastingInfoPanel.SetGame(stream.channel.game);
            broadcastingInfoPanel.SetAvgFps(Math.Round(stream.average_fps, 2));
            broadcastingInfoPanel.SetSrcVidQual(stream.video_height);
        }
        #endregion

        #region Resizing behaviour
        private void MainForm_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void ResizeControls()
        {
            // Watch stream panel
            watchStreamPanel.Width = Size.Width - 30;
            watchStreamPanel.Height = Size.Height - 273;

            // Followers text box
            followersTextBox.Size = new Size(Size.Width - 373, followersTextBox.Size.Height);
        }
        #endregion

        #region Chat bot settings UI events
        private void addEchoCommandButton_Click(object sender, EventArgs e)
        {
            // Trying to add command to dictionary
            string echoMessage = newEchoCommandTextBox.Text;
            int colonIndex = echoMessage.IndexOf(':');

            string commandName = echoMessage.Substring(0, colonIndex);
            string message = echoMessage.Substring(colonIndex + 1);

            lock (IrcBot.EchoCommands)
            {
                // Checking if we can add the entry to the dictionary
                if (IrcBot.EchoCommands.ContainsKey(commandName))
                {
                    return; // XXX failure, TODO tell user
                }

                // Adding the entry
                IrcBot.EchoCommands.Add(commandName, message);
                IrcBot.EchoCommandsCooldown.Add(commandName, 0); // adding and 'resetting' cooldown
            }

            // Adding command to list box - assuming success if we got this far
            echoCommandsListBox.Items.Add(newEchoCommandTextBox.Text);
            newEchoCommandTextBox.Text = "";
        }

        private void addRandomNotificationButton_Click(object sender, EventArgs e)
        {
            // Trying to add item to list
            string message = newRandomNotificationTextBox.Text;

            lock (IrcBot.RandomNotifications)
            {
                // Checking if we can add the entry to the dictionary
                if (IrcBot.RandomNotifications.Contains(message))
                {
                    return; // XXX failure, TODO tell user
                }

                // Adding the entry
                IrcBot.RandomNotifications.Add(message);
            }

            // Adding command to list box - assuming success if we got this far
            randomNotificationsListBox.Items.Add(newRandomNotificationTextBox.Text);
            newRandomNotificationTextBox.Text = "";
        }
        #endregion
    }
}
