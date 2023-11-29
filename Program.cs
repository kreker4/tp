using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;



 class Game
{
    public string Description { get; set; }
    public int ID { get; set; }
    public decimal Price { get; set; }
    public string Title { get; set; }
}
class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public decimal Balance { get; set; }
    public List<int> Library { get; set; } = new List<int>();
    public List<int> Cart { get; set; } = new List<int>();
    public string Nickname { get; set; }
}

class Program
{
    private static List<Game> games = new List<Game>();
    private static List<User> users = new List<User>();
    private static string gamesFilePath = "games.json";
    private static string usersFilePath = "users.json";
    
    static void FirstMenuGreetings()
    {
        Console.WriteLine("1. Вход");
        Console.WriteLine("2. Регистрация");
        Console.WriteLine("0. Выход");
        int choice = CheckIfMenuCorrect();
    }

    static void WrongInput()
    {
        Console.WriteLine("Ошибка входа. Неверный логин или пароль.");
        Console.ReadLine();
    }

    static void Main(string[] args)
    {
        DataAccess.LoadData();

        while (true)
        {
            FirstMenuGreetings();
            //Console.WriteLine("1. Вход");
            //Console.WriteLine("2. Регистрация");
            //Console.WriteLine("0. Выход");
            //int choice = CheckIfMenuCorrect();
            if (choice == null)
            {
               // WrongInput();
                Console.WriteLine("Ошибка входа. Неверный логин или пароль.");
                Console.ReadLine();
                continue;
            }

            switch (choice)
            {
                case 1:
                    User user = Login();
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

    static User Login()
    {
        Console.Write("Введите имя пользователя: ");
        string username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();

        User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

        return user;
    }

    static void Register()
    {
        Console.Write("Введите имя пользователя: ");
        string username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();
        Console.Write("Введите ник: ");
        string nickname = Console.ReadLine();

        if (users.Any(u => u.Username == username))
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
            users.Add(newUser);
            Console.WriteLine("Пользователь успешно зарегистрирован.");
        }
    }
    static void wait()
    {
        Console.WriteLine();
        Console.WriteLine("Для продолжения нажмите любую клавишу:");
        Console.ReadLine();
        Console.Clear();

    }
    static int CheckIfMenuCorrect()//переделать по колву пунктов
    {
        int choice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out choice) & (choice < games.Count + 999))
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
    static void MainMenu(User user)
    {
        while (true)
        {
            wait();
            DataAccess.SaveData();
            Console.WriteLine("1. Просмотр каталога игр");
            Console.WriteLine("2. Просмотр библиотеки игр");
            Console.WriteLine("3. Просмотр корзины");
            Console.WriteLine("4. Пополнить баланс");
            Console.WriteLine("0. Выйти");

            int choice = CheckIfMenuCorrect();

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
        Console.WriteLine("Каталог игр:");
        foreach (var game in games)
        {
            Console.WriteLine($"ID: {game.ID}, Название: {game.Title}, Цена: {game.Price}");
            Console.WriteLine($"Описание: {game.Description}");
            Console.WriteLine();
        }
        Console.WriteLine("1. Добавить игру в корзину");
        Console.WriteLine("2. Вернуться в главное меню");

        int choice = CheckIfMenuCorrect();

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
        Console.WriteLine("Ваша библиотека игр:");
        if (user.Library.Count == 0)
        {
            Console.WriteLine("Ваша библиотека пуста.");
        }
        else
        {
            foreach (int gameId in user.Library)
            {
                Game game = games.FirstOrDefault(g => g.ID == gameId);
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

        Console.WriteLine("1. Удалить игру из корзины");
        Console.WriteLine("2. Оформить покупку");
        Console.WriteLine("0. Вернуться в главное меню");

        int choice = CheckIfMenuCorrect();

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
                Game game = games.FirstOrDefault(g => g.ID == gameId);
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
        int gameId = CheckIfMenuCorrect();

        Game gameToAdd = games.FirstOrDefault(g => g.ID == gameId);
        if (gameToAdd != null)
        {
            user.Cart.Add(gameToAdd.ID);
            Console.WriteLine($"{gameToAdd.Title} добавлена в корзину.");
        }
        else
        {
            Console.WriteLine("Игра с указанным ID не найдена.");
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
        int gameId = CheckIfMenuCorrect();

        if (user.Cart.Contains(gameId))
        {
            Game gameToRemove = games.FirstOrDefault(g => g.ID == gameId);
            user.Cart.Remove(gameId);
            Console.WriteLine($"{gameToRemove.Title} удалена из корзины.");
        }
        else
        {
            Console.WriteLine("Игра с указанным ID не найдена в корзине.");
        }
    }

    static void Purchase(User user)
    {
        if (user.Cart.Count == 0)
        {
            Console.WriteLine("Ваша корзина пуста. Нечего покупать.");
            return;
        }

        decimal totalCost = user.Cart.Select(gameId => games.First(g => g.ID == gameId).Price).Sum();
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
