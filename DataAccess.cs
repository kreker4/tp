using System.Text.Json;
using NotSteam;

internal static class DataAccess
{

    public static List<Game> games { get; private set; } = new List<Game>();
    public static List<User> users { get; private set; } = new List<User>();

    private static string _gamesFilePath = "games.json";
    private static string _usersFilePath = "users.json";

    public static void ChangeUsers(User user)
    {
        users.Add(user);
    }

    
    public static void LoadData()
    {
        if (File.Exists(_gamesFilePath))
        {
            string gamesJson = File.ReadAllText(_gamesFilePath);
            games = JsonSerializer.Deserialize<List<Game>>(gamesJson);
        }

        if (File.Exists(_usersFilePath))
        {
            string usersJson = File.ReadAllText(_usersFilePath);
            users = JsonSerializer.Deserialize<List<User>>(usersJson);
        }
    }

   
    public static void SaveData()
    {
        string gamesJson = JsonSerializer.Serialize(games);
        File.WriteAllText(_gamesFilePath, gamesJson);

        string usersJson = JsonSerializer.Serialize(users);
        File.WriteAllText(_usersFilePath, usersJson);
    }
}