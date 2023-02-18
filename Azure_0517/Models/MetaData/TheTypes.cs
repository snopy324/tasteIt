using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(TheTypeMetaData))]
    public partial class TheType
    {
        public class TheTypeMetaData
        {
            [DisplayName("類型編號")]
            public int ResType_ID { get; set; }
            [DisplayName("類型內容")]
            public string ResType_Style { get; set; }
        }
    }
}