using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotSteam;

namespace NotSteam
{
    internal class UserConsole
    {
    internal static int FirstMenuChoose()
        {
            OutputOnScreen.ShowMessageHello();
            int choice = CheckIfMenuCorrect();
            return choice;

        }

    internal static int CheckIfMenuCorrect()//переделать по колву пунктов
        {
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) & (choice < DataAccess.games.Count + 999))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод");
                }
            }
            return choice;
        }
    internal static void TimeLag()
    {
        Console.WriteLine();
        Console.WriteLine("Для продолжения нажмите любую клавишу:");
        Console.ReadLine();
        Console.Clear();

    }

        internal static void ShowCatalog()
        {
            Console.WriteLine("Каталог игр:");
            foreach (var game in DataAccess.games)
            {
                Console.WriteLine($"ID: {game.ID}, Название: {game.Title}, Цена: {game.Price}");
                Console.WriteLine($"Описание: {game.Description}");
                Console.WriteLine();
            }
            Console.WriteLine("1. Добавить игру в корзину");
            Console.WriteLine("2. Вернуться в главное меню");
        }
    }
}
