using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcc_windows_version.Database;

namespace tcc_windows_version.Business
{
    class OutrosBO
    {
        public string BuscarSaldo(int id)
        {
            if (id > 0) {
                Outros outros = new Outros();
                return outros.GetSaldo(id);
            } 
            else
            {
                return "";
            }
        }
    }
}
