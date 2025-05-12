# Roslyn Samples

Ce repo est un compagnon pour ma présentation sur Roslyn et les générateurs de code donnée le 12 mai 2025 lors du meetup MTG:Toulouse.

## Projets

- 00-roslyn : contient des projets pour illustrer le compilateur roslyn et son utilisation
- 01-analyzer-quickstart : support pour créer ses propres analyseurs. Exemple, tests, package et demo
- 02-codegen-quickstart : support pour créer son propre générateur de code. Exemple, tests et demo.

## Liens

### Roslyn compiler

- [Roslyn API - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/). Doc officielle sur l'API Roslyn
- [Roslyn project - github.com/dotnet/roslyn](https://github.com/dotnet/roslyn). Repo officiel du projet Roslyn.
- [Roslyn Category - Meziantou's Blog](https://www.meziantou.net/archives-categories-roslyn.htm). Articles de blog à propos de Roslyn.
- [Roslyn Quoter - github.com/KirillOsenkov/RoslynQuoter](https://github.com/KirillOsenkov/RoslynQuoter). Générateurs d'appels API Roslyn pour construire l'arbre syntaxique d'un code source donné.

### Analyzers

- [Analyzers Docs - github.com/dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/docs/analyzers/). Articles documentation sur les analyseurs Roslyn.
- [Roslyn Analyzers - github.com/dotnet/roslyn-analyzers](https://github.com/dotnet/roslyn-analyzers). Analyseurs Roslyn officiels pour le code .NET.
- [Writing a Roslyn Analyzer - Meziantou's blog](https://www.meziantou.net/writing-a-roslyn-analyzer.htm). Tuto : comment écrire un analyseur Roslyn.
- [Awesome Analyzers - github.com/Cybermaxs](https://github.com/Cybermaxs/awesome-analyzers). Liste de liens sur les analyseurs Roslyn, exemples et outils.
- [Tutorial: Write your first analyzer and code fix - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix). Tuto : écrire un analyseur et un correcteur de code.

### Source generators

- [Using Source Generators for Fun (and Maybe Profit) (Video) - youtube.com/dotnet](https://www.youtube.com/watch?v=4DVV7FXukC8). Présentation de base des générateurs de code.
- [Incremental Generators - github.com/dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)
- [Incremental Generators Cookbook - github.com/dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md)
- [Creating an incremental generator - Andrew Lock Blog](https://andrewlock.net/creating-a-source-generator-part-1-creating-an-incremental-source-generator/)
- [Let's Build an incremental source generator with Roslyn (Video) - Stefan Pölz](https://www.jetbrains.com/guide/dotnet/links/lets-build-an-incremental-source-generator-with-roslyn/)
- [Testing Roslyn Incremental Generators - Meziantou's Blog](https://www.meziantou.net/testing-roslyn-incremental-source-generators.htm)
- [Source Generators (Legacy) - github.com/dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md)