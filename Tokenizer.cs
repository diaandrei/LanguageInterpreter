namespace LanguageInterpreter
{
    public class Tokenizer
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            { "and", TokenType.AND },
            { "or", TokenType.OR },
            { "true", TokenType.TRUE },
            { "false", TokenType.FALSE },
            { "print", TokenType.PRINT }
        };

        public Tokenizer(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null!, _line));
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '-':
                case '–':
                case '—':
                    AddToken(TokenType.MINUS);
                    break;
                case '+': AddToken(TokenType.PLUS); break;
                case '*': AddToken(TokenType.STAR); break;
                case '/': AddToken(TokenType.SLASH); break;

                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;

                case '"': String(); break;

                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    if (c == '\n') _line++;
                    break;

                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        throw new Exception($"Unexpected character: '{c}' (code: {(int)c})");
                    }
                    break;
            }
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') _line++;

                if (Peek() == '\\' && PeekNext() == '"')
                {
                    Advance();
                }

                Advance();
            }

            if (IsAtEnd())
            {
                throw new Exception("Unterminated string.");
            }

            Advance();

            string value = _source.Substring(_start + 1, _current - _start - 2);

            value = value.Replace("\\\"", "\"")
                        .Replace("\\n", "\n")
                        .Replace("\\t", "\t")
                        .Replace("\\\\", "\\");

            AddToken(TokenType.STRING, value);
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = _source.Substring(_start, _current - _start);

            TokenType type;
            if (!_keywords.TryGetValue(text, out type))
            {
                type = TokenType.IDENTIFIER;
            }

            AddToken(type);
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
            }

            double value = double.Parse(_source.Substring(_start, _current - _start));
            AddToken(TokenType.NUMBER, value);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            return true;
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private char Advance()
        {
            return _source[_current++];
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private void AddToken(TokenType type, object literal = null)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }
    }
}