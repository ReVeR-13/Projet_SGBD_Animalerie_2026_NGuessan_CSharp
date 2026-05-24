using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public class Authentification
    {
        public static bool Login(string email, string password)
        {
            bool ret = false;
            User? user = UserService.DB_FindByEmail(email);
            if (user != null && user.Password.ToLower() == password.Trim().ToLower())
            {
                if (!user.Active)
                {
                    user.UpdateActive(true);
                    UserService.DB_UpdateActive(user);
                }
                ret = true;
            }
            return ret;
        }

        public static bool LogOut(string email)
        {
            bool ret = false;
            User? user = UserService.FindByMail(email);
            if (user != null)
            {
                if (user.Active)
                {
                    user.UpdateActive(false);
                    UserService.DB_UpdateActive(user);
                }
                ret = true;
            }
            return ret;
        }


    }
}
