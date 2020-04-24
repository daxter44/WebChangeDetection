using HtmlAgilityPack;
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
        string _elementName;
        DispatcherTimer _timer;
        HtmlDocument _oldWebPage;
        HtmlDocument _newWebPage;
        HtmlNode _oldNode;
        HtmlNode _newNode;
        public delegate void DetectorHandler(object myObject, DetectorArgs myArgs);
        public event DetectorHandler OnWebsiteChange;
        public delegate void DetectorHistoryHandler(object myObject, DetectorArgs myArgs);
        public event DetectorHistoryHandler HistoryChange;
        public DetectorClass(String url, int freq, String elementName)
        {
            _url = url;
            _freq = freq;
            _elementName = elementName;            
        }

        public void StartDetection()
        {
            _oldWebPage = new HtmlDocument();
            _oldWebPage.LoadHtml(GetHtmlPage());
            if(_elementName != "") { 
                _oldNode = _oldWebPage.GetElementbyId(_elementName);
                if(_oldNode == null)
                {
                    DetectorArgs myArg = new DetectorArgs("Cant find element on website ");
                    HistoryChange(this, myArg);
                }
            }
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, _freq, 0);
            _timer.Start();
        }
        public void StopDetection()
        {
            _timer.Stop();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            _newWebPage = new HtmlDocument() ;
            _newWebPage.LoadHtml(GetHtmlPage());
            
            if (_elementName == "")
            {
                HTMLDocumentCompare();
            }
            else
            {
                _newNode = _newWebPage.GetElementbyId(_elementName);
                HTMLNodeCompare();
            }       
        }

        private void HTMLDocumentCompare()
        {
            if (_oldWebPage.DocumentNode != _newWebPage.DocumentNode)                
            {
                _oldWebPage = _newWebPage;
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
        private void HTMLNodeCompare()
        {
            if (_oldNode != null && _newNode != null)
            {
                if (_oldNode.OuterHtml != _newNode.OuterHtml)
                {
                    _oldNode = _newNode;
                    DetectorArgs myArgs = new DetectorArgs("Your node changes");
                    HistoryChange(this, myArgs);
                    OnWebsiteChange(this, myArgs);
                }
                else
                {
                    DetectorArgs myArgs = new DetectorArgs("no changes on website ");
                    HistoryChange(this, myArgs);
                }
            }
            else
            {
                DetectorArgs myArg = new DetectorArgs("Cant find element on website ");
                HistoryChange(this, myArg);
            }
        }

        private String GetHtmlPage()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader source = new StreamReader(response.GetResponseStream());
                return source.ReadToEnd();                
            }
            catch (Exception ex)
            {
                DetectorArgs myArgs = new DetectorArgs("I can't check page. No internet");
                HistoryChange(this, myArgs);
                return "";
            }

        }
    }
}
