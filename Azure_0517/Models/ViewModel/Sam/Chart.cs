using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_0513.Models.ViewModel.Sam
{
    public class GenderPie
    {
        public int Male { get; set; }
        public int Female { get; set; }
    }
    public class PricePie
    {
        public int Low { get; set; }
        public int Medium { get; set; }
        public int Expensive { get; set; }
        public int VeryExpensive { get; set; }
    }

    public class MemberAge
    {
        public int FifTwe { get; set; }
        public int TweThi { get; set; }
        public int ThiFou { get; set; }
        public int FouFif { get; set; }
        public int FifSix { get; set; }
        public int Elder { get; set; }
    }
}