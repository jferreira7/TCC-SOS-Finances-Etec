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
        public string BuscarSaldo(int id)
        {
            if (id > 0) {
                ValoresRead valor = new ValoresRead();
                return valor.GetSaldo(id);
            } 
            else
            {
                return "";
            }
        }
    }
}
