using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Azure_0517.Models
{  [MetadataType(typeof(SubTypeMetaData))]
    public partial class SubType
    {
        public class SubTypeMetaData
        {
            [DisplayName("子分類編號")]
            public int SubT_ID { get; set; }
            [DisplayName("子分類項目")]
            public string SubT_Contect { get; set; }
            
            public int SubT_ResTypeID { get; set; }

            [ScriptIgnore]
            public virtual TheType TheType { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            [ScriptIgnore]
            public virtual ICollection<Type_Detail> Type_Detail { get; set; }
        }
    }

}