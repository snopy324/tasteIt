using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;

namespace Azure_0517.Models.ViewModel
{
    public class rhp_default_viewmodel
    {
        public int _resid { get; set; }
        public IEnumerable<Discount> all_discount { get; set; }
        public IEnumerable<ReservationOpened> all_reservation { get; set; }
        public IEnumerable<Picture> all_photo { get; set; }
        public IEnumerable<News> all_news { get; set; }
        public IEnumerable<Comment> all_comments { get; set; }
    }
}