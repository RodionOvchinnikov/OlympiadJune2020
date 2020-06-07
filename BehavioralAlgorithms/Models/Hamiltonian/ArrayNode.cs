namespace BehavioralAlgorithms.Models.Hamiltonian
{
    public class ArrayNode
    {
        public int X { get; set; }

        public int Y { get; set; }

        public ArrayNode Left { get; set; }

        public ArrayNode Top { get; set; }

        public ArrayNode Right { get; set; }

        public ArrayNode Bottom { get; set; }
        // Если true, то шагнуть в этот блок невозможно
        public bool Blocked { get; set; }

        public override string ToString()
        {
            return $"x = {X} ; y = {Y}";
        }
    }
}
