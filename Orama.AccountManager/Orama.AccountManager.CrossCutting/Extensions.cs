using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.CrossCutting
{
    public static class Extensions
    {
        public static Exception GetInnerException(this Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            return ex;
        }
    }
}
