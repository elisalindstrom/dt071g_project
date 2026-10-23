/*
Projektuppgift kurs DT071G

Konsolapplikation som analyserar kvinnors testresultat från blodprover.

Av Elisa Lindström 2026
*/

namespace BloodTestAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            IBloodTestStorage storage = new JsonBloodTestStorage("bloodtests.json");
            BloodTestService bloodTestService = new BloodTestService(storage);
            ReferenceRanges referenceRanges = new ReferenceRanges();

            string? choice;

            do
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("BLODPROVSANALYS FÖR KVINNOR\n");
                Console.ResetColor();

                // Om det finns sparade provsvar visas dessa vid programstart
                if (bloodTestService.GetResults().Count > 0)
                {
                    Console.WriteLine("Sparade provsvar:");

                    int i = 0;

                    // Skriver ut alla sparade provsvar med index
                    foreach (TestResult testResult in bloodTestService.GetResults())
                    {
                        Biomarker? biomarker = referenceRanges.GetSpecificBiomarker(testResult.Name);
                        Console.WriteLine("[" + i++ + "] " + testResult.Name + ", " + testResult.Value + " " + biomarker?.Unit + ", " + testResult.Date);
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n1. Registrera nytt provsvar\n");
                Console.WriteLine("2. Ta bort ett provsvar\n");
                Console.WriteLine("X. Avsluta\n");
                Console.ResetColor();
                Console.Write("Ditt val: ");

                choice = Console.ReadLine()?.ToUpper();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nVälj en provtyp från listan");

                        foreach (Biomarker biomarkerName in referenceRanges.GetBiomarkers())
                        {
                            Console.WriteLine(biomarkerName.Name);
                        }

                        Console.Write("Ange provtyp: ");
                        string? nameInput = Console.ReadLine();
                        while (nameInput != "Hemoglobin" && nameInput != "Ferritin" && nameInput != "B12")
                        {
                            Console.WriteLine("Felaktig provtyp, ange typ som Hemoglobin, Ferritin eller B12: ");
                            nameInput = Console.ReadLine();
                        }

                        double value;
                        Console.Write("Ange provresultat: ");
                        string? valueInput = Console.ReadLine();
                        while (!Double.TryParse(valueInput, out value))
                        {
                            Console.WriteLine("Felaktigt provresult, ange resultat som ett tal: ");
                            valueInput = Console.ReadLine();
                        }

                        DateOnly date;
                        Console.Write("Ange provdatum (ÅÅÅÅ-MM-DD): ");
                        string? dateInput = Console.ReadLine();
                        while (!DateOnly.TryParseExact(dateInput, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                        {
                            Console.WriteLine("Felaktigt datum: ");
                            dateInput = Console.ReadLine();
                        }

                        TestResult testResult = bloodTestService.AddResult(nameInput, value, date);
                        Biomarker? biomarker = referenceRanges.GetSpecificBiomarker(nameInput);

                        if (biomarker != null)
                        {
                            string resultAnalysis = bloodTestService.AnalyzeResult(testResult, biomarker);

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\nANALYSRESULTAT");
                            Console.WriteLine($"Provtyp: {testResult.Name}");
                            Console.WriteLine($"Provresultat: {testResult.Value} {biomarker.Unit}");
                            Console.WriteLine($"Datum för prov: {testResult.Date}");
                            Console.WriteLine(resultAnalysis);
                            Console.ReadKey();
                            Console.ResetColor();
                        }

                        break;

                    case "2":
                        Console.Write("Ange index du vill ta bort: ");
                        string? index = Console.ReadLine();

                        // Kontrollerar att index inte är tomt
                        if (!String.IsNullOrEmpty(index))
                            try
                            {   // Konverterar string till en int och tar bort valt index
                                bloodTestService.DelResult(Convert.ToInt32(index));
                            }
                            // Om angivet index är felaktigt/inte finns
                            catch (Exception)
                            {
                                Console.WriteLine("Felaktigt index");
                                Console.ReadKey();
                            }
                        break;

                    case "X":
                        break;
                }
            }
            while (choice != "X");
        }
    }
}