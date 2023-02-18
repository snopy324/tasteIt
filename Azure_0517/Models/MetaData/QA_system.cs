using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Azure_0517.Models
{
    [MetadataType(typeof(QA_systemMetaData))]
    public partial class QA_system
    {
        public class QA_systemMetaData
        {
            [DisplayName("問題編號")]
            public int QA_ID { get; set; }
            public Nullable<int> QA_MemberID { get; set; }
            public Nullable<int> QA_AdministratorID { get; set; }
            public Nullable<int> QA_RestaurantID { get; set; }
            [DisplayName("發問日期")]
            public System.DateTime QA_CreateDate { get; set; }
            [DisplayName("解決時間")]
            public Nullable<System.DateTime> QA_AnswerdDate { get; set; }
            public string QA_Content { get; set; }
            public string QA_Answer { get; set; }
        }
    }


}