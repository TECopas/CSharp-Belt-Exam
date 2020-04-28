using System.ComponentModel.DataAnnotations;
using System;

using System.Collections.Generic; //Need this for lists.

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; //Need this for [NotMapped], use (alt + .) on unrecognized properties

namespace BeltExam.Models
{
   public class LoginUser
        {
            [Required]
            [EmailAddress]
            public string LoginEmail { get; set; }

            [DataType(DataType.Password)]
            [Required]
            public string LoginPassword {get;set;}
        }
}