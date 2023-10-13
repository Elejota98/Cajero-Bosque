using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Entities
{
    public class PaymentEvent
    {
        private int _id_event;
        private int _device;
        private int _message;
        private string _wm_event;

        public int id_event
        {
            get { return _id_event; }
            set { _id_event = value; }
        }

        public int device
        {
            get { return _device; }
            set { _device = value; }
        }

        public int message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string wm_event
        {
            get { return _wm_event; }
            set { _wm_event = value; }
        }


    }
}
