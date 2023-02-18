using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Azure_0517.Models.Member;

namespace Azure_0517.Models
{
    [MetadataType(typeof(CityMetaData))]
    [DisplayName("城市")]
    public partial class City
    {
        public class CityMetaData
        {
            [DisplayName("都市編號")]
            public int Cit_ID { get; set; }
            [DisplayName("都市")]
            public string Cit_Name { get; set; }

        }
    }
}