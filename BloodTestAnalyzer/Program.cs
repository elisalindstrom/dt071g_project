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

            Console.WriteLine("BLODPROVSANALYS FÖR KVINNOR\n");
            Console.WriteLine("\n1. Registrera nytt provsvar\n");
            Console.WriteLine("2. Ta bort ett provsvar\n");
            Console.WriteLine("X. Avsluta\n");
            Console.Write("Ditt val: ");

            string? choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "1":
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

                    break;

                case "X":
                    break;
            }
        }
    }
}