namespace Pazuru.Domain
{
    public abstract class PuzzleRule<TPuzzleMove, TPuzzle> where TPuzzle : Puzzle where TPuzzleMove : PuzzleMove<TPuzzle>
    {
        protected TPuzzle Puzzle { get; }

        public PuzzleRule(TPuzzle puzzle)
        {
            Puzzle = puzzle;
        }

        public abstract bool IsValid(TPuzzleMove puzzleMove);
    }
}
