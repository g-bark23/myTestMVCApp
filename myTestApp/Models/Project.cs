using System;
using System.ComponentModel.DataAnnotations;

namespace myTestApp.Models
{
    public class Project
    {
        public String name { get; set; }
        [Key]
        public int projectID { get; set; }
        public int activeStatus { get; set; }
    }
}
