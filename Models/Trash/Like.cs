using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace trashpanda.Models
{
    public class Like : BaseEntity
    {
        public int likeid { get; set; }
        public int postid { get; set; }
        public int? userid { get; set; }
        public User Liker {get;set;}
        
    }
}