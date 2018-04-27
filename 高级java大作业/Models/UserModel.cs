using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 高级java大作业.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CreateTime { get; set; }
        public string Account { get; set; }
        public string Tel { get; set; }
        public bool Gender { get; set; }
        public string UserImg { get; set; }
    }
}