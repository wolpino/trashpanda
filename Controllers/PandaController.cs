using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trashpanda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace trashpanda.Controllers
{
    public class PandaController : Controller
    {
        private PandaContext _context;
        private User ActiveUser 
        {
            // set active user
            get{ return _context.users.Where(u => u.userid == HttpContext.Session.GetInt32("id")).FirstOrDefault();}
        }
        public PandaController(PandaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Entry()
        {

            return View();
        }
        
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegViewModel model)
        {
            PasswordHasher<RegViewModel> hasher = new PasswordHasher<RegViewModel>();
            //check if user email is already in database and add error if so
            if(_context.users.Where(user => user.username == model.Email).SingleOrDefault() != null)
                ModelState.AddModelError("Email", "Username already in use, please log in or choose another username");
            //if form is valid add new user to database and hash password
            if(ModelState.IsValid)
            {
                User NewUser = new User
                {
                    name = model.Name,
                    alias = model.Alias,
                    username = model.Email,
                    password = hasher.HashPassword(model, model.Password),
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,

                };
                //save new user object to add to session
                User fresh = _context.users.Add(NewUser).Entity;
                _context.SaveChanges();
                HttpContext.Session.SetInt32("id", fresh.userid);
                HttpContext.Session.SetString("alias", fresh.alias);
                return RedirectToAction("Home", "Trash");
            }
            return View("Entry");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel model)
        {
            PasswordHasher<LoginViewModel> hasher = new PasswordHasher<LoginViewModel>();
            //check if user email is in database, if not add an error
            User LoggingIn = _context.users.Where(user => user.username == model.Username).SingleOrDefault();
            if(LoggingIn == null)
                ModelState.AddModelError("Username", "Invalid email login. Have you registered?");
            //if so check hashed password
            else if(hasher.VerifyHashedPassword(model, LoggingIn.password, model.PWD) == 0)
            {
                ModelState.AddModelError("PWD", "Close but no cigar!");
            }
            //if email and password are correct log user in
            if(ModelState.IsValid)
            {
                HttpContext.Session.SetInt32("id", LoggingIn.userid);
                HttpContext.Session.SetString("alias", LoggingIn.alias);
                return RedirectToAction("Home", "Trash");
            }
            return View("Entry");
        }

        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            //reference page for database/register check
            if(ActiveUser == null)
                return RedirectToAction("Entry");
            List<User> AllUsers = _context.users.ToList();
            ViewBag.allusers = AllUsers;
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            //clear session to log user out
            HttpContext.Session.Clear();
            TempData["ErrorMessage"] = "You've logged out!!";
            return RedirectToAction("Entry");
            
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
