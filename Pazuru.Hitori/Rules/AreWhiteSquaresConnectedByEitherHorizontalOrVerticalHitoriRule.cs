using Pazuru.Domain;

namespace Pazuru.Hitori.Rules
{
    public sealed class AreWhiteSquaresConnectedByEitherHorizontalOrVerticalAndBlackSquaresNotHitoriRule : PuzzleRule<HitoriMove, HitoriPuzzle>
    {
        public AreWhiteSquaresConnectedByEitherHorizontalOrVerticalAndBlackSquaresNotHitoriRule(HitoriPuzzle puzzle) : base(puzzle)
        {

        }

        public override bool IsValid(HitoriMove puzzleMove)
        {
            for (int i = 0; i < Puzzle.Size; i++)
            {
                for (int j = 0; j < Puzzle.Size; j++)
                {
                    HitoriCell currentCell = new HitoriCell(Puzzle, i, j);
                    if (currentCell.ColorKey == HitoriMoveColorKey.White &&
                        !currentCell.IsConnectedByEitherHorizontalOrVertical(HitoriMoveColorKey.White)
                        ||
                        currentCell.ColorKey == HitoriMoveColorKey.Black &&
                        currentCell.IsConnectedByEitherHorizontalOrVertical(HitoriMoveColorKey.Black))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
