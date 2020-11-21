using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcc_windows_version.Database;

namespace tcc_windows_version.Business
{
    class ValoresBO
    {
        public string Validacao(int id, string coluna)
        {
            if (id > 0)
            {
                ValoresRead valor = new ValoresRead();
                if (coluna == "saldo")
                    return valor.GetSaldo(id, coluna);
                else if (coluna == "reserva")
                    return valor.GetReserva(id, coluna);
                else
                    return valor.GetPoupanca(id, coluna);
            }
            else
            {
                return "";
            }
        }
        public string BuscarSaldo(int id)
        {
            return Validacao(id, "saldo");
        }
        public string BuscarReserva(int id)
        {
            return Validacao(id, "reserva");
        }
        public string BuscarPoupanca(int id)
        {
            return Validacao(id, "poupanca");
        }
    }
}
