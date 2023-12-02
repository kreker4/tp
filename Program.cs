using NotSteam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Xml.Linq;



class Program
{
   
    static void Main(string[] args)
    {
        DataAccess.LoadData();
               
        while (true)
        {
            int choice = UserConsole.FirstMenuChoose();
            
            if (choice == null)
            {
                OutputOnScreen.ShowMessageLoginError();
                continue;
            }

            switch (choice)
            {
                case 1:
                    User user = SigningManager.Login();
                    if (user != null)
                    {
                        Console.WriteLine($"Добро пожаловать, {user.Nickname}!");
                        MainMenu(user);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка входа. Попробуйте снова.");
                    }
                    break;

                case 2:
                    Register();
                    DataAccess.SaveData();
                    break;

                case 0:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    static void Register()
    {
        Console.Write("Введите имя пользователя: ");
        string username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();
        Console.Write("Введите ник: ");
        string nickname = Console.ReadLine();

        if (DataAccess.users.Any(u => u.Username == username))
        {
            Console.WriteLine("Пользователь с таким именем уже существует.");
        }
        else
        {
            User newUser = new User
            {
                Username = username,
                Password = password,
                Nickname = nickname
            };
            DataAccess.users.Add(newUser);
            Console.WriteLine("Пользователь успешно зарегистрирован.");
        }
    }

    static void MainMenu(User user)
    {
        while (true)
        {
            UserConsole.TimeLag();
            DataAccess.SaveData();
            OutputOnScreen.ShowMainMenuPoints();
            int choice = UserConsole.CheckIfMenuCorrect();

            switch (choice)
            {
                case 1:
                    ViewCatalog(user);
                    break;

                case 2:
                    ViewLibrary(user);
                    break;

                case 3:
                    ManageCart(user);
                    break;

                case 4:
                    ReplenishBalance(user);
                    break;

                case 0:
                    Environment.Exit(0);
                    break;
            }
        }

        
    }

    static void ViewCatalog(User user)
    {
        UserConsole.ShowCatalog();
        int choice = UserConsole.CheckIfMenuCorrect();

        switch (choice)
        {
            case 1:
                AddToCart(user);
                ViewCatalog(user);
                break;

            case 2:
                return;
        }
    }

    static void ViewLibrary(User user)
    {
        OutputOnScreen.ShowMessageYourLibrary();
        if (user.Library.Count == 0)
        {
            Console.WriteLine("Ваша библиотека пуста.");
        }
        else
        {
            foreach (int gameId in user.Library)
            {
                Game game = DataAccess.games.FirstOrDefault(g => g.ID == gameId);
                if (game != null)
                {
                    Console.WriteLine($"ID: {game.ID}, Название: {game.Title}");
                }
            }
        }
    }

    static void ManageCart(User user)
    {
        Console.WriteLine("Управление корзиной:");
        ViewCart(user);

        OutputOnScreen.ShowManageCartMenuPoints();

        int choice = UserConsole.CheckIfMenuCorrect();

        switch (choice)
        {
            case 1:
                RemoveFromCart(user);
                break;
            case 2:
                Purchase(user);
                break;
            case 0:
                return;
        }
    }

    static void ViewCart(User user)
    {
        Console.WriteLine("Корзина:");
        if (user.Cart.Count == 0)
        {
            Console.WriteLine("Ваша корзина пуста.");
        }
        else
        {
            foreach (int gameId in user.Cart)
            {
                Game game = DataAccess.games.FirstOrDefault(g => g.ID == gameId);
                if (game != null)
                {
                    Console.WriteLine($"ID: {game.ID}, Название: {game.Title}, Цена: {game.Price}");
                }
            }
        }
        Console.WriteLine();
    }

    static void AddToCart(User user)
    {
        Console.Write("Введите ID игры для добавления в корзину: ");
        int gameId = UserConsole.CheckIfMenuCorrect();

        Game gameToAdd = DataAccess.games.FirstOrDefault(g => g.ID == gameId);
        if (gameToAdd != null)
        {
            user.Cart.Add(gameToAdd.ID);
            Console.WriteLine($"{gameToAdd.Title} добавлена в корзину.");
        }
        else
        {
            OutputOnScreen.ShowMessageGameNotFound();
        }
    }

    static void RemoveFromCart(User user)
    {
        ViewCart(user);

        if (user.Cart.Count == 0)
        {
            Console.WriteLine("Ваша корзина пуста.");
            return;
        }

        Console.Write("Введите ID игры для удаления из корзины: ");
        int gameId = UserConsole.CheckIfMenuCorrect();

        if (user.Cart.Contains(gameId))
        {
            Game gameToRemove = DataAccess.games.FirstOrDefault(g => g.ID == gameId);
            user.Cart.Remove(gameId);
            Console.WriteLine($"{gameToRemove.Title} удалена из корзины.");
        }
        else
        {
            OutputOnScreen.ShowMessageGameNotFound();
        }
    }

    static void Purchase(User user)
    {
        if (user.Cart.Count == 0)
        {
            Console.WriteLine("Ваша корзина пуста. Нечего покупать.");
            return;
        }

        decimal totalCost = user.Cart.Select(gameId => DataAccess.games.First(g => g.ID == gameId).Price).Sum();
        if (user.Balance >= totalCost)
        {
            user.Balance -= totalCost;
            user.Library.AddRange(user.Cart);
            Console.WriteLine("Покупка успешно оформлена.");
            user.Cart.Clear();
        }
        else
        {
            Console.WriteLine("Недостаточно средств на балансе.");
        }
    }

    static void ReplenishBalance(User user)
    {
        Console.WriteLine($"Ваш текущий баланс: {user.Balance}");
        Console.Write("Введите сумму для пополнения баланса (или 0, чтобы вернуться): ");

        decimal amount = decimal.Parse(Console.ReadLine());

        if (amount > 0)
        {
            user.Balance += amount;
            Console.WriteLine($"Баланс успешно пополнен. Новый баланс: {user.Balance}");
        }
    }
}
