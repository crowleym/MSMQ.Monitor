using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Messaging;

using Common.Logging;

namespace MSMQ.Monitor
{
    public class MSMQCounter : IDisposable
    {
        private static ILog Logger = LogManager.GetCurrentClassLogger();

        private Timer _timer;
        private MessageQueue _queue;
        private readonly string _outputPath;
        private readonly LocalDataContext DataContext = new LocalDataContext();

        public MSMQCounter(string qName, int interval)
        {
            if (!MessageQueue.Exists(qName)) throw new ArgumentException("queue does not exist, or is not accessible", "qName");

            _queue = new MessageQueue(qName);
            _queue.MessageReadPropertyFilter = new MessagePropertyFilter()
            {
                AdministrationQueue = false,
                ArrivedTime = false,
                CorrelationId = false,
                Priority = false,
                ResponseQueue = false,
                SentTime = false,
                Body = false,
                Label = true,
                Id = true
            };

            _timer = new Timer(interval * 1000)
            {
                Enabled = false,
                AutoReset = false
            };

            _timer.Elapsed += (sender, args) => Tick();
        }

        private void Tick()
        {
            var now = DateTime.Now;
            var qName = _queue.FormatName;

            Logger.Debug("Checking queue count...");
            try
            {
                foreach (var m in _queue.GetAllMessages())
                {
                    DataContext.MSMQDataLogs.Add(new MSMQDataLog() { QueueName = qName, MessageID = m.Id, Label = m.Label, LogDate = now });
                }

                DataContext.SaveChanges();
            }
            catch (Exception x)
            {
                Logger.Error("Unexpected Error", x);
            }
            finally
            {
                if (Enabled) _timer.Start(); //work done, poll again
            }
        }

        private bool _enabled;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = _timer.Enabled = value;
            }
        }

        public void Dispose()
        {
            Enabled = false;
            _timer.Dispose();
            _queue.Dispose();
            DataContext.Dispose();
        }
    }
}
