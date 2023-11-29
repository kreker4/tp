using System.Text.Json;

internal static class DataAccess
{

    static void LoadData()
    {
        if (File.Exists(gamesFilePath))
        {
            string gamesJson = File.ReadAllText(gamesFilePath);
            games = JsonSerializer.Deserialize<List<Game>>(gamesJson);
        }

        if (File.Exists(usersFilePath))
        {
            string usersJson = File.ReadAllText(usersFilePath);
            users = JsonSerializer.Deserialize<List<User>>(usersJson);
        }
    }

    static void SaveData()
    {
        string gamesJson = JsonSerializer.Serialize(games);
        File.WriteAllText(gamesFilePath, gamesJson);

        string usersJson = JsonSerializer.Serialize(users);
        File.WriteAllText(usersFilePath, usersJson);
    }
}