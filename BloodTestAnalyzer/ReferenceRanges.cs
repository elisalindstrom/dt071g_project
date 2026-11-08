// Klass för biomarkörer och deras referensintervall

namespace BloodTestAnalyzer
{
    public class ReferenceRanges
    {
        private readonly List<Biomarker> biomarkers = new()
        {
            new Biomarker
            {
                Name = "Hemoglobin",
                Unit = "g/L",
                // Vårdens referensintervall
                ReferenceMin = 120,
                ReferenceMax = 155,
                // Optimala värden
                OptimalMin = 135,
                OptimalMax = 160
            },

            new Biomarker
            {
                Name = "Ferritin",
                Unit = "μg/L",
                ReferenceMin = 10,
                ReferenceMax = 150,
                OptimalMin = 30,
                OptimalMax = 120
            },

            new Biomarker
            {
                Name = "B12",
                Unit = "pmol/L",
                ReferenceMin = 145,
                ReferenceMax = 650,
                OptimalMin = 375,
                OptimalMax = 700
            }
        };

        // Hämtar en specifik biomarkör
        public Biomarker? GetSpecificBiomarker(string name)
        {
            return biomarkers.FirstOrDefault(b => b.Name == name);
        }

        // Hämtar hela listan med biomarkörer
        public List<Biomarker> GetBiomarkers()
        {
            return biomarkers;
        }
    }
}