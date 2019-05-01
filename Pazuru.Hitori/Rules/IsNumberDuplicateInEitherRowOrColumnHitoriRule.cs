using Pazuru.Domain;

namespace Pazuru.Hitori.Rules
{
    public sealed class IsNumberDuplicateInEitherRowOrColumnHitoriRule : PuzzleRule<HitoriMove, HitoriPuzzle>
    {
        public IsNumberDuplicateInEitherRowOrColumnHitoriRule(HitoriPuzzle puzzle) : base(puzzle)
        {

        }

        public override bool IsValid(HitoriMove hitoriMove)
        {
            int number = Puzzle[hitoriMove.Row, hitoriMove.Column];
            for (int i = 0; i < Puzzle.Size; i++)
            {
                if (Puzzle[hitoriMove.Row, i].Equals(number) &&
                    hitoriMove.Column != i &&
                    Puzzle.GetChar(hitoriMove.Row, i) != (char)HitoriMoveColorKey.Black 
                    || 
                    Puzzle[i, hitoriMove.Column].Equals(number) &&
                    hitoriMove.Row != i &&
                    Puzzle.GetChar(i, hitoriMove.Column) != (char)HitoriMoveColorKey.Black)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
