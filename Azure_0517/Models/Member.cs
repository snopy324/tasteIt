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
    
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            this.Comments = new HashSet<Comment>();
            this.Gifts = new HashSet<Gift>();
            this.Gift_Detail = new HashSet<Gift_Detail>();
            this.News = new HashSet<News>();
            this.QA_system = new HashSet<QA_system>();
            this.QA_system1 = new HashSet<QA_system>();
            this.Reports = new HashSet<Report>();
            this.Reports1 = new HashSet<Report>();
            this.ReservationBookeds = new HashSet<ReservationBooked>();
        }
    
        public int Mem_ID { get; set; }
        public string Mem_Name { get; set; }
        public string Mem_Account { get; set; }
        public string Mem_Password { get; set; }
        public Nullable<bool> Mem_Gender { get; set; }
        public System.DateTime Mem_Birthday { get; set; }
        public string Mem_NickName { get; set; }
        public string Mem_Mail { get; set; }
        public string Mem_Phone { get; set; }
        public string Mem_Address { get; set; }
        public System.DateTime Mem_JoinDate { get; set; }
        public int Mem_Exp { get; set; }
        public int Mem_Point { get; set; }
        public byte[] Mem_Photo { get; set; }
        public string Mem_Introduction { get; set; }
        public int Mem_RoleID { get; set; }
        public string Mem_Source { get; set; }
        public string Mem_SourceID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gift> Gifts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gift_Detail> Gift_Detail { get; set; }
        public virtual Role Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QA_system> QA_system { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QA_system> QA_system1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationBooked> ReservationBookeds { get; set; }
    }
}
