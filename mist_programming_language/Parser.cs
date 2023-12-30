using System.Diagnostics;
using TokenType = mist_programming_language.Tokens.TokenType;

namespace mist_programming_language
{
    public class Parser
    {
        private readonly List<TokenType> TermItems = new List<TokenType> { TokenType.PLUS, TokenType.MINUS,TokenType.EQEQ,TokenType.NEQEQ,TokenType.GT,TokenType.LT};
        private readonly List<TokenType> FactorItems = new List<TokenType> { TokenType.MUL, TokenType.DIV };
        private readonly List<Token> _tokens;
        private int pos = 0;
        private Token curr_token;

        public Parser(List<Token> tokens)
        {
            this._tokens = tokens;
            curr_token = this._tokens[0];
            Get_Next();
        }

        private void Get_Next()
        {
            if (pos < this._tokens.Count)
            {
                curr_token = this._tokens[pos];
                pos++;
            }
        }

        public AST Factor()
        {
            AST factor = Term();
            while (curr_token.type != TokenType.EOF && factor != null && FactorItems.Contains(curr_token.type))
            {
                if (curr_token.type == TokenType.MUL)
                {
                    Get_Next();
                    AST rightNode = Term();
                    factor = new ASTMultiply(factor, rightNode);
                }
                else if (curr_token.type == TokenType.DIV)
                {
                    Get_Next();
                    AST rightNode = Term();
                    factor = new ASTDivide(factor, rightNode);
                }
            }
            return factor;
        }

        public AST Term()
        {
            AST term = null;

            if (curr_token.type == TokenType.LBRACE)
            {
                Get_Next();
                term = ParseExp();
                /*if (curr_token.type != TokenType.RBRACE)
                {
                    throw new FormatException("Missing )");
                }*/
            }
            else if (curr_token.type == TokenType.NUMBER)
            {
                term = new ASTLeaf((decimal)Convert.ToDouble(curr_token.value));
            }
            else if (curr_token.type == TokenType.STRING)
            {
                term = new ASTLeaf(curr_token.value.ToString());
            }
            else if (curr_token.type == TokenType.ID)
            {
                dynamic id = curr_token.value;
                if (id == "if")
                {
                    // IF statement
                    Get_Next(); // Move past IF
                    AST condition;
                    if (curr_token.type == TokenType.LPAREN)
                    {
                        Get_Next(); // Move past LPAREN
                        condition = ParseExp(); // Parse the condition
                       // Console.WriteLine($"condition result: {condition.Evaluate()}");
                        // Ensure proper matching of parentheses
                       /* if (curr_token.type != TokenType.RPAREN)
                        {
                            throw new MistException(-1, -1,"<stdin>",$"Unexpected token `{curr_token.type}`");
                        }*/
                        Get_Next();

                        List<AST> blocks = new List<AST>();
                        while(curr_token.type != TokenType.RBRACE)
                        {
                            AST ifBlock = ParseExp();
                            if (ifBlock is null) break;
                            blocks.Add(ifBlock);
                        }
                        term = new ASTIfStatement(condition, blocks);

                    }
                    else
                    {
                        throw new MistException(-1, -1, "<stdin>", "Missing opening parenthesis in if statement");
                    }
                }
                else if(id == "while")
                {
                    // WHILE statement
                    Get_Next(); // Move past WHILE
                    AST condition;
                    if (curr_token.type == TokenType.LPAREN)
                    {
                        Get_Next(); // Move past LPAREN
                        condition = ParseExp(); // Parse the condition
                                                // Console.WriteLine($"condition result: {condition.Evaluate()}");
                                                // Ensure proper matching of parentheses
                        /* if (curr_token.type != TokenType.RPAREN)
                         {
                             throw new MistException(-1, -1,"<stdin>",$"Unexpected token `{curr_token.type}`");
                         }*/
                        Get_Next();
                        AST ifBlock = ParseExp();
                        term = new ASTWhileStatement(condition, ifBlock);
                    }
                    else
                    {
                        throw new MistException(-1, -1, "<stdin>", "Missing opening parenthesis in if statement");
                    }
                }
                else
                {
                    Get_Next();

                    if (curr_token.type == TokenType.LPAREN)
                    {
                        // Function call
                        Get_Next(); // Move past LPAREN
                        List<AST> args = new List<AST>();
                        while (curr_token.type != TokenType.RPAREN)
                        {
                            args.Add(ParseExp());
                            if (curr_token.type == TokenType.COMMA)
                                Get_Next(); // Move past COMMA
                        }
                        Get_Next(); // Move past RPAREN
                        term = new ASTFunctionCall(id, args);
                    }
                    else if (curr_token.type == TokenType.ASSIGN)
                    {
                        // Variable declaration
                        Get_Next();
                        AST expr = ParseExp();
                        term = new ASTVariableDeclaration(id, expr);
                    }
                    else
                    {
                        // Variable reference or string concatenation
                        term = new ASTVariableReference(id);
                    }
                }
            }
             


                if (curr_token.type == TokenType.PLUS && term != null && term is ASTVariableReference)
                {
                    if (term.Evaluate() is String)
                    {
                        Get_Next();
                        AST rightNode = Term();
                        term = new ASTStringConcatenation(term, rightNode);
                    }
                    else
                    {
                        Get_Next();
                        AST rightNode = Term();
                        term = new ASTPlus(term, rightNode);
                    }
                }
                else if (curr_token.type == TokenType.EQEQ && term != null && term is ASTVariableReference)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    term = new ASTEqualEqual(term, rightNode);
                }
                else if (curr_token.type == TokenType.NEQEQ && term != null && term is ASTVariableReference) 
                {
                  
                    Get_Next();
                    AST rightNode = Factor();
                    term = new ASTNotEqualEqual(term, rightNode);
                }
                else if (curr_token.type == TokenType.GT && term != null && term is ASTVariableReference)
                {

                    Get_Next();
                    AST rightNode = Factor();
                    term = new ASTGreaterThan(term, rightNode);
                }
                else if (curr_token.type == TokenType.LT && term != null && term is ASTVariableReference)
                {

                    Get_Next();
                    AST rightNode = Factor();
                    term = new ASTLessThan(term, rightNode);
                }




            Get_Next();
            return term;
        }

        public AST ParseExp()
        {
            AST result = Factor();
            while (curr_token.type != TokenType.EOF && result != null && TermItems.Contains(curr_token.type))
            {
                if (curr_token.type == TokenType.PLUS)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    result = new ASTPlus(result, rightNode);
                }
                else if (curr_token.type == TokenType.MINUS)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    result = new ASTMinus(result, rightNode);
                }
                else if (curr_token.type == TokenType.EQEQ)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    result = new ASTEqualEqual(result, rightNode);
                }
                else if (curr_token.type == TokenType.NEQEQ)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    result = new ASTNotEqualEqual(result, rightNode);
                }
                else if (curr_token.type == TokenType.GT)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    result = new ASTGreaterThan(result, rightNode);
                }
                else if (curr_token.type == TokenType.LT)
                {
                    Get_Next();
                    AST rightNode = Factor();
                    result = new ASTLessThan(result, rightNode);
                }
            }

            return result;
        }
    }
}
