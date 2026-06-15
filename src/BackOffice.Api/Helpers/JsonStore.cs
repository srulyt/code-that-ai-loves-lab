using System.Text.Json;

namespace BackOffice.Api.Helpers
{
    // Tiny JSON-file "database". Every repository news this up with its own file name,
    // so the data-access strategy is copy-pasted rather than shared behind a seam.
    public static class JsonStore
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        // Resolves a file under the app's Data folder (next to the binary at runtime).
        public static string DataPath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "Data", fileName);
        }

        public static List<T> Load<T>(string fileName)
        {
            var path = DataPath(fileName);
            if (!File.Exists(path))
            {
                return new List<T>();
            }
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }
            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }

        public static void Save<T>(string fileName, List<T> items)
        {
            var path = DataPath(fileName);
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(path, JsonSerializer.Serialize(items, Options));
        }
    }
}
