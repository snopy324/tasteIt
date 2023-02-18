using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(ReportMetaData))]
    public partial class Report
    {
        public class ReportMetaData
        {
            public int Rep_ID { get; set; }   
            public Nullable<int> Rep_MemberID { get; set; }
            public Nullable<int> Rep_AdministratorID { get; set; }  
            public Nullable<int> Rep_RestaurantID { get; set; }
            public Nullable<int> Rep_NewsID { get; set; }
            public Nullable<int> Rep_CommentID { get; set; }
            public Nullable<int> Rep_AccuseTypeID { get; set; }
            [DisplayName("回報內容")]            
            public string Rep_Content { get; set; }
            [DisplayName("回報日期")]
            public System.DateTime Rep_CreatedDate { get; set; }  
            public Nullable<int> Rep_Status { get; set; }
            [DisplayName("解決日期")]
            public Nullable<System.DateTime> Rep_SolvedDate { get; set; }

            [JsonIgnore]
            public virtual AccuseType AccuseType { get; set; }
            [JsonIgnore]
            public virtual Comment Comment { get; set; }
            [JsonIgnore]
            public virtual Member Member { get; set; }
            [JsonIgnore]
            public virtual Member Member1 { get; set; }
            [JsonIgnore]
            public virtual News News { get; set; }
            [JsonIgnore]
            public virtual Restaurant Restaurant { get; set; }
            [JsonIgnore]
            public virtual StatusType StatusType { get; set; }
        }
    }


}