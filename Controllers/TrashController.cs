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
    public class TrashController : Controller
    {
        private PandaContext _context;
        private User ActiveUser 
        {
            // set active user
            get{ return _context.users.Where(u => u.userid == HttpContext.Session.GetInt32("id")).FirstOrDefault();}
        }
        public TrashController(PandaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("bright_ideas")]
        public IActionResult Home()
        {
            // test if user is currently logged in with session 
            if(ActiveUser == null)
            {
                TempData["ErrorMessage"] = "You've been redirected because you either tried to access a page without logging in (naughty!) or your session expired (whatchyou waitin' for?!) Please login to proceed!!";
                return RedirectToAction("Entry", "Panda");
            }
            // pull all posts and access session for user information
            List<Post> AllPosts = _context.posts.Include(post => post.Poster).Include(post =>post.Likers).OrderByDescending(post => post.Likers.Count).ToList();
            ViewBag.allposts = AllPosts;
            ViewBag.name = HttpContext.Session.GetString("alias");
            ViewBag.id = HttpContext.Session.GetInt32("id");
            return View();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(PostViewModel newidea)
        {
            // test if user is currently logged in with session             
            if(ActiveUser == null)
            {
                TempData["ErrorMessage"] = "You've been redirected because you either tried to access a page without logging in (naughty!) or your session expired (whatchyou waitin' for?!) Please login to proceed!!";
                return RedirectToAction("Entry", "Panda");
            }
            //check if post form is valid and then add new idea information to database
            if(ModelState.IsValid)
            {
                Post newPost = new Post
                {
                    postmessage = newidea.idea,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    userid = HttpContext.Session.GetInt32("id"),
                };
                _context.posts.Add(newPost);
                _context.SaveChanges();
            }
            return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("destroy/{id}")]
        public IActionResult Destroy(int id)
        {
            // test if user is currently logged in with session             
            if(ActiveUser == null)
            {
                TempData["ErrorMessage"] = "You've been redirected because you either tried to access a page without logging in (naughty!) or your session expired (whatchyou waitin' for?!) Please login to proceed!!";
                return RedirectToAction("Entry", "Panda");
            }
            //find post and delete it
            Post delete = _context.posts.Where(p => p.postid == id).SingleOrDefault(); 
            _context.posts.Remove(delete);
            //find all likes associated with the post and delete them from the database
            List<Like> likes = _context.likes.Where(like => like.postid == id).ToList();
            foreach(Like note in likes)
            {
                _context.likes.Remove(note);
            } 
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("like/{id}")]
        public IActionResult Like(int id)
        {
            // test if user is currently logged in with session             
            if(ActiveUser == null)
            {
                TempData["ErrorMessage"] = "You've been redirected because you either tried to access a page without logging in (naughty!) or your session expired (whatchyou waitin' for?!) Please login to proceed!!";
                return RedirectToAction("Entry", "Panda");
            }
            //convert session id to int
            int user = (int)HttpContext.Session.GetInt32("id");
            //if the user has not already liked this post, add a like to the database
            if(_context.likes.Where(post => post.postid == id).Where(u => u.userid == user).SingleOrDefault() == null)
            {
                Like newLike = new Like
                {
                    postid = id,
                    userid = HttpContext.Session.GetInt32("id"),
                };
                _context.likes.Add(newLike);
                _context.SaveChanges();
            }
            return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("bright_ideas/{id}")]
        public IActionResult Idea(int id)
        {
            // test if user is currently logged in with session             
            if(ActiveUser == null)
            {
                TempData["ErrorMessage"] = "You've been redirected because you either tried to access a page without logging in (naughty!) or your session expired (whatchyou waitin' for?!) Please login to proceed!!";
                return RedirectToAction("Entry", "Panda");
            }
            //get all idea information from database
            ViewBag.ideadeets = _context.posts.Include(post =>post.Poster).Include(post => post.Likers).ThenInclude(like => like.Liker).SingleOrDefault(post => post.postid == id);
            return View();
        }

        [HttpGet]
        [Route("users/{id}")]
        public IActionResult UserPage(int id)
        {
            // test if user is currently logged in with session 
            if(ActiveUser == null)
            {
                TempData["ErrorMessage"] = "You've been redirected because you either tried to access a page without logging in (naughty!) or your session expired (whatchyou waitin' for?!) Please login to proceed!!";
                return RedirectToAction("Entry", "Panda");
            }
            //get all user information from database
            ViewBag.userdeets = _context.users.Include(user =>user.Likes).SingleOrDefault(user => user.userid == id);
            ViewBag.posts = _context.posts.Where(post => post.userid == id).ToList().Count;
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
