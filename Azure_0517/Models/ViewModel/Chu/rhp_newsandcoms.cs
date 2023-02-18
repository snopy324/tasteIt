using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;
namespace Azure_0517.Models.ViewModel
{
    public class rhp_newsandcoms
    {
        public IEnumerable<News> r_news { get; set; }
        public IEnumerable<Comment> r_coms { get; set; }
    }
}