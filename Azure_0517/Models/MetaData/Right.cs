using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class Right
    {
        public class MemberMetaData
        {
            [DisplayName("權限編號")]
            public int Right_ID { get; set; }
            [DisplayName("權限名稱")]
            public string Right_Name { get; set; }
        }
    }
}