namespace Pazuru.Domain
{
    public abstract class PuzzleRule<TPuzzleType> : IPuzzleRule where TPuzzleType : Puzzle
    {
        protected TPuzzleType Puzzle { get; }

        public PuzzleRule(TPuzzleType puzzle)
        {
            Puzzle = puzzle;
        }

        public abstract bool IsValid<TPuzzle>(PuzzleMove<TPuzzle> puzzleMove) where TPuzzle : Puzzle;
    }

    public interface IPuzzleRule
    {
        bool IsValid<TPuzzle>(PuzzleMove<TPuzzle> puzzleMove) where TPuzzle : Puzzle;
    }
}
