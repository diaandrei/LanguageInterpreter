namespace LanguageInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: LanguageInterpreter <filename>");
                return;
            }

            string filename = args[0];

            try
            {
                string[] lines = File.ReadAllLines(filename);
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
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Could not find file: {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}