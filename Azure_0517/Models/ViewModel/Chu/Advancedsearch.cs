using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class Advancedsearch
    {
        //搜尋地區、搜尋訂位、搜尋優惠、搜尋父類別、搜尋子類別、價格區間
        public IEnumerable<TheType> theTypes { get; set; }
        public IEnumerable<SubType> subTypes { get; set; }
        public IEnumerable<City> cities { get; set; }
        public IEnumerable<Discount> discounts { get; set; }
        public IEnumerable<ReservationOpened> reservationOpeneds { get; set; }
        public int maxcost { get; set; }
    }
}