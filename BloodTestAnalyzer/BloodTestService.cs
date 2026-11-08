namespace BloodTestAnalyzer
{
    public class BloodTestService
    {
        private readonly IBloodTestStorage storage;
        private List<TestResult> results;

        // Konstruktor som hämtar tidigare sparade provsvar från storage
        public BloodTestService(IBloodTestStorage storage)
        {
            this.storage = storage;
            results = storage.Load();
        }

        // Skapar och sparar nytt provsvar
        public TestResult AddResult(string name, double value, DateOnly date)
        {
            TestResult testResult = new TestResult();
            testResult.Name = name;
            testResult.Value = value;
            testResult.Date = date;

            // Lägger till och sparar listan
            results.Add(testResult);
            storage.Save(results);

            return testResult;
        }

        // Tar bort provsvar utifrån index
        public int DelResult(int index)
        {
            results.RemoveAt(index);
            storage.Save(results);

            return index;
        }

        // Hämtar alla provsvar
        public List<TestResult> GetResults()
        {
            return results;
        }
    }
}