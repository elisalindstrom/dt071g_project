// Hanterar hur provsvar sparas och laddas

using System.Text.Json;

namespace BloodTestAnalyzer
{
    public class JsonBloodTestStorage : IBloodTestStorage
    {
        private readonly string jsonFile;

        // Konstruktor som anger vilken fil som ska användas vid lagring
        public JsonBloodTestStorage(string jsonFile)
        {
            this.jsonFile = jsonFile;
        }

        // Laddar sparade resultat från JSON-filen
        public List<TestResult> Load()
        {
            // Kontrollerar om filen finns, annars returneras tom lista
            if (!File.Exists(jsonFile))
                return new List<TestResult>();

            // Läser filen
            string json = File.ReadAllText(jsonFile);

            // Konverterar JSON till en lista med sparade resultat, om null returneras tom lista
            return JsonSerializer.Deserialize<List<TestResult>>(json) ?? new List<TestResult>();
        }

        // Sparar inlägg i JSON-filen
        public void Save(List<TestResult> results)
        {
            // Konverterar listans inlägg till JSON
            string json = JsonSerializer.Serialize(results);

            // Skriver innehållet till filen
            File.WriteAllText(jsonFile, json);
        }

    }
}