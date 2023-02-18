using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class Role
    {
        public class MemberMetaData
        {
            [DisplayName("身分編號")]
            public int Role_ID { get; set; }
            [DisplayName("身分名稱")]
            public string Role_Name { get; set; }
        }
    }
}