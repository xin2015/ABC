using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL
{
    public class Data
    {
        public string[] Codes { get; set; }
        public double? Value { get; set; }
    }

    public class ViewData
    {
        public string[] Codes { get; set; }
        public string Value { get; set; }
    }
}
