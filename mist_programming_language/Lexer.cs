using TokenType = mist_programming_language.Tokens.TokenType;

namespace mist_programming_language
{
    internal class Lexer
    {
        int index;
        char cur_char;
        string src;
        public List<Token> tokens;
        int curline = 1,curchar = 1;
        public Lexer(string _src)
        {
            index = 0;
            src = _src;
            if(src.Length == 0)
            {
                curchar = ' ';
            }
            else
            {
                cur_char = src[index];
            }
            tokens = new List<Token>();
        }

        public void Tokenize()
        {
            while (index < src.Length)
            {
                Token next = GetNextToken();
                tokens.Add(next);
                if (next.type == TokenType.EOF)
                {
                    break;
                }
            }
        }

        Token CollectID()
        {
            string value = "";
            while (index < src.Length && Char.IsLetter(cur_char))
            {
                value += cur_char;
                Advance();
            }
            Token finalToken = new Token(TokenType.ID, curline, curchar);
            finalToken.value = value;
            return finalToken;
        }

        Token CollectNumber()
        {
            string value = "";
            while (index < src.Length && Char.IsNumber(cur_char))
            {
                value += cur_char;
                Advance();
            }
            Token finalToken = new Token(TokenType.NUMBER, curline, curchar);
            finalToken.value = value;
            return finalToken;
        }

        void SkipComment()
        {
            Advance();
            while (index < src.Length && cur_char != '\n')
            {
                Advance();
            }
        }

        Token CollectString()
        {
            Advance(); // Skip the "
            string value = "";
            while (index < src.Length && cur_char != '"')
            {
                value += cur_char;
                Advance();
            }
            Advance(); // Skip the "
            Token finalToken = new Token(TokenType.STRING, curline, curchar);
            finalToken.value = value;
            return finalToken;
        }

        Token GetNextToken()
        {
            while (index < src.Length)
            {
                if (cur_char == ' ' || cur_char == '\n' || cur_char == '\t' || cur_char == '\r')
                {
                    SkipWhitespace();
                }
                else if(cur_char == ':')
                {
                    SkipComment();
                }
                else if (Char.IsNumber(cur_char))
                {
                    return CollectNumber();
                }
                else if (Char.IsLetter(cur_char))
                {
                    return CollectID();
                }
                else if (cur_char == '"')
                {
                    return CollectString();
                }
                else
                {
                    switch (cur_char)
                    {
                        case '=':
                            Advance();
                            if(cur_char == '=')
                            {
                                Advance();
                                return new Token(TokenType.EQEQ, curline, curchar);
                            }
                            else
                            {
                                return new Token(TokenType.ASSIGN, curline, curchar);
                            }
                        case '!':
                            Advance();
                            if (cur_char == '=')
                            {
                                Advance();
                                return new Token(TokenType.NEQEQ, curline, curchar);
                            }
                            else
                            {
                                return new Token(TokenType.NOT, curline, curchar);
                            }
                        case ';':
                            Advance();
                            return new Token(TokenType.SEMI, curline, curchar);
                        case '>':
                            Advance();
                            return new Token(TokenType.GT, curline, curchar);
                        case '<':
                            Advance();
                            return new Token(TokenType.LT, curline, curchar);
                        case '(':
                            Advance();
                            return new Token(TokenType.LPAREN, curline, curchar);
                        case ')':
                            Advance();
                            return new Token(TokenType.RPAREN, curline, curchar);
                        case '+':
                            Advance();
                            return new Token(TokenType.PLUS, curline, curchar);
                        case '-':
                            Advance();
                            return new Token(TokenType.MINUS, curline, curchar);
                        case ',':
                            Advance();
                            return new Token(TokenType.COMMA, curline, curchar);
                        case '*':
                            Advance();
                            return new Token(TokenType.MUL, curline, curchar);
                        case '/':
                            Advance();
                            return new Token(TokenType.DIV, curline, curchar);
                        case '{':
                            Advance();
                            return new Token(TokenType.LBRACE, curline, curchar);
                        case '}':
                            Advance();
                            return new Token(TokenType.RBRACE, curline, curchar);
                        default:
                            throw new MistException(curline,curchar,"<stdin>",$"Unknown Character `{cur_char}`");
                    }
                }
            }
            return new Token(TokenType.EOF, curline, curchar);
        }

        void SkipWhitespace()
        {
            while (index < src.Length && (cur_char == ' ' || cur_char == '\n' || cur_char == '\t' || cur_char == '\r'))
            {
                if(cur_char == '\n')
                {
                    curline++;
                    curchar = 0;
                }
                Advance();
            }
        }

        void Advance()
        {
            index++;
            curchar++;
            if (index < src.Length)
            {
                cur_char = src[index];
            }
        }
    }
}
