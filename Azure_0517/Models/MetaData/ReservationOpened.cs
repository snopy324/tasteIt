using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class ReservationOpened
    {
        public class MemberMetaData
        {
            [DisplayName("開放預約編號")]
            public int RVOpen_ID { get; set; }
            [DisplayName("優惠標題")][Required]
            public string RVOpen_OffPriceTitle { get; set; }
            [DisplayName("預約餐廳編號")]
            public int RVOpen_RestaurantID { get; set; }
            [DisplayName("預約保留座位")][Required]
            public int RVOpen_Seats { get; set; }
            [DisplayName("預約日期")][Required]
            public System.DateTime RVOpen_DateTime { get; set; }
            [DisplayName("優惠")]
            public string RVOpen_OffPrice { get; set; }

            [DisplayName("預約保留座位")]
            public int RVOpen_SeatsRemain { get; set; }
        }
        public string OffpriceReadme
        {
            get
            {
                if (this.RVOpen_OffPrice[0] == '+')
                {
                    return $"用餐後將獲得{this.RVOpen_OffPrice}紅利回饋";
                }
                else
                {
                    return $"用餐將獲得{this.RVOpen_OffPrice}優惠";
                }
            }
        }
    }

}