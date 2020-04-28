using System.ComponentModel.DataAnnotations;
using System;

using System.Collections.Generic; //Need this for lists.

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; //Need this for [NotMapped], use (alt + .) on unrecognized properties


namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserID {get; set;}


        [Required]
        [MinLength(2)]
        public string Name {get; set;}

        [EmailAddress]
        [Required]
        public string Email {get; set;}

        [DataType(DataType.Password)]
        [Required]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and comply with at 3 of 4 of the following requirements: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Shindig> MyPlannedEvents {get;set;}

        public List<Association> AttendingShindigs {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}