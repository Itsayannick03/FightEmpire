namespace FightEmpire.Core.Persistence;
using System.Text.Json;
using FightEmpire.Core.Models;
using System.Text.Json.Serialization;

public static class SaveManager
{
    public static void SaveFighters(List<Fighter> fighters, string path)
    {
        string json = JsonSerializer.Serialize(
            fighters,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
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