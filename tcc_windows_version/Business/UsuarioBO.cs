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

namespace tcc_windows_version.Business
{
    class UsuarioBO
    {
        public string nome_usuario = "";
        public int Logar(Usuario usario)
        {
            if (usario.email != "" && usario.senha != "")
            {
                UsuarioValida uv = new UsuarioValida();
                DataTable data = uv.Login(usario);

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
                return -1;
            }
        }
    }
}
