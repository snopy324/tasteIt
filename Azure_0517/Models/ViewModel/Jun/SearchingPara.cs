using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models.Repository;

namespace Azure_0517.Models.ViewModel
{
    public static class SearchingCheckBoxItem
    {
        static CityRepository CityRepository = new CityRepository();
        static TheTypeRepository TheTypeRepository = new TheTypeRepository();
        static SubTypeRepository SubTypeRepository = new SubTypeRepository();


        public static IEnumerable<City> Cities { get {return CityRepository.GetAll(); } }
        public static IEnumerable<TheType> theTypes { get { return TheTypeRepository.GetAll(); } }
        public static IEnumerable<SubType> subTypes { get { return SubTypeRepository.GetAll(); } }
    }
}