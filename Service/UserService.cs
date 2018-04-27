using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service
{
    public class UserService
    {
        private QuestionnaireEntities2 db;
        public UserService()
        {

            db = new QuestionnaireEntities2();
        }
        public User Login(string account, string password)
        {
            
            return db.User.SingleOrDefault(someUser => someUser.Account == account && someUser.Password == password);           
        }
        public string Register(User user)
        {
            try
            {
                if (db.User.SingleOrDefault(aUser => aUser.Account == user.Account) != null)
                    return "该账号已被注册，请重新注册";
                else
                {
                    db.User.Add(user);
                    db.SaveChanges();
                    return "注册成功";
                }
            }catch(Exception e)
            {
                return "注册失败";
            }
            
        }
        public void ChangeUserImg(string Imgpath,int userId)
        {
            db.User.SingleOrDefault(user => user.UserId ==userId).UserImg = Imgpath;
            db.SaveChanges();
        }
    }
}
