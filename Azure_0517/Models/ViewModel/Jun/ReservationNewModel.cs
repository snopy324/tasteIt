using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.ViewModel
{
    public class ReservationNewModel
    {
        public DateTime OpenDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Required]
        public int StepTime { get; set; }
    }
}