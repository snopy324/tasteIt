using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class DisSearchPara
    {
        public IEnumerable<TheType> theTypes { get; set; }
        public IEnumerable<SubType> subTypes { get; set; }
        public IEnumerable<City> cities { get; set; }
        public IEnumerable<Discount> discounts { get; set; }
    }
    public class NewsSearchPara
    {
        public IEnumerable<TheType> theTypes { get; set; }
        public IEnumerable<SubType> subTypes { get; set; }
        public IEnumerable<City> cities { get; set; }
        public IEnumerable<News> news { get; set; }
    }

}