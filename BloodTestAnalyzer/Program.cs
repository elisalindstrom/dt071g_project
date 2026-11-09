/*
Projektuppgift kurs DT071G

Konsolapplikation som analyserar kvinnors testresultat från blodprover.

Av Elisa Lindström 2026
*/

using Spectre.Console;

namespace BloodTestAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Skapar objekt av nödvändiga klasser
            IBloodTestStorage storage = new JsonBloodTestStorage("bloodtests.json");
            BloodTestService bloodTestService = new BloodTestService(storage);
            ReferenceRanges referenceRanges = new ReferenceRanges();
            TestAnalyzer testAnalyzer = new TestAnalyzer();

            string? choice;

            // Loop för att visa menyn så länge användaren inte avslutar programmet
            do
            {
                Console.Clear();

                choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[bold Purple4]BLODPROVSANALYS FÖR KVINNOR[/]")
                    .HighlightStyle(new Style(Color.Purple4))
                    .AddChoices("Registrera nytt provsvar", "Hantera sparade provsvar", "[red3]Avsluta[/]")
                );

                // Hanterar menyval
                switch (choice)
                {
                    case "Registrera nytt provsvar":
                        // Validering
                        string nameInput = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Välj provtyp")
                            .HighlightStyle(new Style(Color.Purple4))
                            .AddChoices(referenceRanges.GetBiomarkers().Select(biomarker => biomarker.Name))
                        );

                        AnsiConsole.MarkupLine($"Du valde {nameInput}");

                        double valueInput = AnsiConsole.Prompt(
                            new TextPrompt<double>("Ange provresultat:")
                            .ValidationErrorMessage("Felaktigt provresult, ange resultat som ett tal")
                        );

                        string dateInput = AnsiConsole.Prompt(
                            new TextPrompt<string>("Ange provdatum (ÅÅÅÅ-MM-DD): ")
                            .Validate(input =>
                            {
                                // Kontrollerar att datumformatet är korrekt
                                if (DateOnly.TryParseExact(input, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
                                {
                                    return ValidationResult.Success();
                                }
                                else
                                {
                                    return ValidationResult.Error("Felaktigt datum. Använd formatet ÅÅÅÅ-MM-DD");
                                }
                            })
                        );

                        DateOnly date = DateOnly.ParseExact(dateInput, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                        // Sparar testresultatet
                        TestResult testResult = bloodTestService.AddResult(nameInput, valueInput, date);
                        // Hämtar rätt biomarkör utifrån användarens val
                        Biomarker? biomarker = referenceRanges.GetSpecificBiomarker(nameInput);

                        // Spinner
                        AnsiConsole.Status()
                        .SpinnerStyle(new Style(Color.Purple4))
                            .Start("Laddar analysresultat...", ctx =>
                            {
                                Thread.Sleep(3000);
                            });

                        // Analysresultat
                        // Kontrollerar att biomarker inte är null
                        if (biomarker != null)
                        {
                            Console.Clear();

                            // Anropar analysmetod
                            string resultAnalysis = testAnalyzer.AnalyzeResult(testResult, biomarker);

                            // Skriver ut resultatet i en panel
                            var panel = new Panel(
                                $"[bold]Provtyp:[/] {testResult.Name}\n" +
                                $"[bold]Provresultat:[/] {testResult.Value} {biomarker.Unit}\n" +
                                $"[bold]Datum för prov:[/] {testResult.Date}\n\n" +
                                $"[bold]Vårdens referensintervall:[/] {biomarker.ReferenceMin}-{biomarker.ReferenceMax} {biomarker.Unit}\n" +
                                $"[bold]Optimalt intervall:[/] {biomarker.OptimalMin}-{biomarker.OptimalMax} {biomarker.Unit}\n" +
                                $"[purple4]{resultAnalysis}[/]\n\n" +
                                "Applikationen ställer inga medicinska diagnoser"
                            )
                            .Header("[bold]ANALYSRESULTAT[/]", Justify.Center)
                            .Padding(2, 1)
                            .RoundedBorder();

                            AnsiConsole.Write(panel);

                            AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .HighlightStyle(new Style(Color.Purple4))
                                .AddChoices("Tillbaka till huvudmenyn")
                            );
                        }

                        break;

                    case "Hantera sparade provsvar":
                        // Kontrollerar om det finns sparade provsvar
                        if (bloodTestService.GetResults().Count > 0)
                        {
                            // Skapar tabell med kolumner
                            var table = new Table()
                            .Border(TableBorder.Rounded)
                            .Title("[bold black]SPARADE PROVSVAR[/]")
                            .AddColumn("[bold]Nr[/]")
                            .AddColumn("[bold]Biomarkör[/]")
                            .AddColumn("[bold]Provresultat[/]")
                            .AddColumn("[bold]Datum[/]");

                            int i = 0;

                            // Skriver ut alla sparade provsvar i tabellen, sortera efter datum
                            foreach (TestResult savedTestResult in bloodTestService.GetResults().OrderByDescending(result => result.Date))
                            {
                                Biomarker? savedBiomarker = referenceRanges.GetSpecificBiomarker(savedTestResult.Name);
                                table.AddRow(i++.ToString(), savedTestResult.Name, $"{savedTestResult.Value} {savedBiomarker?.Unit}", savedTestResult.Date.ToString("yyyy-MM-dd"));

                            }

                            AnsiConsole.Write(table);

                            string subChoice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("[bold]Välj åtgärd[/]")
                                .HighlightStyle(new Style(Color.Purple4))
                                .AddChoices("Ta bort ett provsvar", "Tillbaka till huvudmenyn")
                            );

                            if (subChoice == "Ta bort ett provsvar")
                            {
                                int index = AnsiConsole.Prompt(
                                    new TextPrompt<int>("Ange nr för det provsvar du vill ta bort:")
                                    .ValidationErrorMessage("Ange ett giltigt heltal.")
                                    .Validate(value =>
                                    {
                                        // Kontrollerar att angett index finns
                                        if (value < 0 || value >= bloodTestService.GetResults().Count)
                                        {
                                            return ValidationResult.Error("Felaktigt index");
                                        }
                                        else
                                        {
                                            return ValidationResult.Success();
                                        }
                                    })
                                );

                                // Tar bort provsvaret med valt index
                                bloodTestService.DelResult(index);
                                AnsiConsole.MarkupLine("[green]Provsvar borttaget ✓[/]");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Det finns inga sparade provresult");
                            Console.ReadKey();
                        }

                        break;

                    case "[red3]Avsluta[/]":
                        break;
                }
            }
            // Visar huvudmenyn så länge användaren inte väljer Avsluta
            while (choice != "[red3]Avsluta[/]");
        }
    }
}