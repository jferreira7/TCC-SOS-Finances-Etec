using tcc_windows_version.Database;
using tcc_windows_version.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using tcc_windows_version.View;

namespace tcc_windows_version.Business
{
    class UsuarioBO
    {
        public string nome_usuario = "";
        public int Logar(Usuario usuario)
        {
            try
            {
                if (usuario.email != "" && usuario.senha != "")
                {
                    UsuarioValida uv = new UsuarioValida();
                    
                    DataTable data = uv.Login(usuario);
                    if (data != null)
                    {
                        if (data.Rows.Count > 0)
                        {
                            nome_usuario = data.Rows[0]["nome"].ToString();
                            return Convert.ToInt32(data.Rows[0]["id"]);
                        }
                        else
                        {
                            return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    return -1;
                }
            }catch
            {
                return -3;
            }
        }
        public DataTable UsuarioData(Usuario usuario)
        {           
            if (usuario.email != "" && usuario.senha != "")
            {
                UsuarioValida uv = new UsuarioValida();
                return uv.Login(usuario);
            }
            else
            {
                return null;
            }
        }
        public void Registrar(Usuario usuario)
        {
            if (usuario.nome != "" && usuario.email != "" && usuario.senha != "")
            {
                UsuarioValida uv = new UsuarioValida();
                uv.Register(usuario);
            }
            else
            {
                Mensagem.mensagemErro = "Preencha todos os campos!";
            }
        }
        public int ChecarEmail(string email)
        {
            if (email != "")
            {
                UsuarioValida uv = new UsuarioValida();
                return uv.CheckEmail(email);
            }
            else
            {
                
                return 3;
            }  
        }
    }
}
