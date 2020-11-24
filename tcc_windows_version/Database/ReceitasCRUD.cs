using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Model;
using tcc_windows_version.View;

namespace tcc_windows_version.Database
{
    class ReceitasCRUD
    {
        Connection objConexao = new Connection();
        MySqlCommand objComando = new MySqlCommand();
        MySqlDataAdapter adpt;
        DataTable data;
        
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
                Mensagem.mensagemSucesso = "Receita adicionada com sucesso!";
            }
            catch //(Exception erro)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor! Tente mais tarde.";
            }
        }
        public DataView Read(int id_usuario)
        {
            data = new DataTable("receitas");

            try 
            { 
                string anoAtual = DateTime.Now.Year.ToString();
                adpt = new MySqlDataAdapter("select * from receitas where id_usuario = '" + id_usuario + "' and YEAR(data_insercao) = " + anoAtual + ";", objConexao.Conexao());
                adpt.Fill(data);
                objConexao.Desconectar();

                return data.DefaultView;                
            }
            catch //(Exception erro)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor! Tente mais tarde.";
                return null;                
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
                Mensagem.mensagemSucesso = "Receita atualizada com sucesso!";
            }
            catch //(Exception erro)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor! Tente mais tarde.";
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
                Mensagem.mensagemSucesso = "Receita deletada com sucesso!";
            }
            catch //(Exception erro)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor! Tente mais tarde.";
            }
        }
        public DataView Search(string descricao, string categoria, string mes, string ano, int id_usuario)
        {
            string query = "select * from receitas where ";

            /*"select* from receitas where descricao like '%nada%' and categoria like '%men%' and MONTH(data_vencimento) = '10' and YEAR(data_vencimento) = '2020' and id_usuario = 1;"*/
            if (descricao != "") { query += "descricao like '%" + descricao + "%' and "; }            
            if (categoria != "") { query += "categoria like '%" + categoria + "%' and "; }
            if (mes != "") { query += "MONTH(data_insercao) = '" + mes + "' and "; }
            if (ano != "") { query += "YEAR(data_insercao) = '" + ano + "' and "; }            
            if (id_usuario > 0) { query += "id_usuario = '" + id_usuario.ToString() + "' and "; }

            query = query.TrimEnd(' ', 'a', 'n', 'd', ' ');
            query += ";";

            data = new DataTable("receitas");

            try
            {
                adpt = new MySqlDataAdapter(query, objConexao.Conexao());
                adpt.Fill(data);
                objConexao.Desconectar();
                return data.DefaultView;
            }
            catch //(Exception erro)
            {
                Mensagem.mensagemErro = "Erro de conexão com o servidor! Tente mais tarde.";
                return null;
            }
        }
    }
}
