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
            IBloodTestStorage storage = new JsonBloodTestStorage("bloodtests.json");
            BloodTestService bloodTestService = new BloodTestService(storage);
            ReferenceRanges referenceRanges = new ReferenceRanges();

            string? choice;

            do
            {
                Console.Clear();

                choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[bold Purple4]BLODPROVSANALYS FÖR KVINNOR[/]")
                    .HighlightStyle(new Style(Color.Purple4))
                    .AddChoices("Registrera nytt provsvar", "Visa sparade provsvar", "Ta bort provsvar", "[red3]Avsluta[/]")
                );

                switch (choice)
                {
                    case "Registrera nytt provsvar":
                        // Typ av prov
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

                        TestResult testResult = bloodTestService.AddResult(nameInput, valueInput, date);
                        Biomarker? biomarker = referenceRanges.GetSpecificBiomarker(nameInput);

                        // Spinner
                        AnsiConsole.Status()
                            .Start("Laddar analysresultat...", ctx =>
                            {
                                Thread.Sleep(3000);
                            });

                        // Analysresultat
                        if (biomarker != null)
                        {
                            Console.Clear();

                            string resultAnalysis = bloodTestService.AnalyzeResult(testResult, biomarker);

                            var panel = new Panel(
                                $"[bold]Provtyp:[/] {testResult.Name}\n" +
                                $"[bold]Provresultat:[/] {testResult.Value} {biomarker.Unit}\n" +
                                $"[bold]Datum för prov:[/] {testResult.Date}\n\n" +
                                $"[bold]Vårdens referensintervall:[/] {biomarker.ReferenceMin}-{biomarker.ReferenceMax} {biomarker.Unit}\n" +
                                $"[bold]Optimalt referensintervall:[/] {biomarker.OptimalMin}-{biomarker.OptimalMax} {biomarker.Unit}\n" +
                                $"[purple4]{resultAnalysis}[/]\n\n" +
                                "Applikationen ställer inga medicinska diagnoser"
                            )
                            .Header("[bold]ANALYSRESULTAT[/]", Justify.Center)
                            .Padding(2, 1)
                            .RoundedBorder();

                            AnsiConsole.Write(panel);
                            Console.ReadKey();
                        }

                        break;

                    case "Visa sparade provsvar":
                        // Om det finns sparade provsvar visas dessa vid programstart
                        if (bloodTestService.GetResults().Count > 0)
                        {
                            // Skapar ny tabell med kolumner
                            var table = new Table()
                            .Border(TableBorder.Rounded)
                            .Title("[bold]SPARADE PROVSVAR[/]")
                            .AddColumn("[bold]Nr[/]")
                            .AddColumn("[bold]Biomarkör[/]")
                            .AddColumn("[bold]Provresultat[/]")
                            .AddColumn("[bold]Datum[/]");

                            int i = 0;

                            // Skriver ut alla sparade provsvar i tabellen
                            foreach (TestResult savedTestResult in bloodTestService.GetResults())
                            {
                                Biomarker? savedBiomarker = referenceRanges.GetSpecificBiomarker(savedTestResult.Name);
                                table.AddRow(i++.ToString(), savedTestResult.Name, $"{savedTestResult.Value} {savedBiomarker?.Unit}", savedTestResult.Date.ToString("yyyy-MM-dd"));

                            }
                            AnsiConsole.Write(table);
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Det finns inga sparade provresult");
                            Console.ReadKey();

                        }

                        break;

                    case "Ta bort provsvar":
                        if (bloodTestService.GetResults().Count > 0)
                        {
                            // Skapar ny tabell med kolumner
                            var table = new Table()
                            .Border(TableBorder.Rounded)
                            .Title("[bold]SPARADE PROVSVAR[/]")
                            .AddColumn("[bold]Nr[/]")
                            .AddColumn("[bold]Biomarkör[/]")
                            .AddColumn("[bold]Provresultat[/]")
                            .AddColumn("[bold]Datum[/]");

                            int i = 0;

                            // Skriver ut alla sparade provsvar i tabellen
                            foreach (TestResult savedTestResult in bloodTestService.GetResults())
                            {
                                Biomarker? savedBiomarker = referenceRanges.GetSpecificBiomarker(savedTestResult.Name);
                                table.AddRow(i++.ToString(), savedTestResult.Name, $"{savedTestResult.Value} {savedBiomarker?.Unit}", savedTestResult.Date.ToString("yyyy-MM-dd"));

                            }
                            AnsiConsole.Write(table);

                            Console.Write("Ange nr för det provsvar du vill ta bort: ");
                            string? index = Console.ReadLine();

                            // Kontrollerar att index inte är tomt
                            if (!String.IsNullOrEmpty(index))
                                try
                                {   // Konverterar string till en int och tar bort valt index
                                    bloodTestService.DelResult(Convert.ToInt32(index));
                                    AnsiConsole.MarkupLine("[green]Provsvar borttaget ✓[/]");
                                    Console.ReadKey();
                                }
                                // Om angivet index är felaktigt/inte finns
                                catch (Exception)
                                {
                                    Console.WriteLine("Felaktigt index");
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
            while (choice != "[red3]Avsluta[/]");
        }
    }
}