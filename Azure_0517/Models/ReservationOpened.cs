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
    
    public partial class ReservationOpened
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReservationOpened()
        {
            this.ReservationBookeds = new HashSet<ReservationBooked>();
        }
    
        public int RVOpen_ID { get; set; }
        public string RVOpen_OffPriceTitle { get; set; }
        public int RVOpen_RestaurantID { get; set; }
        public int RVOpen_Seats { get; set; }
        public System.DateTime RVOpen_DateTime { get; set; }
        public string RVOpen_OffPrice { get; set; }
        public int RVOpen_TeamHeader { get; set; }
        public int RVOpen_SeatsRemain { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationBooked> ReservationBookeds { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
