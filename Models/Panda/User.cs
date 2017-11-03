using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace trashpanda.Models
{
    public abstract class BaseEntity {}
    public class User : BaseEntity
    {
        public int userid { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }        
        public List<Like> Likes {get;set;}
    }
}