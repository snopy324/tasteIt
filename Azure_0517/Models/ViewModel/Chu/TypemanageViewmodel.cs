using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class TypemanageViewmodel
    {
        public Restaurant res { get; set; }
        public IEnumerable<string> nonthetype { get; set; }
        public IEnumerable<string> nonsubtype { get; set; }
        public IEnumerable<string> Resthetypes { get; set; }
        public IEnumerable<string> RessubTypes { get; set; }
    }
}