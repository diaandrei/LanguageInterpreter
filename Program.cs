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

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        var tokenizer = new Tokenizer(line);
                        var tokens = tokenizer.ScanTokens();

                        var parser = new Parser(tokens);
                        var expression = parser.Parse();

                        if (expression != null)
                        {
                            var result = expression.Evaluate();
                            Console.WriteLine(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
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