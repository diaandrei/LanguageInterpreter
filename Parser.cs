namespace LanguageInterpreter
{
    public class Parser
    {
        private class ParseError : Exception { }

        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<Statement> Parse()
        {
            List<Statement> statements = new List<Statement>();
            while (!IsAtEnd())
            {
                statements.Add(Statement());
            }

            return statements;
        }

        private Statement Statement()
        {
            Statement stmt;

            if (Match(TokenType.IF))
                stmt = IfStatement();
            else if (Match(TokenType.WHILE))
                stmt = WhileStatement();
            else if (Match(TokenType.LEFT_BRACE))
                stmt = new BlockStatement(Block());
            else if (Match(TokenType.PRINT))
                stmt = PrintStatement();
            else if (Match(TokenType.IDENTIFIER) && Check(TokenType.EQUAL))
            {
                Token name = Previous();
                Consume(TokenType.EQUAL, "Expect '=' after variable name.");
                Expr value = Expression();
                stmt = new VariableStatement(name, value);
            }
            else
                stmt = ExpressionStatement();

            Match(TokenType.SEMICOLON);

            return stmt;
        }

        private Statement IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
            Expr condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition.");

            Statement thenBranch = Statement();
            Statement elseBranch = null;
            if (Match(TokenType.ELSE))
            {
                elseBranch = Statement();
            }

            return new IfStatement(condition, thenBranch, elseBranch);
        }

        private Statement WhileStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'while'.");
            Expr condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
            Statement body = Statement();

            return new WhileStatement(condition, body);
        }

        private List<Statement> Block()
        {
            List<Statement> statements = new List<Statement>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Statement());
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;
        }

        private Statement PrintStatement()
        {
            Expr value = Expression();
            return new PrintStatement(value);
        }

        private Statement ExpressionStatement()
        {
            Expr expr = Expression();
            return new ExpressionStatement(expr);
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            Expr expr = Or();

            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is VariableExpr)
                {
                    Token name = ((VariableExpr)expr).Name;
                    return new AssignExpr(name, value);
                }

                Error(equals, "Invalid assignment target.");
            }

            return expr;
        }

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.OR))
            {
                Token op = Previous();
                Expr right = And();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr And()
        {
            Expr expr = Equality();

            while (Match(TokenType.AND))
            {
                Token op = Previous();
                Expr right = Equality();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = Term();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = Factor();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(TokenType.STAR, TokenType.SLASH))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = Unary();
                return new UnaryExpr(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new LiteralExpr(false);
            if (Match(TokenType.TRUE)) return new LiteralExpr(true);
            if (Match(TokenType.NUMBER)) return new LiteralExpr(Previous().Literal);
            if (Match(TokenType.STRING)) return new LiteralExpr(Previous().Literal);

            if (Match(TokenType.IDENTIFIER))
                return new VariableExpr(Previous());

            if (Match(TokenType.INPUT))
            {
                Consume(TokenType.LEFT_PAREN, "Expect '(' after 'input'.");
                Expr prompt = null!;

                if (!Check(TokenType.RIGHT_PAREN))
                {
                    prompt = Expression();
                }

                Consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments.");
                return new InputExpr(prompt);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new GroupingExpr(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();

            throw Error(Peek(), message);
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private ParseError Error(Token token, string message)
        {
            Console.Error.WriteLine($"Error at {token}: {message}");
            return new ParseError();
        }
    }
}