using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class ReservationSearch
    {
        public ReservationSearch()
        {
            Date = DateTime.Now;
            Time = DateTime.Now;
        }
        public ReservationSearch  NewObject(HttpRequestBase Request)
        {
            
            DateTime date;
            DateTime time;
            this.Date = DateTime.TryParse(Request.QueryString["Item2.Date"], out date) ? date : DateTime.Now;
            this.Time = DateTime.TryParse(Request.QueryString["Item2.Time"], out time) ? time : DateTime.Now;
            int tempPrice;
            if (int.TryParse(Request.QueryString["Item2.Price"], out tempPrice))
            {
                this.Price = tempPrice;
            }
            int tempLocation;
            if (int.TryParse(Request.QueryString["Item2.Location"], out tempLocation))
            {
                this.Location = tempLocation;
            }
            int tempSeats = 0;
            int.TryParse(Request.QueryString["Item2.Seats"], out tempSeats);
            this.Seats = tempSeats;

            return this;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="yyyy-MM-dd")]
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString = "HH:mm")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public int? Price { get; set; }
        public int? Location { get; set; }

    }
}