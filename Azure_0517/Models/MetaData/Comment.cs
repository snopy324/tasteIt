using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Azure_0517.Models.Member;

namespace Azure_0517.Models
{
    [MetadataType(typeof(CommentMetaData))]
    public partial class Comment
    {
        public class CommentMetaData
        {
            [DisplayName("評論編號")]
            public int Com_ID { get; set; }
            [DisplayName("使用者編號")]
            public int Com_MemberID { get; set; }
            [DisplayName("餐廳編號")]
            public int Com_RestaurantID { get; set; }
            [DisplayName("食記編號")]
            public Nullable<int> Com_NewsID { get; set; }
            [DisplayName("評論內容")]
            public string Com_Content { get; set; }
            [DisplayName("評論時間")]
            public System.DateTime Com_CreateDate { get; set; }
            [DisplayName("分數")]
            public int Com_StarRate { get; set; }

        }
    }
}