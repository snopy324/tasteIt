using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(StatusTypeMetaData))]
    public partial class StatusType
    {
        public class StatusTypeMetaData
    {
        [DisplayName("狀態編號")]
        public int Sta_ID { get; set; }
        [DisplayName("狀態內容")]
        public string Sta_Context { get; set; }
    }
    }
 
    
}