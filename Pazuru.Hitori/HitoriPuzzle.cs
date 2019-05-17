using Pazuru.Domain;
using Pazuru.Hitori.Rules;

namespace Pazuru.Hitori
{
    public sealed class HitoriPuzzle : Puzzle<HitoriMove, HitoriPuzzle>
    {
        public override string Description => 
            "Hitori is a number-elimination puzzle based on a square grid filled with numbers. " +
            "The object is to shade squares so that no number appears in a row or column more than once. " +
            "In addition, shaded (black) squares do not touch each other vertically or horizontally and all " +
            "un-shaded (white) squares create a single continuous area when the puzzle is completed.";
        public override int Size { get; }
        public override int this[int row, int column]
        {
            get => base[row * 2, column * 2];
            protected set => base[row * 2, column * 2] = value;
        }

        public HitoriPuzzle(PuzzleState puzzleState, int size = 9) : base(puzzleState)
        {
            Size = size;
            //AddRule(new IsNumberDuplicateInEitherRowOrColumnHitoriRule(this));
            //AddRule(new AreWhiteSquaresConnectedByEitherHorizontalOrVerticalAndBlackSquaresNotHitoriRule(this));
        }

        public override bool ExecuteMove(HitoriMove hitoriMove)
        {
            if (!hitoriMove.IsValid)
                return false;
            SetChar(hitoriMove.Row, hitoriMove.Column, hitoriMove.HitoriMoveColorKey);
            return true;
        }
        public override bool UndoMove(HitoriMove hitoriMove)
        {
            if (!hitoriMove.IsValid)
                return false;
            SetChar(hitoriMove.Row, hitoriMove.Column, hitoriMove.HitoriMoveColorKeyBefore);
            return true;
        }
        public HitoriMoveColorKey GetColorKey(int row, int column)
        {
            return (HitoriMoveColorKey)(char)(base[row * 2, (column * 2) + 1] + 48);
        }
        private void SetChar(int row, int column, HitoriMoveColorKey colorKey)
        {
            base[row * 2, (column * 2) + 1] = (char)colorKey - 48;
        }
    }
}
