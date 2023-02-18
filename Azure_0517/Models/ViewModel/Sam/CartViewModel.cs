using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class CartViewModel
    {
        //GiftDetail
        public int GiftDet_ID { get; set; }
        public int GiftID { get; set; }

        [DisplayName("兌換數量")]
        public int GetQuantity { get; set; }

        [DisplayName("兌換日期")]
        public DateTime? GetDate { get; set; }

        //Gift
        [DisplayName("獎品照片")]
        public byte[] Picture { get; set; }

        [DisplayName("獎品名稱")]
        public string Name { get; set; }

        [DisplayName("兌換點數")]
        public int Point { get; set; }

        public int LastQuantity { get; set; }

        //Member
        public int MemberID { get; set; }
        public int MemberPoint{ get; set; }

        //小計
        [DisplayName("小計")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal Amount {
            get
            {
                return this.GetQuantity * this.Point;
            }
        }
    }
}