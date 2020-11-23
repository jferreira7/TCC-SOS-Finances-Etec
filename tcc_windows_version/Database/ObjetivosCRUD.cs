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
            objComando.CommandText = "insert into objetivos (estado, nome, preco, imagem, porcentagem, valor_guardado, valor_restante, id_usuario) VALUES (@estado, @nome, @preco, @imagem, @porcentagem, @valor_guardado, @valor_restante, @id_usuario);";

            objComando.Parameters.Add("@estado", MySqlDbType.VarChar, 10).Value = objetivo.estado;
            objComando.Parameters.Add("@nome", MySqlDbType.VarChar, 8).Value = objetivo.nome;
            objComando.Parameters.Add("@preco", MySqlDbType.Decimal, 12).Value = objetivo.preco;
            objComando.Parameters.Add("@imagem", MySqlDbType.MediumBlob).Value = objetivo.imagem_bytes;
            objComando.Parameters.Add("@porcentagem", MySqlDbType.Decimal).Value = objetivo.porcentagem;
            objComando.Parameters.Add("@valor_guardado", MySqlDbType.Decimal, 12).Value = objetivo.valor_guardado;
            objComando.Parameters.Add("@valor_restante", MySqlDbType.Decimal, 12).Value = (Convert.ToDecimal(objetivo.preco) - Convert.ToDecimal(objetivo.valor_guardado));
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
        }
        public void Update(Objetivos objetivo)
        {
            objComando.CommandText = "update objetivos set estado = @estado, nome = @nome, preco = @preco, imagem = @imagem where id = @id and id_usuario = @id_usuario;";

            objComando.Parameters.Clear();
            objComando.Parameters.Add("@id", MySqlDbType.Int32).Value = objetivo.id;
            objComando.Parameters.Add("@estado", MySqlDbType.VarChar, 10).Value = objetivo.estado;
            objComando.Parameters.Add("@nome", MySqlDbType.VarChar, 8).Value = objetivo.nome;
            objComando.Parameters.Add("@preco", MySqlDbType.Decimal, 12).Value = objetivo.preco;
            objComando.Parameters.Add("@imagem", MySqlDbType.MediumBlob).Value = objetivo.imagem_bytes;
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = objetivo.id_usuario;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Objetivo atualizado!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }
        public DataView Read(int idUsuario)
        {
            MySqlDataAdapter adpt;
            DataTable data = new DataTable("objetivos");

            string anoAtual = DateTime.Now.Year.ToString();
            adpt = new MySqlDataAdapter("select * from objetivos where id_usuario = " + idUsuario + " and estado in('Andamento','Finalizado');", objConexao.Conexao());
            adpt.Fill(data);
            objConexao.Desconectar();
            return data.DefaultView;
        }
        public void Delete(int id)
        {
            objComando.CommandText = "delete from objetivos where id = @id";
            objComando.Parameters.Add("@id", MySqlDbType.Int32, 11).Value = id;

            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Objetivo deletado!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro de conexão com o servidor: " + erro);
            }
        }
        public void UpdateValorGuardado(int id, int id_usuario, double valor_guardado)
        {
            objComando.CommandText = "update objetivos set valor_guardado=@valor_guardado where id=@id and id_usuario=@id_usuario;";
            
            objComando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;            
            objComando.Parameters.Add("@valor_guardado", MySqlDbType.Decimal, 12).Value = valor_guardado;            
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = id_usuario;
            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Objetivo atualizado!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }
        public void UpdateValorInicial(int id, int id_usuario, double valor_inicial)
        {
            objComando.CommandText = "update objetivos set valor_inicial=@valor_inicial where id=@id and id_usuario=@id_usuario;";

            objComando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            objComando.Parameters.Add("@valor_inicial", MySqlDbType.Decimal, 12).Value = valor_inicial;
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = id_usuario;
            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Objetivo atualizado!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }
        public DataView OneSelection(int id)
        {
            MySqlDataAdapter adpt;
            DataTable data = new DataTable("objetivos");

            string anoAtual = DateTime.Now.Year.ToString();
            adpt = new MySqlDataAdapter("select * from objetivos where id = " + id + " limit 1;", objConexao.Conexao());
            adpt.Fill(data);
            objConexao.Desconectar();
            return data.DefaultView;
        }
        public void UpdateEstado(int id, int id_usuario, string estado)
        {
            objComando.CommandText = "update objetivos set estado=@estado where id=@id and id_usuario=@id_usuario;";

            objComando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            objComando.Parameters.Add("@estado", MySqlDbType.VarChar, 10).Value = estado;
            objComando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = id_usuario;
            try
            {
                objConexao.Conexao();
                objComando.Connection = objConexao.Conectar();
                objComando.ExecuteNonQuery();
                objConexao.Desconectar();
                MessageBox.Show("Objetivo atualizado!");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro conexão com o servidor: " + erro);
            }
        }        
    }
}
