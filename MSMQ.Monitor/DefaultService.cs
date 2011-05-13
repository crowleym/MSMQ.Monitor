using System;
using System.Configuration;
using System.ServiceProcess;
using Common.Logging;

namespace MSMQ.Monitor
{
    /// <summary>
    /// Hybrid Service / Console porcess for hosting SAM
    /// </summary>
    class DefaultService : ServiceBase
    {
        /// <summary>
        /// Initialise logger
        /// </summary>
        private static ILog Logger = LogManager.GetLogger("MSMQ.Monitor");

        private MSMQCounter _counter;

        #region ServiceBase members
        /// <summary>
        /// Executes when a Start command is sent to the service by the Service Control Manager (SCM)
        /// or when the operating system starts (for a service that starts automatically).
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            Logger.Info("Initialising Configuration");
            _counter = new MSMQCounter(
                ConfigurationManager.AppSettings["qName"],
                int.Parse(ConfigurationManager.AppSettings["interval"])
            );

            _counter.Enabled = true;
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            if (_counter != null)
            {
                _counter.Dispose();
                _counter = null;
            }
        }
        #endregion

        #region Main
        /// <summary>
        /// Entry point of program
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            //Log the unknown
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                if (e.IsTerminating)
                {
                    Logger.Fatal("Unhandled Exception", e.ExceptionObject as Exception);
                }
                else
                {
                    Logger.Error("Unhandled Exception", e.ExceptionObject as Exception);
                }
            };

            if (Environment.UserInteractive)
            {
                DefaultService svc = new DefaultService();
                svc.OnStart(args);
                Console.WriteLine("Process started. Press Return to exit.");
                Console.ReadLine();
                svc.OnStop();
            }
            else
            {
                ServiceBase.Run(new DefaultService());
            }
        }
        #endregion
    }

}
