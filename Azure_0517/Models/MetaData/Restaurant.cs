using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(RestaurantMetaData))]
    public partial class Restaurant
    {
        public class RestaurantMetaData
        {
            [DisplayName("餐廳編號")]
            public int Res_ID { get; set; }
            [DisplayName("餐廳名稱")]
            public string Res_Name { get; set; }
            [DisplayName("餐廳帳號")]
            public string Res_Account { get; set; }
            [DisplayName("餐廳密碼")]
            public string Res_Password { get; set; }
            [DisplayName("餐廳地址")]
            public string Res_Address { get; set; }
            [DisplayName("餐廳信箱")]
            public string Res_Email { get; set; }
            [DisplayName("所在城市")]
            public Nullable<int> Res_City { get; set; }
            [DisplayName("餐廳電話")]
            public string Res_Phone { get; set; }
            [DisplayName("餐廳消費金額(高)")]
            public Nullable<int> Res_MaxConsumption { get; set; }
            [DisplayName("餐廳消費金額(低)")]
            public Nullable<int> Res_MinConsumption { get; set; }
            [DisplayName("餐廳評價星等")]
            public Nullable<decimal> Res_StarRate { get; set; }
            [DisplayName("餐廳介紹")]
            public string Res_IntroductionContext { get; set; }
            [DisplayName("餐廳營業日")]
            public string Res_WorkingDate { get; set; }
            [DisplayName("付款方式")]
            public byte[] Res_PayBy { get; set; }
            [DisplayName("餐廳座位")]
            public Nullable<int> Res_Seats { get; set; }
            [DisplayName("餐廳提供停車位數量")]
            public Nullable<int> Res_ParkingSeats { get; set; }
            [DisplayName("10%服務費")]
            public Nullable<bool> Res_TenPercentService { get; set; }
            [DisplayName("餐廳照片")]
            public byte[] Res_HomePagePicture { get; set; }
            [DisplayName("餐廳權限")]
            public int Res_RoleID { get; set; }
            [DisplayName("餐廳加入日期")]
            [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}")]
            public System.DateTime Res_CreateDate { get; set; }
            [DisplayName("餐廳能否被提問")]
            public bool Res_QAEnable { get; set; }
            [DisplayName("餐廳菜式編號")]
            public string Res_TaxID { get; set; }

        }

        
    }
}