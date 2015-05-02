using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
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

            // Preparing custom controls
            InitializeCustomControls();

            // Starting the update timer and issuing a second-0 update
            updateTimer.Start();
            updateTimer_Tick(this, EventArgs.Empty);
            followersUpdateTimer.Start();
        }

        private void InitializeCustomControls()
        {
            // Update broadcasting info button click
            updateBroadcastingInfoPanel.updateButton.Click += updateBroadcastingInfoButton_Click;

            // Loading preview page
            watchStreamPanel.Navigate("http://www.twitch.tv/" + _welcomeForm.TwitchUsername + "/popout");
        }

        private void updateBroadcastingInfoButton_Click(object sender, EventArgs e)
        {
            // Preparing request
            var request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + _welcomeForm.TwitchUsername + "?"
                + "channel[status]=" + Uri.EscapeDataString(updateBroadcastingInfoPanel.TitleText)
                + "&channel[game]=" + Uri.EscapeDataString(updateBroadcastingInfoPanel.GameText)
                + "&oauth_token=" + _welcomeForm.AuthToken + "&_method=put");
            request.Timeout = 1000;
            request.Method = "GET";
            request.Accept = "application/vnd.twitchtv.v2+json";

            // Reading response
            using (var resp = request.GetResponse())
            {
                var html = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                Debug.WriteLine("Response: " + html);
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            DownloadBaseChannelJsonObject();
        }

        private void followersUpdateTimer_Tick(object sender, EventArgs e)
        {
            DownloadFollowersJsonObject();
        }

        private void DownloadBaseChannelJsonObject()
        {
            _baseChannelJsonObjectClient.DownloadStringCompleted += (sender, e) => OnDownloadBaseChannelJsonObject(e.Result);
            _baseChannelJsonObjectClient.DownloadStringAsync(new Uri(_baseApiUrl));
        }

        private void DownloadFollowersJsonObject()
        {
            _followersJsonObjectClient.DownloadStringCompleted += (sender, e) => OnDownloadFollowersJsonObject(e.Result);
            _followersJsonObjectClient.DownloadStringAsync(new Uri(_latestFollowersApiUrl));
        }

        private void OnDownloadFollowersJsonObject(string source)
        {
            var task = new Task(() => UpdateFollowersInfo(source));
            task.Start();
        }

        private void OnDownloadBaseChannelJsonObject(string source)
        {
            var task = new Task(() => UpdateBaseChannelInfo(source));
            task.Start();
        }

        private void UpdateFollowersInfo(string source)
        {
            if (source.Length == 0)
                return;
            var json = JsonConvert.DeserializeObject<FollowersJsonObject.RootObject>(source);

            if (json.follows.Count == 0)
                return;
            var follows = json.follows;

            // Updating recent followers info
            followersRichTextBox.Invoke((MethodInvoker)(() => followersRichTextBox.Text = ""));

            foreach (FollowersJsonObject.Follow follow in follows)
            {
                followersRichTextBox.Invoke((MethodInvoker)(() => followersRichTextBox.Text += String.Format("{0} {1}{2}", follow.created_at, follow.user.display_name, Environment.NewLine)));
            }
        }

        private void UpdateBaseChannelInfo(string source)
        {
            if (source.Length == 0)
                return;
            var json = JsonConvert.DeserializeObject<BaseChannelJsonObject.RootObject>(source);

            if (json.streams.Count == 0)
                return;
            var stream = json.streams[0];

            // Updating the status main statslabel
            UpdateMainStatsLabel(stream);

            // Updating channel broadcasting config panel
            UpdateChannelBroadcastingConfigPanel(stream);

            // Updating other stuff
            // XXX _streamPreviewImageUrl = stream.preview.template;
        }

        private void UpdateMainStatsLabel(BaseChannelJsonObject.Stream stream)
        {
            mainStatsLabel.Text = String.Format("Viewers: {0}, Followers: {1}, Views: {2}", stream.viewers, stream.channel.followers, stream.channel.views);
        }

        private void UpdateChannelBroadcastingConfigPanel(BaseChannelJsonObject.Stream stream)
        {
            broadcastingInfoPanel.SetTitle(stream.channel.status);
            broadcastingInfoPanel.SetGame(stream.channel.game);
            broadcastingInfoPanel.SetAvgFps(Math.Round(stream.average_fps, 2));
            broadcastingInfoPanel.SetSrcVidQual(stream.video_height);
        }

        private void masterMenuStrip_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void ResizeControls()
        {
            // Watch stream panel
            watchStreamPanel.Width = Size.Width - 38;
            watchStreamPanel.Height = Size.Height - 273;

            // Followers text box
            followersRichTextBox.Size = new Size(Size.Width - 373, followersRichTextBox.Size.Height);
        }
    }
}
