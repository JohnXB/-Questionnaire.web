using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class QuestionnaireService
    {
        private QuestionnaireEntities2 db;
        public QuestionnaireService()
        {

            db = new QuestionnaireEntities2();
        }
        public bool SaveQuestionnaire(Questionnaire questionnaire)
        {
            try
            {
                db.Questionnaire.Add(questionnaire);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        public List<Questionnaire> GetQuestionnaire(string key,string serchMode)
        {
            if(key == "所有")
            {
                return db.Questionnaire.ToList();
            }
            else
            {
                var questionnaireList = new List<Questionnaire>();
                var questionnaires = db.Questionnaire.ToList();
                if (serchMode == "QName")
                {
                    foreach (var questionnaire in questionnaires)
                    {
                        if (questionnaire.QName.Contains(key))
                        {
                            questionnaireList.Add(questionnaire);
                        }
                    }
                }
                else
                {
                    foreach (var questionnaire in questionnaires)
                    {
                        if (questionnaire.User.UserName.Contains(key))
                        {
                            questionnaireList.Add(questionnaire);
                        }
                    }
                }
                return questionnaireList;
            }
        }
        public Questionnaire GetQuestionnaireById(int QId)
        {
            return db.Questionnaire.SingleOrDefault(Q => Q.QId == QId);
        }
        public string SaveAnswer(List<Answer> answers)
        {
            try
            {
                var QId = answers[0].QId;
                var IP = answers[0].IpAddress;
                if(db.Answer.Where(Q => Q.QId == QId &&Q.IpAddress==IP) != null)
                {
                    return "填写失败,不能重复填写";
                }
                foreach (var answer in answers)
                {
                    db.Answer.Add(answer);
                }
                //db.Answer.AddRange(answers);
                db.Questionnaire.SingleOrDefault(Q => Q.QId == QId).NumOfPeople++;
                db.SaveChanges();
                return "填写成功";
            }
            catch (Exception e)
            {
                return "填写失败,请重新填写";
            }

            
        }
        public List<Questionnaire> GetQuestionnaireByUser(string userAccount)
        {
            return db.User.SingleOrDefault(user => user.Account == userAccount).Questionnaire.ToList();
                
        }
        public List<Answer> GetAnswerByQId(int QId)
        {
            return db.Answer.Where(answer => answer.QId == QId).ToList();
        }

        public string GetQNameByQId(int QId)
        {
            return db.Questionnaire.SingleOrDefault(Q => Q.QId == QId).QName;
        }
        public string GetQuestionNameByQuestionId(int QuestionId)
        {
            return db.Question.SingleOrDefault(Question => Question.QuestionId == QuestionId).QuestionName;
        }
        public string GetOptionNameByOptionId(int OptionId)
        {
            return db.Option.SingleOrDefault(option => option.OptionId == OptionId).OptionName;
        }
    }
}
