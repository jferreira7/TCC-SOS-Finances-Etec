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
                (objetivo.image_bytes != null) && 
                (objetivo.valor_mes != "" || Convert.ToDecimal(objetivo.valor_mes) > 0) && 
                (objetivo.valor_inicial != "" || Convert.ToDecimal(objetivo.valor_inicial) > 0) &&
                (objetivo.id_usuario > 0))
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.Create(objetivo);
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
    }
}
