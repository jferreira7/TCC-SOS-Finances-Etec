using System;
using System.Collections.Generic;
using System.Linq;
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

namespace tcc_windows_version.View
{
    /// <summary>
    /// Lógica interna para Cadastro.xaml
    /// </summary>
    public partial class Cadastro : Window
    {
        public Cadastro()
        {
            InitializeComponent();
        }
        public void mensagemSucesso(string mensagem)
        {
            if (mensagem != "")
            {
                tbMensagem.Visibility = Visibility.Visible;
                tbMensagem.Foreground = new SolidColorBrush(Colors.Green);
                tbMensagem.Text = mensagem;
                Mensagem.mensagemSucesso = "";
            }
        }
        public void mensagemErro(string mensagem)
        {
            if (mensagem != "")
            {
                tbMensagem.Visibility = Visibility.Visible;
                tbMensagem.Foreground = new SolidColorBrush(Colors.Red);
                tbMensagem.Text = mensagem;
                Mensagem.mensagemErro = "";
            }
        }
        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {

            Usuario u = new Usuario();
            UsuarioBO uBO = new UsuarioBO();

            u.nome = txtNomeCompleto.Text;
            u.email = txtEmail.Text;
            u.senha = txtSenha.Password;

            if (uBO.ChecarEmail(u.email) == 1)
            {
                if (u.senha == txtConfirmarSenha.Password) 
                {
                    u.senha = sha256(u.senha);

                    uBO.Registrar(u);                    
                }
                else
                {
                    Mensagem.mensagemErro = "Senhas não batem!";
                }   
            }
            else if (uBO.ChecarEmail(u.email) == 3)
            {
                Mensagem.mensagemErro = "Preencha todos os campos!";
            } 
            else if (uBO.ChecarEmail(u.email) == 2)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor!";
            }
            else if(uBO.ChecarEmail(u.email) == 0)
            {
                Mensagem.mensagemErro = "E-mail já existe!";
            }

            mensagemErro(Mensagem.mensagemErro);
            mensagemSucesso(Mensagem.mensagemSucesso);
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
