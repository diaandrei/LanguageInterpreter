namespace LanguageInterpreter
{
    public abstract class Expr
    {
        public abstract object Evaluate(Environment environment);
    }

    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message) { }
    }

    public class BinaryExpr : Expr
    {
        public Expr Left { get; }
        public Token Operator { get; }
        public Expr Right { get; }

        public BinaryExpr(Expr left, Token op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public override object Evaluate(Environment environment)
        {
            object leftVal = Left.Evaluate(environment);
            object rightVal = Right.Evaluate(environment);

            switch (Operator.Type)
            {
                case TokenType.PLUS:
                    if (leftVal is string)
                        return (string)leftVal + rightVal?.ToString();

                    if (rightVal is string)
                        return leftVal?.ToString() + (string)rightVal;

                    if (leftVal is double && rightVal is double)
                        return (double)leftVal + (double)rightVal;

                    throw new RuntimeException("Operands must be two numbers or at least one string.");

                case TokenType.MINUS:
                    if (leftVal is double && rightVal is double)
                        return (double)leftVal - (double)rightVal;
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.STAR:
                    if (leftVal is double && rightVal is double)
                        return (double)leftVal * (double)rightVal;
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.SLASH:
                    if (leftVal is double && rightVal is double)
                    {
                        if ((double)rightVal == 0)
                            throw new RuntimeException("Division by zero.");
                        return (double)leftVal / (double)rightVal;
                    }
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.GREATER:
                    if (leftVal is double && rightVal is double)
                        return (double)leftVal > (double)rightVal;
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.GREATER_EQUAL:
                    if (leftVal is double && rightVal is double)
                        return (double)leftVal >= (double)rightVal;
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.LESS:
                    if (leftVal is double && rightVal is double)
                        return (double)leftVal < (double)rightVal;
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.LESS_EQUAL:
                    if (leftVal is double && rightVal is double)
                        return (double)leftVal <= (double)rightVal;
                    throw new RuntimeException("Operands must be numbers.");

                case TokenType.EQUAL_EQUAL:
                    return IsEqual(leftVal, rightVal);

                case TokenType.BANG_EQUAL:
                    return !IsEqual(leftVal, rightVal);

                case TokenType.AND:
                    if (leftVal is bool && rightVal is bool)
                        return (bool)leftVal && (bool)rightVal;
                    throw new RuntimeException("Operands must be booleans.");

                case TokenType.OR:
                    if (leftVal is bool && rightVal is bool)
                        return (bool)leftVal || (bool)rightVal;
                    throw new RuntimeException("Operands must be booleans.");

                default:
                    throw new RuntimeException($"Unknown operator: {Operator.Lexeme}");
            }
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            if (a is string && b is string)
                return (string)a == (string)b;

            if (a is bool && b is bool)
                return (bool)a == (bool)b;

            if (a is double && b is double)
                return (double)a == (double)b;

            return false;
        }
    }

    public class GroupingExpr : Expr
    {
        public Expr Expression { get; }

        public GroupingExpr(Expr expression)
        {
            Expression = expression;
        }

        public override object Evaluate(Environment environment)
        {
            return Expression.Evaluate(environment);
        }
    }

    public class LiteralExpr : Expr
    {
        public object Value { get; }

        public LiteralExpr(object value)
        {
            Value = value;
        }

        public override object Evaluate(Environment environment)
        {
            return Value;
        }
    }

    public class UnaryExpr : Expr
    {
        public Token Operator { get; }
        public Expr Right { get; }

        public UnaryExpr(Token op, Expr right)
        {
            Operator = op;
            Right = right;
        }

        public override object Evaluate(Environment environment)
        {
            object rightVal = Right.Evaluate(environment);

            switch (Operator.Type)
            {
                case TokenType.MINUS:
                    if (rightVal is double)
                        return -(double)rightVal;
                    throw new RuntimeException("Operand must be a number.");

                case TokenType.BANG:
                    return !IsTruthy(rightVal);

                default:
                    throw new RuntimeException($"Unknown operator: {Operator.Lexeme}");
            }
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
    }

    public class VariableExpr : Expr
    {
        public Token Name { get; }

        public VariableExpr(Token name)
        {
            Name = name;
        }

        public override object Evaluate(Environment environment)
        {
            return environment.Get(Name.Lexeme);
        }
    }

    public class AssignExpr : Expr
    {
        public Token Name { get; }
        public Expr Value { get; }

        public AssignExpr(Token name, Expr value)
        {
            Name = name;
            Value = value;
        }

        public override object Evaluate(Environment environment)
        {
            object value = Value.Evaluate(environment);
            environment.Assign(Name.Lexeme, value);
            return value;
        }
    }

    public class InputExpr : Expr
    {
        public Expr Prompt { get; }

        public InputExpr(Expr prompt)
        {
            Prompt = prompt;
        }

        public override object Evaluate(Environment environment)
        {
            if (Prompt != null)
            {
                object promptValue = Prompt.Evaluate(environment);
                Console.Write(promptValue.ToString());
            }

            return Console.ReadLine();
        }
    }
}