using System.Windows.Forms;

namespace TwitchClient.Panels
{
    public partial class UpdateBroadcastingInfoPanel : UserControl
    {
        public string GameText { get { return gameTextBox.Text; } }
        public string TitleText { get { return titleTextBox.Text; } }

        public UpdateBroadcastingInfoPanel()
        {
            InitializeComponent();
        }

        public void ClearGameTextBox()
        {
            gameTextBox.Text = "";
        }

        public void ClearTitleTextBox()
        {
            titleTextBox.Text = "";
        }
    }
}
