namespace LanguageInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Language Interpreter - Stage Selection");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Stage 1 - Basic Calculator");
                Console.WriteLine("2. Stage 2 - Boolean Logic");
                Console.WriteLine("3. Stage 3 - Text Values");
                Console.WriteLine("4. Stage 4 - Global Data");
                Console.WriteLine("5. Stage 5 - Control Flow");
                Console.WriteLine("6. Stage 6 - Lists");
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 0:
                            running = false;
                            break;
                        case 1:
                            RunStage("stage1_examples.txt");
                            break;
                        case 2:
                            RunStage("stage2_examples.txt");
                            break;
                        case 3:
                            RunStage("stage3_examples.txt");
                            break;
                        case 4:
                            RunStage("stage4_examples.txt");
                            break;
                        case 5:
                            RunStage("stage5_examples.txt");
                            break;
                        case 6:
                            RunStage("stage6_examples.txt");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }

                    if (running)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
            }
        }

        static void RunStage(string filename)
        {
            try
            {
                string fullPath = Path.Combine("examples", filename);
                Console.WriteLine($"Running {filename}...");
                Console.WriteLine();

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Error: File not found: {fullPath}");
                    return;
                }

                string[] lines = File.ReadAllLines(fullPath);
                string source = string.Join("\n", lines);

                try
                {
                    Tokenizer tokenizer = new Tokenizer(source);
                    List<Token> tokens = tokenizer.ScanTokens();

                    Parser parser = new Parser(tokens);
                    List<Statement> statements = parser.Parse();

                    Environment environment = new Environment();

                    foreach (var statement in statements)
                    {
                        statement.Execute(environment);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}