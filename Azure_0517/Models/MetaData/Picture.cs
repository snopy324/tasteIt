using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(PictureMetaData))]
    public partial class Picture {
        public class PictureMetaData
        {
            [DisplayName("照片編號")]
            public int Pic_ID { get; set; }
            public Nullable<int> Pic_NewsID { get; set; }
            public Nullable<int> Pic_Restaurant { get; set; }
            public byte[] Pic_Picture { get; set; }
            [DisplayName("照片描述")]
            public string Pic_Content { get; set; }
            [DisplayName("照片標題")]
            public string Pic_title { get; set; }


        }
    }


}