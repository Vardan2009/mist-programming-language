using static mist_programming_language.Tokens;

namespace mist_programming_language
{
    public class Tokens
    {
        public enum TokenType
        {
            ID,LPAREN,RPAREN,STRING,SEMI,EOF,ASSIGN,LBRACE,RBRACE,NUMBER,PLUS,MINUS,MUL,DIV,COMMA,EQEQ,NEQEQ,NOT,GT,LT
        }
    }
    public class Token
    {
        public TokenType type;
        public string value = "";
        public int line,charac;
        public Token(TokenType _type)
        {
            type = _type;
        }
        public Token(TokenType _type,int l,int c)
        {
            type = _type;
            line = l;
            charac = c;
        }
        public override string ToString()
        {
            if (value == "")
            {
                return type.ToString();
            }
            else
            {
                return type.ToString() + " -> " + value;
            }
        }
    }
}
