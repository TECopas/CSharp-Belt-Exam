using Microsoft.EntityFrameworkCore;


namespace BeltExam.Models
{
public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        // this is the variable we will use to connect to the MySQL table, Lizards

        public DbSet<User> Users {get;set;}

        public DbSet <Shindig> Shindigs {get; set;}

        public DbSet <Association> Associations {get;set;}
    }
}