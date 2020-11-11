using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Model;

namespace tcc_windows_version.Database
{
    class ReceitasCRUD
    {
        Connection objConexao = new Connection();
        MySqlCommand objComando = new MySqlCommand();
        public void Create(Receitas receita)
        {
            objComando.CommandType = CommandType.Text;
            objComando.CommandText = "insert into receitas (descricao, categoria, valor, id_usuario) VALUES (@descricao, @categoria, @valor, @id_usuario);";

            objComando.Parameters.Add("@descricao", MySqlDbType.VarChar, 150).Value = receita.descricao;            
            objComando.Parameters.Add("@categoria", MySqlDbType.VarChar, 100).Value = receita.categoria;
            objComando.Parameters.Add("@valor", MySqlDbType.Decimal, 12).Value = receita.valor;
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = receita.id_usuario;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Receita cadastrada!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }
        public void Update(Receitas receita)
        {
            objComando.CommandText = "update receitas set descricao = @descricao, categoria = @categoria, valor = @valor where id = @id and id_usuario = @id_usuario";

            objComando.Parameters.Add("@descricao", MySqlDbType.VarChar, 150).Value = receita.descricao;
            objComando.Parameters.Add("@categoria", MySqlDbType.VarChar, 100).Value = receita.categoria;
            objComando.Parameters.Add("@valor", MySqlDbType.Decimal, 12).Value = receita.valor;
            objComando.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(receita.id);
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = receita.id_usuario;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Receita atualizada!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }
        public void Delete(int id)
        {
            objComando.CommandText = "delete from receitas where id = @id";
            objComando.Parameters.Add("@id", MySqlDbType.Int32, 11).Value = id;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Receita deletada!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro de conexão com o servidor: " + erro);
            }
        }
    }
}
