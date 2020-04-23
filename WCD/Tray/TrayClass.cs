using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCDWpf.Tray
{
    class TrayClass
    {
        private MainWindow mainWindow;
        private ContextMenu m_menu;
        private NotifyIcon ni;

        public TrayClass(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            /* */
            ni = new NotifyIcon();
            ni.Icon = WCDWpf.Properties.Resources.ico;
            ni.Text = "WebChangeDetector";
            ni.Visible = true;
            ni.DoubleClick += TrayOpen_Click;
            /* menu */
            m_menu = new ContextMenu();
            m_menu.MenuItems.Add(0,
                new MenuItem("Open", new System.EventHandler(TrayOpen_Click)));
            m_menu.MenuItems.Add(1,
                new MenuItem("Close", new System.EventHandler(TrayExit_Click)));
            ni.ContextMenu = m_menu;
        }

        private void TrayExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void TrayOpen_Click(object sender, EventArgs e)
        {
            mainWindow.Show();
            mainWindow.Activate();
        }

        public void ShowTrayInformation(string Title, string Content)
        {
            ni.BalloonTipTitle = Title;
            ni.BalloonTipText = Content;
            ni.BalloonTipIcon = ToolTipIcon.None;
            ni.Visible = true;
            ni.ShowBalloonTip(30000);
            ni.BalloonTipClicked += delegate (object sender, EventArgs args)
            {
                mainWindow.Show();
                mainWindow.Activate();
            };
        }
    }
}
