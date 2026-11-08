// Jämför användarens provsvar med vårdens och det optimala referensintervallet

namespace BloodTestAnalyzer
{
    public class TestAnalyzer
    {
        public string AnalyzeResult(TestResult result, Biomarker biomarker)
        {
            string referenceAnalysis;
            string optimalAnalysis;

            // Provsvar jämförs med vårdens referensintervall
            if (result.Value < biomarker.ReferenceMin)
                referenceAnalysis = "Värdet är lågt och ligger under vårdens referensintervall.";
            else if (result.Value > biomarker.ReferenceMax)
                referenceAnalysis = "Värdet är högt och ligger över vårdens referensintervall.";
            else
                referenceAnalysis = "Värdet är inom vårdens referensintervall.";

            // Provsvar jämförs med optimala värden
            if (result.Value < biomarker.OptimalMin)
                optimalAnalysis = "Värdet ligger under det optimala intervallet.";
            else if (result.Value > biomarker.OptimalMax)
                optimalAnalysis = "Värdet ligger över det optimala intervallet.";
            else
                optimalAnalysis = "Värdet är inom det optimala intervallet.";

            return $"{referenceAnalysis} {optimalAnalysis}";
        }
    }
}