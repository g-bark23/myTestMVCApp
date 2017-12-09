using System;
using System.ComponentModel.DataAnnotations;

namespace myTestApp.Models
{
    public class UserToGroup
    {
        [Key]
        public String userToGroupID { get; set; }
        public String userID { get; set; }
        public String groupID { get; set; }
    }
}
