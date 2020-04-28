using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;

using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext {get;set;}
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost("RegisterUser")]
        public IActionResult Register(User reg)
        {
            if (ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == reg.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                reg.Password = Hasher.HashPassword(reg, reg.Password);

                dbContext.Users.Add(reg);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
                }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("LoggingIn")]
        public IActionResult LoggingInUser(LoginUser log)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(ru => ru.Email.Contains(log.LoginEmail));
                if(userInDb == null) //Checks if the email is present in the database at all
                {
                    ModelState.AddModelError("LoginEmail", "Email not registered.");
                    return View("Index");
                }

                var hash = new PasswordHasher<LoginUser>();
                var result = hash.VerifyHashedPassword(log, userInDb.Password, log.LoginPassword);
                if (result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Incorrect Password.");
                        return View("Index");
                    }

                HttpContext.Session.SetInt32("ID", userInDb.UserID);
                return Redirect("Home"); //Passing userInDB through to the AccountID View, hopefully allow the User access
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("Home")]
        public IActionResult Home()
        {

            ViewBag.Shindigs = dbContext.Shindigs
                .Include(w => w.Planner)
                .Include(w => w.Attendees)
                .ThenInclude(a => a.NavUser)
                .OrderBy(s => s.Date)
                .OrderBy(s => s.Time)
                .ToList();

            User UserinSession = dbContext.Users
                                .Include(u => u.AttendingShindigs)
                                .ThenInclude(a => a.NavShindig)
                                .FirstOrDefault(u => u.UserID == HttpContext.Session.GetInt32("ID"));
            return View(UserinSession);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/"); //seems to work appropriately, maintain use of "/" in all redirects
        }

        [HttpGet("NewShindig")]
        public IActionResult ShindigForm()
        {
            ViewBag.Planner = HttpContext.Session.GetInt32("ID");
            return View();
        }

        [HttpPost("CreateActivity")]
        public IActionResult CreateActivity(Shindig nu)
        {
            if (ModelState.IsValid)
                {
                    dbContext.Shindigs.Add(nu);
                    dbContext.SaveChanges();
                    return Redirect("/Home");
                }
            else
                {
                    ViewBag.Planner = HttpContext.Session.GetInt32("ID");
                    return View("ShindigForm");
                }
        }
        [HttpGet("Activity/{shinID}")]
        public IActionResult ShindigInfo (int shinID)
        {

            List<User> Current = dbContext.Shindigs //Selects all categories that the item belongs to
            .Include(u => u.Attendees) //Builds the item
            .ThenInclude(a => a.NavUser) //Builds the item in the association further
            .FirstOrDefault(w => w.ShindigID == shinID) //Finds the appropriate Product
            .Attendees.Select(a => a.NavUser).ToList(); //Uses the Association

            ViewBag.Attendees = Current;

            ViewBag.ShinInfo = dbContext.Shindigs.Include(s => s.Planner).FirstOrDefault(w => w.ShindigID == shinID);

            User UserinSession = dbContext.Users
                                .Include(u => u.AttendingShindigs)
                                .ThenInclude(a => a.NavShindig)
                                .FirstOrDefault(u => u.UserID == HttpContext.Session.GetInt32("ID"));
            
            return View(UserinSession);
        }
        [HttpGet("DeleteActivity/{shinID}")]
        public IActionResult DeleteActivity(int shinID)
        {
            var onewed = dbContext.Shindigs.FirstOrDefault(w => w.ShindigID == shinID);
            dbContext.Shindigs.Remove(onewed);
            dbContext.SaveChanges();
            return Redirect("/Home");
        }

        [HttpGet("Join/{shinID}")]
        public IActionResult RSVP(int shinID)
        {
            var onewed = dbContext.Shindigs.FirstOrDefault(w => w.ShindigID == shinID);
            var userinsesh = dbContext.Users
                                .Include(u => u.AttendingShindigs)
                                .ThenInclude(a => a.NavShindig)
                                .FirstOrDefault(u => u.UserID == HttpContext.Session.GetInt32("ID"));
            var RSVP = new Association();
            RSVP.ShindigID = onewed.ShindigID;
            RSVP.UserID = userinsesh.UserID;
            dbContext.Associations.Add(RSVP);
            dbContext.SaveChanges();
            return Redirect("/Home");
        }

        [HttpGet("unJoin/{shinID}")]
        public IActionResult unRSVP(int shinID)
        {
            var onewed = dbContext.Shindigs.FirstOrDefault(w => w.ShindigID == shinID);
            var userinsesh = dbContext.Users
                                .Include(u => u.AttendingShindigs)
                                .ThenInclude(a => a.NavShindig)
                                .FirstOrDefault(u => u.UserID == HttpContext.Session.GetInt32("ID"));
            var unRSVP = dbContext.Associations.Where(a => a.ShindigID == onewed.ShindigID).FirstOrDefault(a => a.UserID == userinsesh.UserID);
            dbContext.Associations.Remove(unRSVP);
            dbContext.SaveChanges();
            return Redirect("/Home");
        }

    }
}
