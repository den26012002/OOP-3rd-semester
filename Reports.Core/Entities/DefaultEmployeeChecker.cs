using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Core.Entities
{
    public class DefaultEmployeeChecker : IEmployeeChecker
    {
        public bool Check(Employee employee)
        {
            return true;
        }
    }
}
