using System.Windows.Forms;

namespace TwitchClient.Panels.ChatBot
{
    public partial class ChatBotCredentialsPanel : UserControl
    {
        // TODO add tool tips, password must be in oauth, from http://twitchapps.com/tmi/, etc.
        public string Nickname { get { return nicknameTextBox.Text; } }
        public string Password { get { return passwordTextBox.Text; } }
        public string Hostname
        {
            get
            {
                // Checking if the irc server textbox is not invalid
                if (ircServerTextBox.Text.Length == 0 || !ircServerTextBox.Text.Contains("/"))
                    return null;

                // Returning the target server's ip endpoint
                string[] component = ircServerTextBox.Text.Split('/');
                return component[0];
            }
        }
        public int Port
        {
            get
            {
                // Checking if the irc server textbox is not invalid
                if (ircServerTextBox.Text.Length == 0 || !ircServerTextBox.Text.Contains("/"))
                    return -1;

                // Returning the target server's ip endpoint
                string[] component = ircServerTextBox.Text.Split('/');
                return int.Parse(component[1]);
            }
        }

        public ChatBotCredentialsPanel()
        {
            InitializeComponent();
        }
    }
}
