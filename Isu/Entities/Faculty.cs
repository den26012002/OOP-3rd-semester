namespace Isu.Entities
{
    public class Faculty
    {
        public Faculty(char letter, string name)
        {
            Letter = letter;
            Name = name;
        }

        public char Letter { get; }
        public string Name { get; }
    }
}
