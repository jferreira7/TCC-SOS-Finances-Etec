using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Properties;

namespace tcc_windows_version
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (Settings.Default["email"].ToString() == "" && Settings.Default["senha"].ToString() == "")
            {
                this.StartupUri = new Uri("View/Login.xaml", UriKind.Relative);
            }
            else
            {
                this.StartupUri = new Uri("Principal.xaml", UriKind.Relative);
                //this.StartupUri = new Uri("View/Configurações.xaml", UriKind.Relative);
            }
        }
    }
}
