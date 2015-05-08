using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TwitchClient
{
    public partial class WelcomeForm : Form
    {
        private const string TargetApplicationScope =
            "channel_editor+channel_commercial+channel_subscriptions+channel_check_subscription+user_follows_edit+user_blocks_read+user_blocks_edit";

        private const string ClientId = "po0xacl9h91pa2mfesv2h26l8xwtars";
        private readonly string _responseHtml;
        private byte[] _buffer;
        private Socket _listener;
        private string _response;

        public WelcomeForm()
        {
            InitializeComponent();

            // Checking if the resources path exists
            string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

            if (!Directory.Exists(resourcesPath))
            {
                MessageBox.Show("The resources directory is missing, please re-download the application.",
                    "Resources folder missing - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Directory.CreateDirectory(resourcesPath);
                Application.Exit();
            }

            // Reading the html response file, if possible
            string fileName = Path.Combine(resourcesPath, "twitch_api_response_page.html");

            if (!File.Exists(fileName))
            {
                MessageBox.Show("A resource is missing (twitch_api_response_page.html).",
                    "Resource missing - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            else
            {
                _responseHtml = File.ReadAllText(fileName);
            }
        }

        public string ApplicationScope { get; private set; }
        public bool Success { get; private set; }
        public string AuthToken { get; private set; }

        public string TwitchUsername
        {
            get { return twitchUsernameTextBox.Text; }
        }

        #region UI Manipulation

        private void WelcomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Quitting the application completely, if the user presses the close button
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
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
            if (!String.IsNullOrWhiteSpace(twitchResponseTextBox.Text))
            {
                string response = twitchResponseTextBox.Text.Trim();

                // Checking response length - without content length of format is 21
                if (response.Length < 22)
                {
                    MessageBox.Show(
                        "Invalid response length, too short.",
                        "Invalid response - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // format: #access_token={auth token}&scope={scope}
                // Parsing the response
                int scopeSegmentIndex = response.IndexOf("&scope=");
                string scope = response.Substring(scopeSegmentIndex + 7);
                string authToken = response.Substring(14).Split('&')[0];

                // Checking and updating parameters
                if (String.IsNullOrWhiteSpace(authToken))
                {
                    MessageBox.Show(
                        "The auth token appears to be invalid.",
                        "Invalid auth. token - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!scope.Equals(TargetApplicationScope, StringComparison.CurrentCultureIgnoreCase))
                {
                    MessageBox.Show(
                        "The requested application scope does not match the provided one, as such some features may not function.",
                        "Request Scope Mismatch - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Updating parameters and continuing
                ApplicationScope = scope;
                AuthToken = authToken;
                Success = true;
            }
            else
            {
                MessageBox.Show("Please enter the twitch response you got from the browser.");
                return;
            }

            // Fall-back, `good call` procedure (to resume application)
            Hide();
        }

        private void connectTwitchButton_Click(object sender, EventArgs e)
        {
            // Starting the response server listener
            try
            {
                InitializeResponseServer(IPAddress.Parse("127.0.0.1"), 18573);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to initialize HTTP server (on /localhost:18573) to capture Twitch response." +
                    Environment.NewLine + "Error: " + ex.Message, "HTTPD Init - Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // Launching the connect with twitch button
            Process.Start("https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=" + ClientId +
                          "&redirect_uri=http://localhost:18573&scope=" + TargetApplicationScope);
            SetStatus("Waiting for twitch response...");
        }

        #endregion

        #region HTTP response processing pipeline

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
            remoteSocket.EndReceive(ar);
            //string payload = Encoding.UTF8.GetString(_buffer);

            // Sending back a success message and continuing process
            GenerateResponse();
            remoteSocket.BeginSend(Encoding.UTF8.GetBytes(_response), 0, _response.Length, SocketFlags.None,
                SendDataAsyncCallback, remoteSocket);
        }

        private void SendDataAsyncCallback(IAsyncResult ar)
        {
            // Ending connection and send data async
            var remoteSocket = (Socket) ar.AsyncState;
            remoteSocket.Close();

            // Updating status
            SetStatus("Waiting for user response...");
        }

        #endregion

        #region Response Generation

        private void GenerateResponse()
        {
            // XXX improve generation + make page look better
            // Headers
            AppendHeader("HTTP/1.1 200 OK");
            AppendHeader("Connection: close");
            AppendHeader("Date: " + DateTime.Now.ToUniversalTime().ToString("r"));

            // HTML/JS
            _response += Environment.NewLine + _responseHtml;
        }

        private void AppendHeader(string header)
        {
            _response += header + Environment.NewLine;
        }

        #endregion
    }
}