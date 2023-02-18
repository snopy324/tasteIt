using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(Type_Detail))]
    public partial class Type_Detail
    {
        public class Type_DetailMetaData
        {
            public int ResTypeDet_ID { get; set; }
            [DisplayName("餐廳編號")]
            public Nullable<int> ResTypeDet_RestaurantID { get; set; }
            [DisplayName("食記編號")]
            public Nullable<int> ResTypeDet_NewsID { get; set; }
            [DisplayName("類別編號")]
            public Nullable<int> ResTypeDet_ResTypeID { get; set; }
            [DisplayName("子類別編號")]
            public Nullable<int> ResTypeDet_SubTypeID { get; set; }
        }
    }
}