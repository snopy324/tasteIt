using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models
{
    [MetadataType(typeof(ReservationBookedMetaData))]
    public partial class ReservationBooked
    {
        public class ReservationBookedMetaData
        {
            [DisplayName("訂位編號")]
            public int RVBooked_ID { get; set; }
            [DisplayName("下訂日期")]
            public System.DateTime RVBooked_BookedDate { get; set; }
            public Nullable<int> RVBooked_DiscountID { get; set; }
            public Nullable<int> RVBooked_ReservationOpenedID { get; set; }

            public int RVBooked_MemberID { get; set; }
            [DisplayName("訂位數量")]
            public int RVBooked_Seats { get; set; }
            [DisplayName("備忘錄")]
            [DataType(DataType.MultilineText)]
            public string RVBooked_ReminderContext { get; set; }

            
        }
        public int? BackUpNo
        {
            get
            {
                return (this.RVBooked_ReminderContext != null && this.RVBooked_ReminderContext.Contains("BackUpSeats:")) ?
                    Convert.ToInt32(this.RVBooked_ReminderContext.Split(';')[0].Split(':')[1]) : (int?)null;
            }
            set
            {
                string str = (value is null) ? string.Empty : $"BackUpSeats:{value.Value};";

                char[] chars = new char[] {';'};
                if (this.RVBooked_ReminderContext.Split(chars, 2).Length <= 1)
                {

                    this.RVBooked_ReminderContext = str;

                }
                else
                {

                    this.RVBooked_ReminderContext = str + this.RVBooked_ReminderContext.Split(chars, 2)[1];
                }
            }
        }

        [DataType(DataType.MultilineText)]
        [DisplayName("食物敏感備註")]
        public string ReminderText {
            get
            {
                if(this.BackUpNo is null)
                {
                    return this.RVBooked_ReminderContext == "" ? null: this.RVBooked_ReminderContext;
                }
                else
                {
                    char[] chars = new char[] { ';' };
                    if(this.RVBooked_ReminderContext.Split(chars, 2).Length <= 1)
                    {
                        return null;
                    }
                    return this.RVBooked_ReminderContext.Split(chars, 2)[1];
                }
            }
            set
            {
                if (this.BackUpNo is null)
                {
                    this.RVBooked_ReminderContext= value;
                }
                else
                {
                    char[] chars = new char[] { ';' };
                    this.RVBooked_ReminderContext = this.RVBooked_ReminderContext.Split(chars, 2)[0] + ";" + value;
                }
            }
        }

        


    }


}