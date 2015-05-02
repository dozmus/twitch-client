using System.Windows.Forms;

namespace TwitchClient.Panels
{
    public partial class BroadcastingInfoPanel : UserControl
    {
        public BroadcastingInfoPanel()
        {
            InitializeComponent();
        }

        public void SetGame(string game)
        {
            gameLabel.Invoke((MethodInvoker)(() => gameLabel.Text = "Game: " + game));
        }

        public void SetTitle(string title)
        {
            titleLabel.Invoke((MethodInvoker)(() => titleLabel.Text = "Title: " + title));
        }

        public void SetAvgFps(double avgFps)
        {
            avgFpsLabel.Invoke((MethodInvoker)(() => avgFpsLabel.Text = "Average FPS: " + avgFps));
        }

        public void SetSrcVidQual(int vidHeight)
        {
            srcVidQualLabel.Invoke((MethodInvoker)(() => srcVidQualLabel.Text = "Source Video Quality: " + vidHeight + "p"));
        }
    }
}
