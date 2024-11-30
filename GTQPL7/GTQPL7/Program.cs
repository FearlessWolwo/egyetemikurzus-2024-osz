using System.Globalization;
using System.Numerics;

using GTQPL7.Utils.TokenConverter;
using GTQPL7.Utils.Tokenizer;
using System.Runtime.CompilerServices;
using System.Text.Json;

using GTQPL7.Classes;

namespace GTQPL7;

class Program
{
    private static readonly string ProjectFolder = GetSourceFilePathName();
    private static readonly Tokenizer Tokenizer = new Tokenizer();
    private static ITokenConverter? s_tokenConverter;
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
                List<MathSymbol> mathSymbols = s_tokenConverter!.ConvertDslTokensToMathSymbols(tokens);
            }
        }
    }

    static void LoadConfig()
    {
        string configFilePath = Path.Combine(ProjectFolder, "Configs/config.json");
        string json = File.ReadAllText(configFilePath);
        Dictionary<string, string>? configs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        s_tokenConverter = configs!["numberSet"] switch
        {
            "int" => new TokenConverter<int>(),
            "real" => new TokenConverter<double>(),
            "complex" => new TokenConverter<Complex>(),
            _ => throw new Exception($"Unknown number set: {configs["numberSet"]}")
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