using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class Gift_Detail
    {
        public class MemberMetaData
        {
            [DisplayName("明細編號")]
            public int GiftDet_ID { get; set; }

            [DisplayName("會員編號")]
            public int GiftDet_MemberID { get; set; }

            [DisplayName("獎品編號")]
            public int GiftDet_GiftID { get; set; }

            [DisplayName("兌換日期")]
            public Nullable<System.DateTime> GiftDet_GetDate { get; set; }

            [DisplayName("兌換數量")]
            public int GiftDet_GetQuantity { get; set; }
        }
    }
}