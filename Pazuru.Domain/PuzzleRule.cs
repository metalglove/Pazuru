namespace Pazuru.Domain
{
    public abstract class PuzzleRule<TPuzzle> : IPuzzleRule where TPuzzle : Puzzle
    {
        protected TPuzzle Puzzle { get; }

        public PuzzleRule(TPuzzle puzzle)
        {
            Puzzle = puzzle;
        }

        public abstract bool IsValid(int row, int column, int number);
    }

    public interface IPuzzleRule
    {
        bool IsValid(int row, int column, int number);
    }
}
