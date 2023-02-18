using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Azure_0517.Models.Member;

namespace Azure_0517.Models
{
    [MetadataType(typeof(AccuseTypeMetaData))]
    public partial class AccuseType
    {
    
        public class AccuseTypeMetaData
        {
            [DisplayName("檢舉類型")]
            public int RepTyp_ID { get; set; }
            [DisplayName("檢舉內容")]
            public string RepTyp_Content { get; set; }
        }
    }
}