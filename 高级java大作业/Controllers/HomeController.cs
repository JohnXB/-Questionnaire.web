using System.Web.Mvc;
using Service;
using 高级java大作业.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
namespace 高级java大作业.Controllers
{
    public class HomeController : Controller
    {
        private UserService _userService = new UserService();
        private QuestionnaireService _questionnaireService = new QuestionnaireService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string account, string password)
        {
            User aUser = _userService.Login(account, password);
            if (aUser != null)
            {
                UserModel user = new UserModel()
                {
                    Account = aUser.Account,
                    CreateTime = aUser.CreateTime.ToString(),
                    UserName = aUser.UserName,
                    UserId = aUser.UserId,
                    UserImg = aUser.UserImg,
                    Gender = aUser.Gender,
                    Tel = aUser.Tel
                };
                Session.Add("user", user);
                string json = JsonConvert.SerializeObject(user);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public ActionResult Logout()
        {
            try
            {
                Session.Remove("user");
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RegisterView()
        {
            return View();
        }
        public ActionResult Register(UserModel user)
        {
            User newUser = new User
            {
                Account = user.Account,
                CreateTime = DateTime.Now,
                Gender = user.Gender,
                Password = user.Password,
                Tel = user.Tel,
                UserName = user.UserName,
                UserImg = "/Img/image_3.jpg"
            };
            var result = new Result();
            result.result = _userService.Register(newUser);
            if (result.result != "注册成功")
                result.url = "/Home/RegisterView";
            else
                result.url = "/Home/Index";
            return View("~/Views/Home/Result.cshtml", result);

        }
        public ActionResult Create()
        {
            if (Session["user"] == null)
                return RedirectToAction("Index");
            return View();
        }
        public ActionResult CreateSave(QuestionnaireModel questionnaire)
        {
            List<QuestionModel> question = questionnaire.Questions;
            var newQuestionnaire = new Questionnaire
            {
                QName = questionnaire.QName,
                QId = questionnaire.QId,
                CreateTime = DateTime.Now,
                UserId = ((UserModel)Session["user"]).UserId,
                Question = questionnaire.Questions.Select(a => new Question
                {
                    QuestionId = a.QuestionId,
                    QuestionName = a.QuestionName,
                    Type = a.Type,
                    Option = a.Options.Select(b => new Option
                    {
                        OptionName = b.OptionName,
                        OptionId = b.OptionId
                    }).ToList()
                }).ToList()
            };
            var success = _questionnaireService.SaveQuestionnaire(newQuestionnaire);
            Result result = new Result();
            if (success)
                result.result = "创建问卷成功";
            else
                result.result = "创建问卷失败";
            result.url = "/Home/Create";
            return View("~/Views/Home/Result.cshtml", result);
        }
        public ActionResult QuestionnaireList()
        {
            return View();
        }
        public ActionResult Serch(string serchKey, string serchMode)
        {
            var questionnaires = QuestionnairesToQuestionnaireModel(_questionnaireService.GetQuestionnaire(serchKey, serchMode));
            if (questionnaires.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var json = JsonConvert.SerializeObject(questionnaires);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillIn(string QId)
        {
            var questionnaire = _questionnaireService.GetQuestionnaireById(Convert.ToInt32(QId));
            if (questionnaire == null)
            {
                Result result = new Result();
                result.result = "请正确使用问卷网";
                result.url = "/Home/QuestionnaireList";
                return View("~/Views/Home/Result.cshtml", result);
            }

            return View(QuestionnairesToQuestionnaireModel(questionnaire));
        }
        public ActionResult FillInSave(AnswerModel[] answers)
        {

            var userHostAddress = GetHostAddress();
            
            var answerModelList = answers.ToList();
            List<Answer> answerList = new List<Answer>();
            foreach (var answer in answerModelList)
            {
                if (answer.OptionId.Count == 1)
                {
                    Answer oneAnswer = new Answer();
                    oneAnswer.AnswerId = answer.AnswerId;
                    oneAnswer.IpAddress = userHostAddress;
                    oneAnswer.CreateTime = DateTime.Now;
                    oneAnswer.QId = answer.QId;
                    oneAnswer.QuestionId = answer.QuestionId;
                    oneAnswer.OptionId = answer.OptionId[0];
                    answerList.Add(oneAnswer);
                }
                else
                {
                    for (int i = 0; i < answer.OptionId.Count; i++)
                    {
                        Answer oneAnswer = new Answer();
                        oneAnswer.AnswerId = answer.AnswerId;
                        oneAnswer.IpAddress = userHostAddress;
                        oneAnswer.CreateTime = DateTime.Now;
                        oneAnswer.QId = answer.QId;
                        oneAnswer.QuestionId = answer.QuestionId;
                        oneAnswer.OptionId = answer.OptionId[i];
                        answerList.Add(oneAnswer);
                    }
                }
            }
            var resultMsg = _questionnaireService.SaveAnswer(answerList);
            Result result = new Result();
            result.result = resultMsg;
            if (resultMsg == "填写成功")
            {
                result.url = "/Home/QuestionnaireList";
            }
            else
            {
                result.url = "/Home/FillIn?QId=" + answerModelList[0].QId;
            }
            return View("~/Views/Home/Result.cshtml", result);
        }

        public ActionResult MyQuestionnaire()
        {
            if (Session["user"] == null)
                return RedirectToAction("Index");
            var questionnaires = QuestionnairesToQuestionnaireModel(_questionnaireService.GetQuestionnaireByUser(((UserModel)Session["user"]).Account));
            return View(questionnaires);
        }
        public ActionResult QuestionnaireAnalysis(string QId)
        {
            if (Session["user"] == null)
                return RedirectToAction("Index");
            var answers = _questionnaireService.GetAnswerByQId(Convert.ToInt32(QId));
            var questionnaireModel = QuestionnairesToQuestionnaireModel(_questionnaireService.GetQuestionnaireById(Convert.ToInt32(QId)));
            List<AnswerShowModel> answerShowList = new List<AnswerShowModel>();
            for (int i = 0;i < answers.Count;i++)
            {
                var answerShowModel = new AnswerShowModel();
                answerShowModel.IpAddress = answers[i].IpAddress;
                answerShowModel.CreateTime = answers[i].CreateTime.ToString();
                answerShowModel.QName = _questionnaireService.GetQNameByQId(answers[i].QId);
                answerShowModel.QuestionName = _questionnaireService.GetQuestionNameByQuestionId(answers[i].QuestionId);
                answerShowModel.OptionName.Add(_questionnaireService.GetOptionNameByOptionId(answers[i].OptionId));
                for(int j = i + 1;j < answers.Count; j++)
                {
                    if(answers[i].QuestionId == answers[j].QuestionId&& answers[i].IpAddress == answers[j].IpAddress)
                    {
                        answerShowModel.OptionName.Add(_questionnaireService.GetOptionNameByOptionId(answers[i].OptionId));
                    }
                }
                answerShowList.Add(answerShowModel);
            }
            var questionnaireAnalysis = new QuestionnaireAnalysis();
            questionnaireAnalysis.answerShowList = answerShowList;
            questionnaireAnalysis.questionnaireModel = questionnaireModel;
            return View(questionnaireAnalysis);
        }
        public ActionResult ChangeUserImg()
        {
            HttpPostedFileBase hpf = Request.Files["file"];
            string fileName = hpf.FileName;
            string path = "/Img/" + fileName;
            string mapPath = Server.MapPath(path);
            hpf.SaveAs(mapPath);
            _userService.ChangeUserImg(path,((UserModel)Session["user"]).UserId);
            ((UserModel)Session["user"]).UserImg = path;
            var json = JsonConvert.SerializeObject(path);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        
        /* 服务方法*/
        public string GetHostAddress()
        {
            string userHostAddress = Request.ServerVariables.Get("Remote_Addr").ToString(); ;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }
        
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        public List<QuestionnaireModel> QuestionnairesToQuestionnaireModel(List<Questionnaire> questionnaireList)
        {
            var questionnaires = questionnaireList.Select(questionnaire => new QuestionnaireModel
            {
                QId = questionnaire.QId,
                QName = questionnaire.QName,
                CreateTime = questionnaire.CreateTime.ToString(),
                NumOfPeople = questionnaire.NumOfPeople,
                UserName = questionnaire.User.UserName,
                Questions = questionnaire.Question.Select(question => new QuestionModel
                {
                    QuestionId = question.QuestionId,
                    QuestionName = question.QuestionName,
                    Type = question.Type,
                    Options = question.Option.Select(option => new OptionModel
                    {
                        OptionId = option.OptionId,
                        OptionName = option.OptionName
                    }).ToList()
                }).ToList()
            }).ToList();
            return questionnaires;
        }
        public QuestionnaireModel QuestionnairesToQuestionnaireModel(Questionnaire questionnaire)
        {
            var questionnaires = new QuestionnaireModel
            {
                QId = questionnaire.QId,
                QName = questionnaire.QName,
                CreateTime = questionnaire.CreateTime.ToString(),
                NumOfPeople = questionnaire.NumOfPeople,
                UserName = questionnaire.User.UserName,
                Questions = questionnaire.Question.Select(question => new QuestionModel
                {
                    QuestionId = question.QuestionId,
                    QuestionName = question.QuestionName,
                    Type = question.Type,
                    Options = question.Option.Select(option => new OptionModel
                    {
                        OptionId = option.OptionId,
                        OptionName = option.OptionName
                    }).ToList()
                }).ToList()
            };
            return questionnaires;
        }
    }
}