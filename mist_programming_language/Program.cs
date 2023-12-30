using mist_programming_language;

const string version = "0.2";

Console.WriteLine("MIST Programming Language ver." + version);
Console.WriteLine("Created by Vardan Petrosyan (github/Vardan2009)");

void ExecuteCode(string l)
{
    try
    {
        Lexer lex = new Lexer(l);
        lex.Tokenize();
       /* foreach(Token tok in lex.tokens)
        {
            Console.WriteLine(tok.ToString());
        }*/
        Parser parser = new Parser(lex.tokens);
        while(true)
        {
            AST astObj = parser.ParseExp();
            if (astObj is null) break;
            astObj.Evaluate();
        }
        Console.WriteLine();

    }
    catch (MistException e)
    {
        string f = $"At file {e.File} on line {e.Line},{e.Character}: {e.Message}";
        if (e.Line == -1)
        {
            f = $"At file {e.File}: {e.Message}";
        }
        Console.WriteLine("[MIST] " + f);
    }
}


void Shell()
{
    while (true)
    {
        Console.Write("Mist " + version + " >> ");
        string l = Console.ReadLine();
        ExecuteCode(l);
    }
}

Shell();
