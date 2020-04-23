using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCDWpf.Detector
{
    public class DetectorArgs : EventArgs
    {
        private string message;
        private DateTime date;
        public DetectorArgs(string message)
        {
            this.message = message;
            this.date = DateTime.Now;
        }

        // This is a straightforward implementation for 
        // declaring a public field
        public string Message
        {
            get
            {
                return message;
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
        }
    }
}
