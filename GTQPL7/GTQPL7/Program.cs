using GTQPL7.Utils.TokenConverter;
using GTQPL7.Utils.Tokenizer;
using System.Runtime.CompilerServices;
using System.Text.Json;

using GTQPL7.Classes;
using GTQPL7.Exceptions;
using GTQPL7.Utils.Interactors;
using GTQPL7.Utils.SymbolSorter;
using GTQPL7.Utils.RpnEvaluator;
using GTQPL7.Utils.ResultDisplayers;

namespace GTQPL7;

class Program
{
    private static readonly string ProjectFolder = GetSourceFilePathName();
    private static readonly Tokenizer Tokenizer = new Tokenizer();
    private static TokenConverter TokenConverter;
    private static IResultDisplayer ResultDisplayer;
    private static readonly SymbolSorter SymbolSorter = new SymbolSorter();
    private static readonly RpnEvaluator RpnEvaluator = new RpnEvaluator();

    private static string helpString = """
                                       .help - opens this menu.
                                       .inputfile [filepath] - evaluates the input file. Use absolute path. The first line of the file must contain the expression.
                                       .outputfile [filepath] - redirect the output to the file. Use absolute path.
                                       .console - display the output on the console. (default)
                                       .help eq - lists the available operations.
                                       .exit - exits the program.
                                       """;

    private static string equationHelpString = """
                                               Available operations:
                                               + [possible operands: (scalar, scalar), (matrix, matrix)]
                                               - [possible operands: (scalar, scalar), (matrix, matrix)]
                                               * [possible operands: (scalar, matrix), (matrix, matrix)]
                                               det() [possible operands: (matrix)]
                                               inv() [possible operands: (matrix)]
                                               trans() [possible operands: (matrix)]
                                               () brackets
                                               
                                               Using variables:
                                               numbers are constants (Ex. 3.14)
                                               lowercase letters are scalar (Ex. a)
                                               uppercase letters are matrices (Ex. B)
                                               
                                               Separate operators and variables by spaces (expect brackets).
                                               Putting "-" directly before a scalar results in a negative value and not a subtraction.
                                               After inputting an expression you will be asked to define the variables.
                                               For scalar values, input a number.
                                               For matrices input it's dimensions separated by space and for each subsequent row input {column} scalars separated by space.
                                               
                                               Example expression:
                                               C * (a * A + trans(B))
                                               """;
    
    static void Main(string[] args)
    {
        LoadConfig();
        Console.WriteLine("Welcome to my matrix calculator!");
        Console.WriteLine("Please enter an equation!");
        Console.WriteLine("Type \".help eq\" for the list of available operations and syntax guidance.");
        Console.WriteLine("Type \".help\" for the list of available commands.");

        while (true)
        {
            string? input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }
            if (input.Equals(".help", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine(helpString);
            }
            else if (input.Equals(".help eq", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine(equationHelpString);
            }
            else if (input.StartsWith(".inputfile", StringComparison.CurrentCultureIgnoreCase))
            {
                string[] param = input.Split(" ");
                try
                {
                    string[] fileContents = File.ReadAllLines(Path.GetFullPath(param[1]));
                    IInteractor interactor = TokenConverter.Interactor;
                    TokenConverter.Interactor = new FileInteractor(fileContents);
                    Execute(fileContents[0]);
                    TokenConverter.Interactor = interactor;
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Can't open file");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("No such file");
                }
            }
            else if (input.StartsWith(".outputfile", StringComparison.CurrentCultureIgnoreCase))
            {
                string[] param = input.Split(" ");
                ResultDisplayer = new FileDisplayer(param[1]);
                RpnEvaluator.ResultDisplayer = ResultDisplayer;
            }
            else if (input.Equals(".console", StringComparison.CurrentCultureIgnoreCase))
            {
                ResultDisplayer = new ConsoleDisplayer();
                RpnEvaluator.ResultDisplayer = ResultDisplayer;
            }
            else if (input.Equals(".exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Environment.Exit(0);
            }
            else
            {
                Execute(input);
            }
        }
    }

    static void LoadConfig()
    {
        string configFilePath = Path.Combine(ProjectFolder, "Configs/config.json");
        string json = "";

        try
        {
            json = File.ReadAllText(configFilePath);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Could not open config file. No access.");
            Environment.Exit(0);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Could not open config file. File not found.");
            Environment.Exit(0);
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected error occured.");
            Environment.Exit(0);
        }
        
        Dictionary<string, string>? configs = null;

        try
        {
            configs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        catch (JsonException)
        {
            Console.WriteLine("Error parsing config file.");
            Environment.Exit(0);
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected error occured.");
            Environment.Exit(0);
        }

        switch (configs["numberSet"])
        {
            case "real":
                TokenConverter = new TokenConverter();
                break;
            default:
                Console.WriteLine($"Unknown number set: {configs["numberSet"]}");
                Environment.Exit(0);
                break;
        }

        switch (configs["result"])
        {
            case "console":
                ResultDisplayer = new ConsoleDisplayer();
                break;
            default:
                Console.WriteLine("Not yet implemented.");
                Environment.Exit(0);
                break;
        }
    }

    static string GetSourceFilePathName([CallerFilePath] string? path = null)
    {
        if (path != null)
        {
            path = Directory.GetParent(path)?.FullName;
        }
        return path ?? "";
    }

    static void Execute(string input)
    {
        List<DslToken>? tokens = null;

        try
        {
            tokens = Tokenizer.Tokenize(input);
        }
        catch (TokenizerException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception occured.");
            Environment.Exit(0);
        }

        List<MathSymbol>? mathSymbols = null;

        try
        {
            mathSymbols = TokenConverter.ConvertDslTokensToMathSymbols(tokens);
        }
        catch (TokenConverterException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception occured.");
            Environment.Exit(0);
        }

        Queue<MathSymbol>? reversePolishNotation = null;

        try
        {
            reversePolishNotation = SymbolSorter.Sort(mathSymbols);
        }
        catch (SymbolSorterException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception occured.");
            Environment.Exit(0);
        }

        try
        {
            RpnEvaluator.Evaluate(reversePolishNotation!);
        }
        catch (RpnEvaluatorException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Input file does not exist.");
        }
        catch (AccessViolationException)
        {
            Console.WriteLine("Could not access the input file.");
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception occured.");
            Environment.Exit(0);
        }
    }
}