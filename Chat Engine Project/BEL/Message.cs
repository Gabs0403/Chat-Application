using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEL
{
    internal class Message
    {
        public int messageID { get; set; }

        public int conversationID { get; set; }
        public int senderID { get; set; }
        public DateTime messageDate { get; set; }
        public string messageText { get; set; }
    }
}
