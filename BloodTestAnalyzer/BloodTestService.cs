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

        // Jämför användarens provsvar med biomarkörens referensintervaller
        public string AnalyzeResult(TestResult result, Biomarker biomarker)
        {
            if (result.Value < biomarker.ReferenceMin)
                return $"Värdet är lågt och ligger under vårdens referensintervall.";

            if (result.Value > biomarker.ReferenceMax)
                return $"Värdet är högt och ligger över vårdens referensintervall.";

            if (result.Value < biomarker.OptimalMin)
                return $"Värdet är inom vårdens referensintervall, men ligger under det optimala referensintervallet.";

            if (result.Value > biomarker.OptimalMax)
                return $"Värdet är inom vårdens referensintervall, men ligger över det optimala referensintervallet.";

            return $"Dina värden är inom vårdens referensintervall och även inom den optimala referensintervallen.";
        }
    }
}