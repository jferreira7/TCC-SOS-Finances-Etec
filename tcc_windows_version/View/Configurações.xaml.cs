using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using tcc_windows_version.Business;
using tcc_windows_version.Model;
using tcc_windows_version.Properties;

namespace tcc_windows_version.View
{
    /// <summary>
    /// Lógica interna para Configurações.xaml
    /// </summary>
    public partial class Configurações : Window
    {
        public Configurações()
        {
            InitializeComponent();
            exibirDadosUsuario();
        }  
        public void exibirDadosUsuario()
        {
            Usuario usuario = new Usuario();
            usuario.email = Settings.Default["email"].ToString();
            usuario.senha = Settings.Default["senha"].ToString();

            UsuarioBO bo = new UsuarioBO();
            DataTable user = bo.UsuarioData(usuario);

            if (Convert.ToInt32(user.Rows[0]["id"]) > 0)
            {
                tbNomeUsuario.Text = user.Rows[0]["nome"].ToString();
                tbEmailUsuario.Text = usuario.email;
                tbVencimentoPlanoUsuario.Text = user.Rows[0]["vencimento_plano"].ToString().Substring(0, 10);
            }
        }

        private void tbAlterarUsuario_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Deseja realmente trocar de usuário?","Trocar usuário", MessageBoxButton.YesNo);

            if (Result == MessageBoxResult.Yes)
            {
                Settings.Default["email"] = "";
                Settings.Default["senha"] = "";
                Settings.Default.Save();
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else if (Result == MessageBoxResult.No)
            {
                
            }
            
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
