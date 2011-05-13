using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace MSMQ.Monitor
{
    [RunInstaller(true)]
    public partial class DefaultInstaller : Installer
    {
        public DefaultInstaller()
        {
            InitializeComponent();
        }

        private void DefaultInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            ServiceInstaller serviceInstaller = new ServiceInstaller()
            {
                Description = "MSMQ Monitor",
                StartType = ServiceStartMode.Automatic,
            };

            string name = Context.Parameters["name"];
            if (!string.IsNullOrEmpty(name))
            {
                serviceInstaller.ServiceName = name;
            }
            else
            {
                serviceInstaller.ServiceName = "MSMQ.Monitor";
            }

            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalSystem
            };

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}
