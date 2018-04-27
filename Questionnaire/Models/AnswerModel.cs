using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 高级java大作业.Models
{
    public class AnswerModel
    {
        public int AnswerId { get; set; }
        public int QId { get; set; }
        public int QuestionId { get; set; }
        public List<int> OptionId { get; set; } = new List<int>();
        public string IpAddress { get; set; }
        public string CreateTime { get; set; }
    }
}