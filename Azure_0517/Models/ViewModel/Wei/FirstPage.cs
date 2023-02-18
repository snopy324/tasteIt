using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models.ViewModel;
using Azure_0517.Models.Repository;

namespace Azure_0517.Models.ViewModel
{
    public class FirstPage
    {
        public IEnumerable<News> Hotnews { get; set; }
        public IEnumerable<News> Newinnews { get; set; }
        public IEnumerable<News> ClickRatenews { get; set; }
        public IEnumerable<News> Membernews { get; set; }
        public IEnumerable<Restaurant> Star { get; set; }
        public IEnumerable<Restaurant> Special { get; set; }
        public IEnumerable<Discount> DiscountCube { get; set; }
    }

}