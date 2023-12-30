namespace mist_programming_language
{
    internal class MistException : Exception
    {
        public int Line { get; }
        public int Character { get; }
        public string File { get; }

        public MistException(int line, int character, string file, string message) : base(message)
        {
            Line = line;
            Character = character;
            File = file;
        }
    }
}

