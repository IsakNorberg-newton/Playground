using Models.Music;
using Newtonsoft.Json;

namespace PlayGround.Extensions;

// Extension method for IEnumerable<T> - just like LINQ methods
public static class MusicGroupExtensions
{
    
    public static List<MusicGroup> SerializeJson(this List<MusicGroup> list, string jsonFileName)
    {
        string sJson = JsonConvert.SerializeObject(list, Formatting.Indented);

        using (Stream s = File.Create(fname(jsonFileName)))
        using (TextWriter writer = new StreamWriter(s))
            writer.Write(sJson);
        
        return list;
    }

    public static List<MusicGroup> DeSerializeJson(this List<MusicGroup> list, string jsonFileName)
    {
        using (Stream s = File.OpenRead(fname(jsonFileName)))
        using (TextReader reader = new StreamReader(s))
        {
            var flist = JsonConvert.DeserializeObject<List<MusicGroup>>(reader.ReadToEnd());
            return flist;
        }
    }

    static string fname(string name)
    {
        var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        documentPath = Path.Combine(documentPath, "AdvancedProgramming", "Linq");
        if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
        return Path.Combine(documentPath, name);
    }
}

