using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class Gift
    {
        public class MemberMetaData
        {
            [DisplayName("獎品編號")]
            public int Gift_ID { get; set; }

            [DisplayName("獎品名稱")]
            [Required]
            public string Gift_Name { get; set; }

            [DisplayName("獎品描述")]
            [Required]
            public string Gift_Content { get; set; }

            [DisplayName("兌換點數")]
            [Required]
            public int Gift_Point { get; set; }

            [DisplayName("創建人員")]
            public int Gift_CreatedAdmini { get; set; }

            [DisplayName("創建日期")]
            public System.DateTime Gift_CreatedDate { get; set; }

            [DisplayName("獎品數量")]
            [Required]
            public Nullable<int> Gift_Quantity { get; set; }

            [DisplayName("獎品照片")]
            public byte[] Gift_Picture { get; set; }
        }
    }
}