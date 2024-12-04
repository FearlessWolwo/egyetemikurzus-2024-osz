using GTQPL7.Utils.TokenConverter;
using GTQPL7.Utils.Tokenizer;
using System.Runtime.CompilerServices;
using System.Text.Json;

using GTQPL7.Classes;
using GTQPL7.Utils.SymbolSorter;
using GTQPL7.Utils.RpnEvaluator;
using GTQPL7.Utils.ResultDisplayers;

namespace GTQPL7;

class Program
{
    private static readonly string ProjectFolder = GetSourceFilePathName();
    private static readonly Tokenizer Tokenizer = new Tokenizer();
    private static ITokenConverter TokenConverter;
    private static IResultDisplayer ResultDisplayer;
    private static readonly SymbolSorter SymbolSorter = new SymbolSorter();
    private static readonly RpnEvaluator RpnEvaluator = new RpnEvaluator();
    static void Main(string[] args)
    {
        LoadConfig();
        Console.WriteLine("Welcome to my matrix calculator!");
        Console.WriteLine("Please enter an equation!");
        Console.WriteLine("Type .help for the list of available operations and syntax guidance.");

        while (true)
        {
            string? input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }
            else
            {
                List<DslToken> tokens = Tokenizer.Tokenize(input);
                List<MathSymbol> mathSymbols = TokenConverter!.ConvertDslTokensToMathSymbols(tokens);
                Queue<MathSymbol> reversePolishNotation = SymbolSorter.Sort(mathSymbols);
                RpnEvaluator.Evaluate(reversePolishNotation);
            }
        }
    }

    static void LoadConfig()
    {
        string configFilePath = Path.Combine(ProjectFolder, "Configs/config.json");
        string json = File.ReadAllText(configFilePath);
        Dictionary<string, string>? configs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        TokenConverter = configs!["numberSet"] switch
        {
            "real" => new TokenConverter(),
            _ => throw new ArgumentException($"Unknown number set: {configs["numberSet"]}")
        };
        ResultDisplayer = configs!["result"] switch
        {
            "console" => new ConsoleDisplayer(),
            _ => throw new IOException($"Save to file not yet implemented")
        };
    }

    static string GetSourceFilePathName([CallerFilePath] string? path = null)
    {
        if (path != null)
        {
            path = Directory.GetParent(path)?.FullName;
        }
        return path ?? "";
    }
}