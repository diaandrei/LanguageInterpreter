namespace LanguageInterpreter
{
    public abstract class Statement
    {
        public abstract void Execute(Environment environment);
    }

    public class ExpressionStatement : Statement
    {
        public Expr Expression { get; }

        public ExpressionStatement(Expr expression)
        {
            Expression = expression;
        }

        public override void Execute(Environment environment)
        {
            object result = Expression.Evaluate(environment);

            if (result != null && !(Expression is AssignExpr))
            {
                Console.WriteLine(result);
            }
        }
    }

    public class PrintStatement : Statement
    {
        public Expr Expression { get; }

        public PrintStatement(Expr expression)
        {
            Expression = expression;
        }

        public override void Execute(Environment environment)
        {
            object value = Expression.Evaluate(environment);
            Console.WriteLine(value);
        }
    }

    public class VariableStatement : Statement
    {
        public Token Name { get; }
        public Expr Initializer { get; }

        public VariableStatement(Token name, Expr initializer)
        {
            Name = name;
            Initializer = initializer;
        }

        public override void Execute(Environment environment)
        {
            object value = null;
            if (Initializer != null)
            {
                value = Initializer.Evaluate(environment);
            }
            environment.Define(Name.Lexeme, value);
        }
    }
}