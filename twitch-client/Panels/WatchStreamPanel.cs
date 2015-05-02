using System.Windows.Forms;

namespace TwitchClient.Panels
{
    public partial class WatchStreamPanel : UserControl
    {
        public WatchStreamPanel()
        {
            InitializeComponent();
        }

        public void Navigate(string url)
        {
            webBrowser.Navigate(url);
        }

        private void webBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true; // disabling pop ups
        }
    }
}
