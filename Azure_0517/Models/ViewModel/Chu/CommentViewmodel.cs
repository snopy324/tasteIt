using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;

namespace Azure_0517.Models.ViewModel
{
    public class CommentViewmodel
    {
        public Restaurant Restaurant { get; set; }
        public Comment Comment { get; set; }
        public IEnumerable<Comment> rescommemt { get; set; }
        public string thename { get; set; }
        public int? thekey { get; set; }
        public int therole { get; set; }
        public int? mlove { get; set; }
        public int? mcom { get; set; }
        public Geometry geometry { get; set; }
        public int resself { get; set; }
    }
}