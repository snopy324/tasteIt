using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;
namespace Azure_0517.Models.ViewModel
{
    public class AdmQAViewModel
    {
        public IEnumerable<QA_system> qA_SystemsNotice { get; set; }
        public IEnumerable<QA_system> qA_SystemsMemNotice { get; set; }
        public IEnumerable<QA_system> qA_SystemsResNotice { get; set; }
        public IEnumerable<QA_system> QA_SystemsData { get; set; }
        public Dictionary<int?, string> QA_SystemsNameData { get; set; }
        public Dictionary<int?, string> QA_SystemsResNameData { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }


    }
}