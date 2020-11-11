using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tcc_windows_version.Database;
using tcc_windows_version.Model;

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
                MessageBox.Show("Preencha corretamente as lacunas!");
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
                MessageBox.Show("Preencha corretamente as lacunas!");
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
