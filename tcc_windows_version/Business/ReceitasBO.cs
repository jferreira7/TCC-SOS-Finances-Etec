using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Database;
using tcc_windows_version.Model;
using tcc_windows_version.View;

namespace tcc_windows_version.Business
{
    class ReceitasBO
    {
        public void Cadastrar(Receitas receita)
        {

            if ((receita.descricao != "") && (receita.categoria != "") && (receita.valor != "") && (receita.id_usuario > 0))
            {
                ReceitasCRUD crud = new ReceitasCRUD();
                crud.Create(receita);
            }
            else
            {                
                Mensagem.mensagemErro = "Preencha corretamente as lacunas!";
            }
        }
        public void Editar(Receitas receita)
        {
            if ((receita.id != "") && (receita.descricao != "") && (receita.categoria != "") && (receita.valor != "") && (receita.id_usuario > 0))
            {
                ReceitasCRUD crud = new ReceitasCRUD();
                crud.Update(receita);
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
                ReceitasCRUD crud = new ReceitasCRUD();
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
                ReceitasCRUD crud = new ReceitasCRUD();
                crud.Delete(id);
            }
        }
    }
}
