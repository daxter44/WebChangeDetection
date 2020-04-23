using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WCDWpf.Tray;
using WCDWpf.Detector;
using System.Text.RegularExpressions;
using WCDWpf.Mail;

namespace WCDWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TrayClass trayClass;
        DetectorClass detector;
        public MainWindow()
        {
            InitializeComponent();
            Closing += OnClosingWindow;
            trayClass = new TrayClass(this);
        }
        private void OnClosingWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void btnSpy_Click(object sender, RoutedEventArgs e)
        {
            if (btnSpy.Content == "Stop")
            {
                btnSpy.Content = "Detect";
                tbURL.IsEnabled = true;
                tbEmail.IsEnabled = true;
                tbFreq.IsEnabled = true;
            }
            else
            {
                btnSpy.Content = "Stop";
                tbURL.IsEnabled = false;
                tbEmail.IsEnabled = false;
                tbFreq.IsEnabled = false;
                StartDetector();
            };
        }

        private void StartDetector()
        {
            detector = new DetectorClass(tbURL.Text, Convert.ToInt32(tbFreq.Text));
            detector.OnWebsiteChange +=
               new DetectorClass.DetectorHandler(ShowNotification);
            detector.HistoryChange +=
              new DetectorClass.DetectorHistoryHandler(HistoryChanged);
            detector.StartDetection();
        }

        private void HistoryChanged(object myObject, DetectorArgs myArgs)
        {
            String historyItem = myArgs.Date.ToLocalTime().ToString() + " -- "+myArgs.Message;
            lbHistory.Items.Add(historyItem);
        }

        private void ShowNotification(object myObject, DetectorArgs myArgs)
        {
            trayClass.ShowTrayInformation("Website change detector! ", myArgs.Message);
            MailSender.sendMail(tbEmail.Text, tbURL.Text);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
