using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabler
{
    static class Extensions
    {


        public static bool IsEmpty(this ExcelRange cell)
        {
            return ((cell.Value == null) || string.IsNullOrEmpty(cell.Value.ToString()));

        }
           


    }
}
