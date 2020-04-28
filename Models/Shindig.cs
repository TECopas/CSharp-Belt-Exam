using System.ComponentModel.DataAnnotations;
using System;

using System.Collections.Generic; //Need this for lists.

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; //Need this for [NotMapped], use (alt + .) on unrecognized properties

namespace BeltExam.Models
{
    public class Shindig
    {
        [Key]
        public int ShindigID {get; set;}

        [Required]
        public string Title {get; set;}



        [Required]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time {get; set;}

        [Required]
        public int Duration {get; set;}

        [Required]
        public string Description {get; set;}

        public int UserID {get; set;}

        public User Planner {get; set;}

        public List<Association> Attendees {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;


    }
}