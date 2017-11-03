using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace trashpanda.Models
{
    public class Post : BaseEntity
    {
        public int postid { get; set; }
        public string postmessage { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }        
        public int? userid { get; set; }
        public User Poster {get;set;}
        public List<Like> Likers {get;set;}

    }
}