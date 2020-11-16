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
    class ObjetivosCRUD
    {
        Connection objConexao = new Connection();
        MySqlCommand objComando = new MySqlCommand();
        public void Create(Objetivos objetivo)
        {
            objComando.CommandType = CommandType.Text;
            objComando.CommandText = "insert into objetivos (nome, preco, imagem, valor_mes, valor_inicial, valor_guardado,valor_restante, id_usuario) VALUES (@nome, @preco, @imagem, @valor_mes, @valor_inicial, @valor_guardado, @valor_restante, @id_usuario);";

            objComando.Parameters.Add("@nome", MySqlDbType.VarChar, 8).Value = objetivo.nome;
            objComando.Parameters.Add("@preco", MySqlDbType.Decimal, 12).Value = objetivo.preco;
            objComando.Parameters.Add("@imagem", MySqlDbType.MediumBlob).Value = objetivo.image_bytes;
            objComando.Parameters.Add("@valor_mes", MySqlDbType.Decimal, 12).Value = objetivo.valor_mes;
            objComando.Parameters.Add("@valor_inicial", MySqlDbType.Decimal, 12).Value = objetivo.valor_inicial;
            objComando.Parameters.Add("@valor_guardado", MySqlDbType.Decimal, 12).Value = objetivo.valor_inicial;
            objComando.Parameters.Add("@valor_restante", MySqlDbType.Decimal, 12).Value = (Convert.ToDecimal(objetivo.preco) - Convert.ToDecimal(objetivo.valor_inicial));
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = objetivo.id_usuario;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Objetivo cadastrado!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }/*
        public void Read()
        {

        }
        public void Update(Despesas despesa)
        {
            objComando.CommandText = "update despesas set estado = @estado, empresa = @empresa, nome = @nome, categoria = @categoria, valor = @valor, data_vencimento = @data_vencimento where id = @id and id_usuario = @id_usuario;";

            objComando.Parameters.Add("@estado", MySqlDbType.VarChar, 8).Value = despesa.estado;
            objComando.Parameters.Add("@nome", MySqlDbType.VarChar, 150).Value = despesa.nome;
            objComando.Parameters.Add("@empresa", MySqlDbType.VarChar, 150).Value = despesa.empresa;
            objComando.Parameters.Add("@categoria", MySqlDbType.VarChar, 100).Value = despesa.categoria;
            objComando.Parameters.Add("@valor", MySqlDbType.Decimal, 12).Value = Convert.ToDecimal(despesa.valor);
            objComando.Parameters.Add("@data_vencimento", MySqlDbType.Date).Value = despesa.data_vencimento;
            objComando.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(despesa.id);
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = Convert.ToInt32(despesa.id_usuario);

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Despesa atualizada!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }
        public void Delete(int id)
        {
            objComando.CommandText = "delete from despesas where id = @id";
            objComando.Parameters.Add("@id", MySqlDbType.Int32, 11).Value = id;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Despesa deletada!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro de conexão com o servidor: " + erro);
            }
        }
        public DataView Search(string nome, string empresa, string categoria, string mes, string ano, string estado, int id_usuario)
        {
            string query = "select * from despesas where ";
            /*string nome = "Conta";
            string empresa = "DA";
            string categoria = "NA";
            string mes = "1";
            string ano = "2020";*/

            /*"select* from despesas where nome like '%nada%' and empresa like '%DAAE%' and categoria like '%men%' and MONTH(data_vencimento) = '10' and YEAR(data_vencimento) = '2020' and estado = 'Pendente';"*//*
            if (nome != "") { query += "nome like '%" + nome + "%' and "; }
            if (empresa != "") { query += "empresa like '%" + empresa + "%' and "; }
            if (categoria != "") { query += "categoria like '%" + categoria + "%' and "; }
            if (mes != "") { query += "MONTH(data_vencimento) = '" + mes + "' and "; }
            if (ano != "") { query += "YEAR(data_vencimento) = '" + ano + "' and "; }
            if (estado != "") { query += "estado = '" + estado + "' and "; }
            if (id_usuario > 0) { query += "id_usuario = '" + id_usuario.ToString() + "' and "; }

            query = query.TrimEnd(' ', 'a', 'n', 'd', ' ');
            query += ";";

            Connection objConexao = new Connection();
            MySqlDataAdapter adpt;
            DataTable data = new DataTable("despesas");

            adpt = new MySqlDataAdapter(query, objConexao.Conexao());
            adpt.Fill(data);
            objConexao.Desconectar();

            return data.DefaultView;
        }*/
    }
}
