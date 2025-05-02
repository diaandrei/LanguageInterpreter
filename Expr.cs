namespace LanguageInterpreter
{
    public abstract class Expr
    {
        public abstract object Evaluate();
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

        public override object Evaluate()
        {
            object leftVal = Left.Evaluate();
            object rightVal = Right.Evaluate();

            if (leftVal is double left && rightVal is double right)
            {
                switch (Operator.Type)
                {
                    case TokenType.PLUS:
                        return left + right;
                    case TokenType.MINUS:
                        return left - right;
                    case TokenType.STAR:
                        return left * right;
                    case TokenType.SLASH:
                        if (right == 0)
                            throw new System.Exception("Division by zero.");
                        return left / right;
                    default:
                        throw new System.Exception($"Unknown operator: {Operator.Lexeme}");
                }
            }

            throw new System.Exception("Operands must be numbers.");
        }
    }

    public class GroupingExpr : Expr
    {
        public Expr Expression { get; }

        public GroupingExpr(Expr expression)
        {
            Expression = expression;
        }

        public override object Evaluate()
        {
            return Expression.Evaluate();
        }
    }

    public class LiteralExpr : Expr
    {
        public object Value { get; }

        public LiteralExpr(object value)
        {
            Value = value;
        }

        public override object Evaluate()
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

        public override object Evaluate()
        {
            object rightVal = Right.Evaluate();

            if (rightVal is double value)
            {
                switch (Operator.Type)
                {
                    case TokenType.MINUS:
                        return -value;
                    default:
                        throw new System.Exception($"Unknown operator: {Operator.Lexeme}");
                }
            }

            throw new System.Exception("Operand must be a number.");
        }
    }
}