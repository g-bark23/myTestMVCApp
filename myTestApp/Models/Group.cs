using System;
using System.ComponentModel.DataAnnotations;

namespace myTestApp.Models
{
    public class Group
    {
        [Key]
        public int groupID { get; set; }
        public String name { get; set; }
        public bool isActive { get; set; }
    }
}
