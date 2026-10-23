// Klass för biomarkörers referensintervaller

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
                ReferenceMin = 120,
                ReferenceMax = 155,
                OptimalMin = 130,
                OptimalMax = 140
            },

            new Biomarker
            {
                Name = "Ferritin",
                Unit = "μg/L",
                ReferenceMin = 10,
                ReferenceMax = 150,
                OptimalMin = 50,
                OptimalMax = 100
            },

            new Biomarker
            {
                Name = "B12",
                Unit = "pmol/L",
                ReferenceMin = 145,
                ReferenceMax = 650,
                OptimalMin = 250,
                OptimalMax = 650
            }
        };

        // Hämta en specifik biomarkör
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