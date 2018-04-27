using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 高级java大作业.Models
{
    public class AnswerShowModel
    {
        public int AnswerId { get; set; }
        public string QName { get; set; }
        public string QuestionName { get; set; }
        public List<string> OptionName { get; set; } = new List<string>();

        public int OptionNameCount { get; set; }
        public string IpAddress { get; set; }
        public string CreateTime { get; set; }
    }
}