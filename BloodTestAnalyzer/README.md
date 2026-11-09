# Projektuppgift BloodTestAnalyzer
Konsolapplikation utvecklad i C# som analyserar kvinnors provresultat från blodprover. Programmet jämför användarens provsvar med vårdens referensintervall samt ett optimalt intervall. Det optimala intervallet representerar ett värde som bedömds hälsomässigt bättre att ligga inom.

## Installation
Nedklonat projekt körs med kommando dotnet run.

## Användning
Användaren kan lägga till och ta bort provsvar som lagras lokalt i en JSON-fil. Första gången ett provsvar sparas skapas lagringsfilen. När användaren lägger till ett nytt provsvar analyseras värdet utifrån de två referensintervallen.

## Projektstruktur
```bash
└── BloodTestAnalyzer
    ├── Models
    │   ├── Biomarker.cs
    │   └── TestResult.cs
    ├── Storage
    │   ├── IBloodTestStorage.cs
    │   └── JsonBloodTestStorage.cs
    ├── BloodTestAnalyzer.csproj
    ├── BloodTestService.cs
    ├── Program.cs
    ├── ReferenceRanges.cs
    └── TestAnalyzer.cs
```

Applikationen ställer inga medicinska diagnoser.

Av Elisa L. 2026