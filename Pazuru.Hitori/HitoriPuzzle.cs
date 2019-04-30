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

        public HitoriPuzzle(PuzzleState puzzleState, int length = 9) : base(puzzleState)
        {
            Size = length;
            AddRule(new IsNumberDuplicateInEitherRowOrColumnHitoriRule(this));
        }

        public override bool ExecuteMove(HitoriMove hitoriMove)
        {
            throw new System.NotImplementedException();
        }
        public override bool UndoMove(HitoriMove hitoriMove)
        {
            throw new System.NotImplementedException();
        }
        public char GetChar(int row, int column)
        {
            return (char)(base[row * 2, (column * 2) + 1] + 48);
        }
        public void SetChar(int row, int column, HitoriMoveColourKey colourKey)
        {
            base[row * 2, (column * 2) + 1] = (char)colourKey - 48;
        }
    }
}
