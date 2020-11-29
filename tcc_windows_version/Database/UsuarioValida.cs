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
using tcc_windows_version.View;

namespace tcc_windows_version.Database
{
    class UsuarioValida
    {
        Connection objConexao = new Connection();
        MySqlCommand objComando = new MySqlCommand();
        MySqlDataAdapter adpt;

        public DataTable Login(Usuario usuario)
        {
            try
            {
                DataTable data = new DataTable("usuarios");
                adpt = new MySqlDataAdapter("SELECT id, nome, vencimento_plano FROM usuarios WHERE email='" + usuario.email + "' AND senha='" + usuario.senha + "' LIMIT 1;", objConexao.Conexao());
                
                adpt.Fill(data);
                objConexao.Desconectar();
                return data;
            } catch
            {                
                return null;
            }
        }
        public void Register(Usuario usuario)
        {
            objComando.CommandType = CommandType.Text;
            objComando.CommandText = "insert into usuarios (nome, email, senha, vencimento_plano) VALUES (@nome, @email, @senha, '2021-01-01 10:10:10');";
            
            objComando.Parameters.Add("@nome", MySqlDbType.VarChar, 150).Value = usuario.nome;
            objComando.Parameters.Add("@email", MySqlDbType.VarChar, 150).Value = usuario.email;
            objComando.Parameters.Add("@senha", MySqlDbType.VarChar, 150).Value = usuario.senha;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                Mensagem.mensagemSucesso = "Usuário cadastrado com sucesso!";
            }
            catch //(Exception erro)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor! Tente mais tarde.";
            }
        }
        public int CheckEmail(string email)
        {
            try
            {
                DataTable data = new DataTable("usuarios");
                adpt = new MySqlDataAdapter("SELECT email FROM usuarios WHERE email='"+email+"' LIMIT 1;", objConexao.Conexao());
                adpt.Fill(data);
                objConexao.Desconectar();

                if(data.Rows.Count > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
                
            } 
            catch //(Exception erro)
            {
                
                return 2;
            }
        }
    }
}
