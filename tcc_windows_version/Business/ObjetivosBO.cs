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
                (objetivo.valor_guardado != "" || Convert.ToDecimal(objetivo.valor_guardado) >= 0) &&
                (objetivo.id_usuario > 0))
            {
                if (Convert.ToDecimal(objetivo.preco) != Convert.ToDecimal(objetivo.valor_guardado))
                {
                    objetivo.estado = "Andamento";
                }
                else
                {
                    objetivo.estado = "Finalizado";
                }
                objetivo.porcentagem = Math.Round(((Convert.ToDouble(objetivo.valor_guardado) * 100) / Convert.ToDouble(objetivo.preco)), 2, MidpointRounding.AwayFromZero);
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
            if ((objetivo.id > 0) &&
                (objetivo.nome != "") &&
                (objetivo.preco != "" || Convert.ToDecimal(objetivo.preco) > 0) &&
                (objetivo.valor_guardado != "" || Convert.ToDecimal(objetivo.valor_guardado) >= 0) &&
                (objetivo.imagem_bytes != null) &&
                (objetivo.id_usuario > 0))
            {
                //Preço maior que o vg --> alterar ou manter o estado
                //Preço igual ao vg --> alterar o estado
                //Preço menor que o vg --> alterar o saldo, a reserva e o estado(alterar ou manter)
                ObjetivosCRUD crud = new ObjetivosCRUD();
                if (Convert.ToDecimal(objetivo.preco) > Convert.ToDecimal(objetivo.valor_guardado))
                {
                    if (objetivo.estado != "Andamento")
                    {                        
                        objetivo.estado = "Andamento";                        
                    }
                    crud.Update(objetivo);                    
                }
                else if (Convert.ToDecimal(objetivo.preco) == Convert.ToDecimal(objetivo.valor_guardado))
                {
                    if (objetivo.estado != "Finalizado")
                    {
                        objetivo.estado = "Finalizado";
                    }
                    crud.Update(objetivo);                    
                }
                else //Convert.ToDecimal(objetivo.preco) < Convert.ToDecimal(objetivo.valor_guardado)
                {
                    if (objetivo.estado != "Finalizado")
                    {
                         objetivo.estado = "Finalizado";
                    }
                    objetivo.valor_guardado = objetivo.preco;
                    crud.UpdateValorGuardado(objetivo.id, objetivo.id_usuario, Convert.ToDouble(objetivo.valor_guardado));
                    crud.Update(objetivo);
                }
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
        public void AtualizarValorGuardado(int id, int id_usuario, double valor_guardado)
        {
            if (id > 0 && id_usuario > 0 && valor_guardado >= 0)
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.UpdateValorGuardado(id, id_usuario, valor_guardado);
            }
            else
            {
                MessageBox.Show("Selecione uma linha e preencha corretamente a lacuna do valor a ser guardado.");
            }
        }
        public DataView SelecionarUm(int id)
        {
            if (id > 0)
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                return crud.OneSelection(id);
            }
            else
            {
                return null;
            }
        }
        public void AtualizarEstado(int id, int id_usuario, string estado)
        {
            if (id > 0 && id_usuario > 0 && estado == "Comprado")
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.UpdateEstado(id, id_usuario, estado);
            }
            else
            {
                MessageBox.Show("Erro ao debitar o objetivo.");
            }
        }
        /*
        public void AtualizarValorInicial(int id, int id_usuario, double valor_inicial)
        {
            if (id > 0 && id_usuario > 0 && valor_inicial >= 0)
            {
                ObjetivosCRUD crud = new ObjetivosCRUD();
                crud.UpdateValorInicial(id, id_usuario, valor_inicial);
            }
            else
            {
                MessageBox.Show("Selecione uma linha e preencha corretamente a lacuna do valor a ser guardado.");
            }
        }*/
    }
}
