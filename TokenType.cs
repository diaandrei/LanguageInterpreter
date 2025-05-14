namespace LanguageInterpreter
{
    public enum TokenType
    {
        LEFT_PAREN, RIGHT_PAREN,
        LEFT_BRACE, RIGHT_BRACE,
        MINUS, PLUS, SLASH, STAR,
        SEMICOLON,

        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        NUMBER, STRING, TRUE, FALSE,

        AND, OR,

        IDENTIFIER, PRINT,
        IF, ELSE, WHILE, INPUT,

        EOF
    }
}