// Ett provresultat som användaren registrerar

namespace BloodTestAnalyzer
{
    public class TestResult
    {
        public string? Name { get; set; }

        public double Value { get; set; }

        public DateOnly Date { get; set; }
    }
}