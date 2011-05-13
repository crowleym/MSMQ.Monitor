using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSMQ.Monitor
{
    public class MSMQDataLog
    {
        public int ID { get; set; }

        public string QueueName { get; set; }

        public string MessageID { get; set; }

        public string Label { get; set; }

        public DateTime LogDate { get; set; }
    }
}
