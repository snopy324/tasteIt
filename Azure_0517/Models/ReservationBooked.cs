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
    
    public partial class ReservationBooked
    {
        public int RVBooked_ID { get; set; }
        public System.DateTime RVBooked_BookedDate { get; set; }
        public Nullable<int> RVBooked_DiscountID { get; set; }
        public Nullable<int> RVBooked_ReservationOpenedID { get; set; }
        public int RVBooked_MemberID { get; set; }
        public int RVBooked_Seats { get; set; }
        public string RVBooked_ReminderContext { get; set; }
        public Nullable<System.DateTime> RVBooked_CheckinTime { get; set; }
    
        public virtual Discount Discount { get; set; }
        public virtual Member Member { get; set; }
        public virtual ReservationOpened ReservationOpened { get; set; }
    }
}
