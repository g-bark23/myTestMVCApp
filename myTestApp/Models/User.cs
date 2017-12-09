using System;
using System.ComponentModel.DataAnnotations;

namespace myTestApp.Models
{
    public class User
    {
        public String name { get; set; }
        public String username { get; set; }
        [Key]
        public int userID { get; set; }
        public int isAdmin { get; set; }
        public String password { get; set; }
    }
}
