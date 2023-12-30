using System.Linq.Expressions;

namespace mist_programming_language
{
    public abstract class AST
    {
        public abstract dynamic Evaluate();
    }
    public class ASTLeaf:AST
    {
        public readonly dynamic _num;
        public ASTLeaf(dynamic num)
        {
            _num = num;
        }
        public override dynamic Evaluate()
        {
            return _num;
        }
        public override string ToString()
        {
            return _num.ToString();
        }
    }
    public class ASTPlus : AST
    {
        public readonly AST _leftNode;
        public readonly AST _rightNode;

        public ASTPlus(AST leftNode, AST rightNode)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
        }
        public override dynamic Evaluate()
        {
            return _leftNode.Evaluate() + _rightNode.Evaluate();
        }
        public override string ToString()
        {
            return String.Format("({0} + {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
    public class ASTMinus : AST
    {
        public readonly AST _leftNode;
        public readonly AST _rightNode;

        public ASTMinus(AST leftNode, AST rightNode)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
        }
        public override dynamic Evaluate()
        {
            return this._leftNode.Evaluate() - this._rightNode.Evaluate();
        }
        public override string ToString()
        {
            return String.Format("({0} - {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
    public class ASTMultiply : AST
    {
        public readonly AST _leftNode;
        public readonly AST _rightNode;

        public ASTMultiply(AST leftNode, AST rightNode)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
        }
        public override dynamic Evaluate()
        {
            return _leftNode.Evaluate() * _rightNode.Evaluate();
        }

        public override string ToString()
        {
            return String.Format("({0} * {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
    public class ASTDivide : AST
    {
        public readonly AST _leftNode;
        public readonly AST _rightNode;

        public ASTDivide(AST leftNode, AST rightNode)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
        }
        public override dynamic Evaluate()
        {
            return _leftNode.Evaluate() / _rightNode.Evaluate();
        }
        public override string ToString()
        {
            return String.Format("({0} / {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }

   

    public class ASTFunctionCall : AST
    {
        private readonly string _functionName;
        private readonly List<AST> _arguments;

        public ASTFunctionCall(string functionName, List<AST> arguments)
        {
            _functionName = functionName;
            _arguments = arguments;
        }

        public override dynamic Evaluate()
        {
            if(_functionName == "out")
            {
                string result = "";
                foreach(AST arg in _arguments)
                {
                 //   Console.WriteLine(arg.GetType().ToString());
                    result += arg.Evaluate();
                }
                Console.Write(result);
                return result;
            }
            else if(_functionName == "outln")
            {
                string result = "";
                foreach (AST arg in _arguments)
                {
                    result += arg.Evaluate();
                }
                result += "\n";
                Console.Write(result);
                return result;
            }
            else if(_functionName == "clear")
            {
                Console.Clear();
                return "";
            }
            else if (_functionName == "in")
            {
                Console.Write(_arguments[0].Evaluate());
                string input = Console.ReadLine();
                return input;
            }
            else if (_functionName == "rand")
            {
                Random rand = new Random();
                return rand.Next((int)_arguments[0].Evaluate(),(int)_arguments[1].Evaluate());
            }
            else if (_functionName == "stoi")
            {
                int a = Convert.ToInt32(_arguments[0].Evaluate());
                return a;
            }
            else if (_functionName == "itos")
            {
                string a = Convert.ToString(_arguments[0].Evaluate());
                return a;
            }
            else if (_functionName == "halt")
            {
                Environment.Exit(1);
                return null;
            }
            else
            {
                
               throw new MistException(-1, -1, "<stdin>", "Function Doesn't exist: " + _functionName);
                return null;
            }
        }

        public override string ToString()
        {
            return $"{_functionName}({_arguments[0]})";
        }
    }
    public class ASTEqualEqual : AST
    {
        private readonly AST _leftNode,_rightNode;
        public ASTEqualEqual(AST LeftNode,AST RightNode)
        {
            _leftNode = LeftNode;
            _rightNode = RightNode;
        }
        public override dynamic Evaluate()
        {
            try
            {
                return _leftNode.Evaluate() == _rightNode.Evaluate();
            }
            catch
            {
                throw new MistException(-1, -1, "<stdin>", "Cannot Compare different data types");
            }
        }
    }

    public class ASTGreaterThan : AST
    {
        private readonly AST _leftNode, _rightNode;
        public ASTGreaterThan(AST LeftNode, AST RightNode)
        {
            _leftNode = LeftNode;
            _rightNode = RightNode;
        }
        public override dynamic Evaluate()
        {
            try
            {
                return _leftNode.Evaluate() > _rightNode.Evaluate();
            }
            catch
            {
                throw new MistException(-1, -1, "<stdin>", "Can compare only numerical variables!");
            }
        }
    }

    public class ASTLessThan : AST
    {
        private readonly AST _leftNode, _rightNode;
        public ASTLessThan(AST LeftNode, AST RightNode)
        {
            _leftNode = LeftNode;
            _rightNode = RightNode;
        }
        public override dynamic Evaluate()
        {
            try
            {
                return _leftNode.Evaluate() < _rightNode.Evaluate();
            }
            catch
            {
                throw new MistException(-1, -1, "<stdin>", "Can compare only numerical variables!");
            }
        }
    }

    public class ASTNotEqualEqual : AST
    {
        private readonly AST _leftNode, _rightNode;
        public ASTNotEqualEqual(AST LeftNode, AST RightNode)
        {
       
            _leftNode = LeftNode;
            _rightNode = RightNode;
        }
        public override dynamic Evaluate()
        {
          
            try
            {
                return _leftNode.Evaluate() != _rightNode.Evaluate();
            }
            catch
            {
                throw new MistException(-1, -1, "<stdin>", "Cannot Compare different data types");
            }
        }
    }

    public class ASTWhileStatement : AST
    {
        private readonly AST _condition;
        private readonly AST _whileBlock;

        public ASTWhileStatement(AST condition, AST ifBlock)
        {
            _condition = condition;
            _whileBlock = ifBlock;
        }

        public override dynamic Evaluate()
        {
            // Evaluate the condition and execute the block if true
            while ((bool)_condition.Evaluate())
            {
                return _whileBlock.Evaluate();
            }
            return null; // Return null if the condition is false
        }
    }

    public class ASTIfStatement : AST
    {
        private readonly AST _condition;
        private readonly List<AST> _ifBlock = new List<AST>();

        public ASTIfStatement(AST condition, List<AST> ifBlock)
        {
            _condition = condition;
            _ifBlock = ifBlock;
        }

        public override dynamic Evaluate()
        {
           
            if ((bool)_condition.Evaluate())
            {
                foreach(AST block in _ifBlock)
                {
                    block.Evaluate();
                }
                return true;
            }
            return null;
        }
    }


    public class ASTVariableDeclaration : AST
    {
        private readonly string _variableName;
        private readonly AST _expression;

        public ASTVariableDeclaration(string variableName, AST expression)
        {
            _variableName = variableName;
            _expression = expression;
        }
        public override dynamic Evaluate()
        {
            dynamic assignedValue = _expression.Evaluate();
            Variables.SetVariable(_variableName, assignedValue);
            return assignedValue;
        }

        public override string ToString()
        {
            return $"decl {_variableName} = {_expression}";
        }
    }
    public class ASTVariableReference : AST
    {
        public readonly string _variableName;

        public ASTVariableReference(string variableName)
        {
            _variableName = variableName;
        }

        public override dynamic Evaluate()
        {
            return Variables.GetVariable(_variableName);
        }

        public override string ToString()
        {
            return _variableName;
        }
    }
    public class ASTStringConcatenation : AST
    {
        private readonly AST _leftNode;
        private readonly AST _rightNode;

        public ASTStringConcatenation(AST leftNode, AST rightNode)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
        }

        public override dynamic Evaluate()
        {
            return _leftNode.Evaluate().ToString() + _rightNode.Evaluate().ToString();
        }

        public override string ToString()
        {
            return $"{_leftNode} + {_rightNode}";
        }
    }

}
