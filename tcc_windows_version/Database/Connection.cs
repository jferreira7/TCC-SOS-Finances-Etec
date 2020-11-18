using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcc_windows_version.Database
{
    class Connection
    {
        MySqlConnection objSqlConnnection = new MySqlConnection();
        
        string server = "localhost";
        string port = "3306";
        string user_id = "root";
        string password = "";
        string database = "sym";
        
        /*
        string server = "localhost";
        string port = "3306";
        string user_id = "root";
        string password = "";
        string database = "sym";
        */

        public string Conexao()
        {
            objSqlConnnection.ConnectionString = "server=" + server + ";port=" + port + ";User Id=" + user_id + ";password=" + password + ";database=" + database + ";";
            return objSqlConnnection.ConnectionString;
        }

        public MySqlConnection Conectar()
        {            
            if (objSqlConnnection.State == System.Data.ConnectionState.Closed)
            {
                objSqlConnnection.Open();
            }
            return objSqlConnnection;

        }
        public void Desconectar()
        {
            if (objSqlConnnection.State == System.Data.ConnectionState.Open)
            {
                objSqlConnnection.Close();
            }
        }
    }
}

