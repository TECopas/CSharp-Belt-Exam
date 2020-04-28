using System.ComponentModel.DataAnnotations;
using System;

using System.Collections.Generic; //Need this for lists.

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; //Need this for [NotMapped], use (alt + .) on unrecognized properties

namespace BeltExam.Models
{
    public class Association
    {
        [Key]
        public int AssociationID {get;set;}

        public int UserID {get;set;}

        public int ShindigID {get;set;}

        public User NavUser {get;set;}

        public Shindig NavShindig {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}