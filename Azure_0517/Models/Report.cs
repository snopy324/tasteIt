//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Azure_0517.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int Rep_ID { get; set; }
        public Nullable<int> Rep_MemberID { get; set; }
        public Nullable<int> Rep_AdministratorID { get; set; }
        public Nullable<int> Rep_RestaurantID { get; set; }
        public Nullable<int> Rep_NewsID { get; set; }
        public Nullable<int> Rep_CommentID { get; set; }
        public Nullable<int> Rep_AccuseTypeID { get; set; }
        public string Rep_Content { get; set; }
        public System.DateTime Rep_CreatedDate { get; set; }
        public Nullable<int> Rep_Status { get; set; }
        public Nullable<System.DateTime> Rep_SolvedDate { get; set; }
    
        public virtual AccuseType AccuseType { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Member Member { get; set; }
        public virtual Member Member1 { get; set; }
        public virtual News News { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual StatusType StatusType { get; set; }
    }
}
