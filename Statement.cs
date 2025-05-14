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

    public class BlockStatement : Statement
    {
        public List<Statement> Statements { get; }

        public BlockStatement(List<Statement> statements)
        {
            Statements = statements;
        }

        public override void Execute(Environment environment)
        {
            foreach (var statement in Statements)
            {
                statement.Execute(environment);
            }
        }
    }

    public class IfStatement : Statement
    {
        public Expr Condition { get; }
        public Statement ThenBranch { get; }
        public Statement ElseBranch { get; }

        public IfStatement(Expr condition, Statement thenBranch, Statement elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override void Execute(Environment environment)
        {
            if (IsTruthy(Condition.Evaluate(environment)))
            {
                ThenBranch.Execute(environment);
            }
            else if (ElseBranch != null)
            {
                ElseBranch.Execute(environment);
            }
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
    }

    public class WhileStatement : Statement
    {
        public Expr Condition { get; }
        public Statement Body { get; }

        public WhileStatement(Expr condition, Statement body)
        {
            Condition = condition;
            Body = body;
        }

        public override void Execute(Environment environment)
        {
            while (IsTruthy(Condition.Evaluate(environment)))
            {
                Body.Execute(environment);
            }
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
    }
}