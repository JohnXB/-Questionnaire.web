using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 高级java大作业.Models
{
    public class QuestionnaireModel
    {
        public int QId { get; set; }
        public string QName { get; set; }
        public string CreateTime { get; set; }
        public int NumOfPeople { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        
    }
}