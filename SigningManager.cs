using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSteam
{
    internal class SigningManager
    {
        internal static User Login()
        {
            Console.Write("Введите имя пользователя: ");
            string username = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            User user = DataAccess.users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }
    }
}
