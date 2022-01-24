using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Core.Tools
{
    public class ReportsException : Exception
    {
        public ReportsException()
        {
        }

        public ReportsException(string message) :
            base(message)
        {
        }

        public ReportsException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
