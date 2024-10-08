using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.Framework.Exceptions
{
  
    public class EmailException : Exception
    {
        public EmailException()
        {
        }

        public EmailException(string message, Exception ex)
            : base(message, ex)
        {
        }
 
    }
}
