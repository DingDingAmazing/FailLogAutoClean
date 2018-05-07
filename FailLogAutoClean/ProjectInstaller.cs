using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace FailLogAutoClean
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Committed += new InstallEventHandler(ProjectInstaller_Committed);
        }

        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {
            //auto start the service after installing
            //System.ServiceProcess.ServiceController controller = new System.ServiceProcess.ServiceController("FailLogAutoClean(ICI2.0)");
            //controller.Start();
        }
    }
}
