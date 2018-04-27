using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 高级java大作业.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string Type { get; set; }
        public List<OptionModel> Options { get; set; }= new List<OptionModel>();

    }
}