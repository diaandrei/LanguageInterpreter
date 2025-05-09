namespace LanguageInterpreter
{
    public enum TokenType
    {
        LEFT_PAREN, RIGHT_PAREN,
        MINUS, PLUS, SLASH, STAR,

        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        NUMBER, STRING, TRUE, FALSE,

        AND, OR,

        EOF
    }
}