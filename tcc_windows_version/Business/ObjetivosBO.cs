using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Database;
using tcc_windows_version.Model;

namespace tcc_windows_version.Business
{
    class ObjetivosBO
    {
        public void Cadastrar(Objetivos objetivo)
        {

            if ((objetivo.nome != "") && 
                (objetivo.preco != "" || Convert.ToDecimal(objetivo.preco) > 0) && 
                (objetivo.imagem_bytes != null) &&                 
                (objetivo.valor_inicial != "" || Convert.ToDecimal(objetivo.valor_inicial) > 0) &&
                (objetivo.id_usuario > 0))
            {
                objetivo.porcentagem = Math.Round(((Convert.ToDouble(objetivo.valor_inicial) * 100) / Convert.ToDouble(objetivo.preco)), 2, MidpointRounding.AwayFromZero);
                MessageBox.Show(objetivo.porcentagem.ToString());

                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.Create(objetivo);
            }
            else
            {
                MessageBox.Show("Preencha corretamente as lacunas!");
            }
        }
        public void Editar(Objetivos objetivo)
        {
            if ((objetivo.nome != "") &&
                (objetivo.preco != "" || Convert.ToDecimal(objetivo.preco) > 0) &&
                (objetivo.imagem_bytes != null) &&
                (objetivo.id_usuario > 0))
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.Update(objetivo);
            }
            else
            {
                MessageBox.Show("Preencha corretamente as lacunas!");
            }
        }
        public DataView BuscarTodos (int idUsuario)
        {
            if (idUsuario > 0)
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                return crud.Read(idUsuario);
            }
            else
            {                
                return null;
            }
        }
        public void Deletar(int id)
        {
            if (id > 0)
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.Delete(id);
            }
        }
    }
}
