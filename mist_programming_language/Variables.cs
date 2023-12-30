namespace mist_programming_language
{
    public class Variables
    {
        static Dictionary<string, dynamic> variables = new Dictionary<string, dynamic>();
       
        public static void SetVariable(string name, dynamic value)
        {
            variables[name] = value;
        }
        public static dynamic GetVariable(string name) {
            try
            {
                return variables[name];
            }
            catch (KeyNotFoundException e)
            {
                throw new MistException(-1, -1, "<stdin>", $"Undefined variable {name}");
            }
        }
    }
}
