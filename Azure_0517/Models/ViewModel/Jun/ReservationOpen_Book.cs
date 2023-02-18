using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class ReservationOpen_Book
    {
        public ReservationOpened Reservation { get; set; }
        public ReservationBooked Booked{ get; set; }
        public Member Member { get; set; }
        public string BackupData { get; set; }
        public static ReservationOpen_Book NewObject(ReservationOpened target, Member member)
        {
            ReservationOpen_Book open_Book = new ReservationOpen_Book
            {
                Reservation = target,
                Member = member,
                Booked = new ReservationBooked
                {
                    RVBooked_MemberID = member.Mem_ID,
                    RVBooked_ReservationOpenedID = target.RVOpen_ID,
                    RVBooked_BookedDate = target.RVOpen_DateTime
                },
            };
            return open_Book;
        }
    }
}