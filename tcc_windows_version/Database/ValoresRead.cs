using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace tcc_windows_version.Database
{
    class ValoresRead
    {
        Connection objConexao = new Connection();
        MySqlDataAdapter adpt;

        public string GetGeneric(int id, string coluna)
        {
            DataTable data = new DataTable("saldo");
            adpt = new MySqlDataAdapter("select valor_" + coluna + " from valores where id=" + id + " limit 1;", objConexao.Conexao());
            adpt.Fill(data);
            objConexao.Desconectar();


            if (data.Rows.Count > 0)
            {
                return data.Rows[0]["valor_" + coluna].ToString();
            }
            else
            {
                return "";
            }
        }
        public string GetSaldo (int id, string coluna) {
            return GetGeneric(id, coluna);
        }
        public string GetReserva(int id, string coluna)
        {
            return GetGeneric(id, coluna);
        }

        /*public string GetPoupanca(int id, string coluna)
        {
            return GetGeneric(id, coluna);
        }*/
    }
}
