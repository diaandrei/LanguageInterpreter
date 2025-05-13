namespace LanguageInterpreter
{
    public enum TokenType
    {
        LEFT_PAREN, RIGHT_PAREN,
        MINUS, PLUS, SLASH, STAR,
        SEMICOLON,

        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        NUMBER, STRING, TRUE, FALSE,

        AND, OR,

        IDENTIFIER, PRINT,

        EOF
    }
}