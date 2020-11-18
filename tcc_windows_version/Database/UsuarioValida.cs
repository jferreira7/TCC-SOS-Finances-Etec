using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Business;
using tcc_windows_version.Database;
using tcc_windows_version.Model;
using MySql.Data.MySqlClient;

namespace tcc_windows_version.Database
{
    class UsuarioValida
    {
        Connection objConexao = new Connection();        
        MySqlDataAdapter adpt;

        public DataTable Login(Usuario usuario)
        {
            try
            {
                DataTable data = new DataTable("usuarios");
                adpt = new MySqlDataAdapter("SELECT id, nome FROM usuarios WHERE email='" + usuario.email + "' AND senha='" + usuario.senha + "';", objConexao.Conexao());
                adpt.Fill(data);
                objConexao.Desconectar();
                return data;
            } catch
            {
                return null;
            }
        }
    }
}
