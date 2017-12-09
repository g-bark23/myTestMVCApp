using System;
using System.ComponentModel.DataAnnotations;

namespace myTestApp.Models
{
    public class TimeCard
    {
        [Key]
        public int timeCardID { get; set; }
        public String startTime { get; set; }
        public string stopTime { get; set; }
        public int userID { get; set; }
        public float totalTime { get; set; }
        public String revisionHistory { get; set; }
        public String lastModDate { get; set; }
        public String comments { get; set; }
    }
}
