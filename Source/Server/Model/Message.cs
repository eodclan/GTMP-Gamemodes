using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Model
{
    class Message
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string RightLabel { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public Client Sender { get; set; }
        public Message()
        {
            Title = "";
            Description = "";
            RightLabel = "";
            Value1 = "";
            Value2 = "";
            Sender = null;
        }
    }
}
