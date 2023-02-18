using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class News
    {

        public List<string> GetImg
        {
            get
            {

                string[] str = { "<img" };
                List<string> anwser = new List<string>();
                if (this.New_Content != null)
                {
                    foreach (string img in this.New_Content.Split(str, StringSplitOptions.None))
                        anwser.Add("<img " + img.Split('>')[0] + ">");
                }
                if (anwser.Count > 0)
                {
                    anwser.RemoveAt(0);
                }
                
                //return anwser;
                return anwser;

            }
        }

        public class MemberMetaData
        {
            [DisplayName("食記編號")]
            public int New_ID { get; set; }

            [DisplayName("食記標題")]
            public string New_Title { get; set; }

            [DisplayName("食記內容")][AllowHtml]
            public string New_Content { get; set; }

            [DisplayName("創建日期")]
            public Nullable<System.DateTime> New_CreateDate { get; set; }

            [DisplayName("會員編號")]
            public Nullable<int> New_MemberID { get; set; }

            [DisplayName("餐廳編號")]
            public int New_RestaurantID { get; set; }

            [DisplayName("點擊數")]
            public Nullable<int> New_ClickRate { get; set; }

            [DisplayName("申請日期")]
            public System.DateTime New_OrderDate { get; set; }

            [DisplayName("要求內容")]
            public string New_OrderContent { get; set; }
        }
    }
}