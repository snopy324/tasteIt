using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Azure_0517.Models.Member;

namespace Azure_0517.Models
{
    [MetadataType(typeof(DiscountMetaData))]
    public partial class Discount
    {
        public class DiscountMetaData
        {
            [DisplayName("優惠編號")]
            public int Dis_ID { get; set; }
            [DisplayName("優惠主題")]
            [Required]
            public string Dis_Title { get; set; }
            [DisplayName("餐廳編號")]
            public int Dis_RestaurantID { get; set; }
            [DisplayName("優惠內容")]
            [Required]
            [DataType(DataType.MultilineText)]
            public string Dis_Content { get; set; }
            [DisplayName("開始時間")]
            [DataType(DataType.Date)]
            [Required]
            public System.DateTime Dis_Start { get; set; }
            [DisplayName("結束時間")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> Dis_End { get; set; }
        }
    }
}