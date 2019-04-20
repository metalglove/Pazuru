namespace Pazuru.Domain
{
    public abstract class PuzzleGenerator<TPuzzle> where TPuzzle : Puzzle
    {
        public abstract TPuzzle Generate();
    }
}
