using System;
using System.IO;
using System.Net;
using System.Windows.Threading;

namespace WCDWpf.Detector
{
    class DetectorClass
    {
        int _freq;
        string _url;
        string _oldWebPage;
        DispatcherTimer _timer;
        public delegate void DetectorHandler(object myObject, DetectorArgs myArgs);

        public event DetectorHandler OnWebsiteChange;
        public delegate void DetectorHistoryHandler(object myObject, DetectorArgs myArgs);

        public event DetectorHistoryHandler HistoryChange;
        public DetectorClass(String url, int freq)
        {
            _url = url;
            _freq = freq;
           
        }

        public void StartDetection()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader source = new StreamReader(response.GetResponseStream());
                _oldWebPage = source.ReadToEnd();
            }
            catch (Exception ex)
            {
                DetectorArgs myArgs = new DetectorArgs("I can't check page. No internet");
                HistoryChange(this, myArgs);
            }
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, _freq, 0);
            _timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            try { 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader source2 = new StreamReader(response.GetResponseStream());
                String newWebPage;
                newWebPage = source2.ReadToEnd();
                    if (newWebPage == "")
                    {
                        _oldWebPage = newWebPage;
                    }
                    else
                    {
                        if (_oldWebPage != newWebPage)
                        {
                            _oldWebPage = newWebPage;
                            DetectorArgs myArgs = new DetectorArgs("something on website changes");
                            HistoryChange(this, myArgs);
                            OnWebsiteChange(this, myArgs);
                        }
                        else
                        {
                            DetectorArgs myArgs = new DetectorArgs("no changes on website ");
                            HistoryChange(this, myArgs);
                        }
                    }
            }
            catch (Exception ex)
            {
                DetectorArgs myArgs = new DetectorArgs("I can't check page. No internet");
                HistoryChange(this, myArgs);
            }
           
        }
    }
}
