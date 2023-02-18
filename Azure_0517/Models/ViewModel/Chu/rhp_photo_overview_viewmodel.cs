using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;

namespace Azure_0517.Models.ViewModel
{
    public class rhp_photo_overview_viewmodel
    {
        public IEnumerable<Picture> rhp_po_self { get; set; }
        public IEnumerable<News> rhp_po_news { get; set; }
    }
}