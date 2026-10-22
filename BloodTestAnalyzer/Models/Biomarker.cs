// Klass för biomarkörer

namespace BloodTestAnalyzer
{
    public class Biomarker
    {
        public string? Name { get; set; }

        // Enhet biomarkören mäts i
        public string? Unit { get; set; }

        // Vårdens referensintervaller
        public double ReferenceMin { get; set; }

        public double ReferenceMax { get; set; }

        // Optimala referensintervaller
        public double OptimalMin { get; set; }

        public double OptimalMax { get; set; }
    }
}