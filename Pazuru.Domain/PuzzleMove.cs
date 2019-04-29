namespace Pazuru.Domain
{
    public abstract class PuzzleMove<TPuzzle> where TPuzzle : Puzzle
    {
        public bool IsValid { get; protected set; }

        public abstract bool Execute(TPuzzle puzzle);
        public abstract void Undo(TPuzzle puzzle);
    }
}
