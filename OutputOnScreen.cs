using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NotSteam
{
    internal class OutputOnScreen
    {
        internal static void ShowMessageHello()
        {
            Console.WriteLine("1. Вход");
            Console.WriteLine("2. Регистрация");
            Console.WriteLine("0. Выход");
        }
        internal static void ShowMessageLoginError()
        {
            Console.WriteLine("Ошибка входа. Неверный логин или пароль.");
            Console.ReadLine();
        }
        internal static void ShowMainMenuPoints()
        {
            Console.WriteLine("1. Просмотр каталога игр");
            Console.WriteLine("2. Просмотр библиотеки игр");
            Console.WriteLine("3. Просмотр корзины");
            Console.WriteLine("4. Пополнить баланс");
            Console.WriteLine("0. Выйти");
        }
        internal static void ShowMessageYourLibrary()
        {
            Console.WriteLine("Ваша библиотека игр:");
        }
        internal static void ShowManageCartMenuPoints()
        {
            Console.WriteLine("1. Удалить игру из корзины");
            Console.WriteLine("2. Оформить покупку");
            Console.WriteLine("0. Вернуться в главное меню");
        }
        internal static void ShowMessageGameNotFound()
        {
            Console.WriteLine("Игра с указанным ID не найдена в корзине.");
        }
    }
}
