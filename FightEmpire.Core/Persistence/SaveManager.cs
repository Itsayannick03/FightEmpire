namespace FightEmpire.Core.Persistence;
using System.Text.Json;
using FightEmpire.Core.Models;

public static class SaveManager
{
    public static void SaveFighters(List<Fighter> fighters, string path)
    {
        string json = JsonSerializer.Serialize(
            fighters,
            new JsonSerializerOptions
            {
                WriteIndented = true
            }
        );

        File.WriteAllText(path, json);
    }

    public static List<Fighter>? LoadFighters(string path)
    {
        if(!File.Exists(path))
            return null;
        
        string json = File.ReadAllText(path);

        List<Fighter>? loadedFighters = JsonSerializer.Deserialize<List<Fighter>>(json);

        return loadedFighters;
    }
}