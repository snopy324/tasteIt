using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(MemberMetaData))]
    public partial class Member
    {
        public class MemberMetaData
        {
            [DisplayName("會員編號")]
            public int Mem_ID { get; set; }

            [DisplayName("會員名稱")]
            public string Mem_Name { get; set; }

            [DisplayName("會員帳號")]
            public string Mem_Account { get; set; }

            [DisplayName("會員密碼")]
            public string Mem_Password { get; set; }

            [DisplayName("會員性別")]
            public Nullable<bool> Mem_Gender { get; set; }

            [DisplayName("會員生日")]
            [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
            public System.DateTime Mem_Birthday { get; set; }

            [DisplayName("會員暱稱")]
            public string Mem_NickName { get; set; }

            [DisplayName("會員信箱")]
            public string Mem_Mail { get; set; }

            [DisplayName("會員電話")]
            public string Mem_Phone { get; set; }

            [DisplayName("會員住址")]
            public string Mem_Address { get; set; }

            [DisplayName("加入日期")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
            public System.DateTime Mem_JoinDate { get; set; }

            [DisplayName("會員經驗")]
            public int Mem_Exp { get; set; }

            [DisplayName("會員點數")]
            public int Mem_Point { get; set; }

            [DisplayName("會員頭像")]
            public byte[] Mem_Photo { get; set; }

            [DisplayName("會員自介")]
            [DataType(DataType.MultilineText)]
            public string Mem_Introduction { get; set; }

            [DisplayName("會員權限")]
            public int Mem_RoleID { get; set; }
        }
    }


}