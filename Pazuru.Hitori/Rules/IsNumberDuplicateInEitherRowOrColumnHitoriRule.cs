using Pazuru.Domain;

namespace Pazuru.Hitori.Rules
{
    public sealed class IsNumberDuplicateInEitherRowOrColumnHitoriRule : PuzzleRule<HitoriMove, HitoriPuzzle>
    {
        public IsNumberDuplicateInEitherRowOrColumnHitoriRule(HitoriPuzzle puzzle) : base(puzzle)
        {
            // NOTE: also checks if the duplicate number has already been set to HitoriMoveColorKey.Black    
        }

        public override bool IsValid(HitoriMove hitoriMove)
        {
            HitoriCell currentCell = new HitoriCell(Puzzle, hitoriMove.Row, hitoriMove.Column);
            for (int i = 0; i < Puzzle.Size; i++)
            {
                if (Puzzle[hitoriMove.Row, i].Equals(currentCell.Number) &&
                    hitoriMove.Column != i /*&&
                    Puzzle.GetColorKey(hitoriMove.Row, i) != HitoriMoveColorKey.Black*/ 
                    || 
                    Puzzle[i, hitoriMove.Column].Equals(currentCell.Number) &&
                    hitoriMove.Row != i /*&&
                    Puzzle.GetColorKey(i, hitoriMove.Column) != HitoriMoveColorKey.Black*/)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
