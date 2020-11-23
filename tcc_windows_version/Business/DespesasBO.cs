using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Database;
using tcc_windows_version.Model;
using tcc_windows_version.View;

namespace tcc_windows_version.Business
{
    class DespesasBO
    {
        DespesasCRUD crud;
        public void Cadastrar(Despesas despesa)
        {
            
            if ((despesa.nome != "") && 
                (despesa.categoria != "") &&
                (despesa.data_vencimento != "") &&
                (despesa.estado != "") && 
                (despesa.valor != "") && 
                (despesa.id_usuario > 0))
            {
                crud = new DespesasCRUD();
                crud.Create(despesa);                
            }
            else
            {
                Mensagem.mensagemErro = "Preencha corretamente as lacunas!";
            }
        }        
        public void Editar(Despesas despesa)
        {
            if ((despesa.id != "") && (despesa.nome != "") && (despesa.categoria != "") && (despesa.estado != "") && (despesa.valor != "") && (despesa.id_usuario > 0))
            {
                crud = new DespesasCRUD();
                crud.Update(despesa);                
            }
            else
            {
                Mensagem.mensagemErro = "Selecione uma linha e preencha corretamente as lacunas!";
            }
        }
        public DataView Buscar(int id_usuario)
        {
            if (id_usuario > 0)
            {
                crud = new DespesasCRUD();
                return crud.Read(id_usuario);
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
                crud = new DespesasCRUD();
                crud.Delete(id);
            }
        }
        public DataView Filtrar(string nome, string empresa, string categoria, string mes, string ano, string estado, int id_usuario)
        {
            if ((nome != "" || empresa != "" || categoria != "" || mes != "" || ano != "" || estado != "") && id_usuario > 0)
            {
                crud = new DespesasCRUD();
                return crud.Search(nome, empresa, categoria, mes, ano, estado, id_usuario);
            } else {
                Mensagem.mensagemErro = "Preencha pelo menos um dos campos!";
                return null;
            }
        }
    }
}
