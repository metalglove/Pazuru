using Pazuru.Domain;
using System.Text;

namespace Pazuru.Hitori
{
    public sealed class HitoriPrinter : PuzzlePrinter<HitoriPuzzle>
    {
        public override string Print(HitoriPuzzle puzzle)
        {
            StringBuilder ds = new StringBuilder();
            ds.AppendLine("Attempt:");
            string puzzleAsString = puzzle.PuzzleState.ToString();
            for (int i = 0; i < puzzleAsString.Length; i += puzzle.Size * 2)
            {
                ds.AppendLine(puzzleAsString.Substring(i, puzzle.Size * 2));
            }
            ds.AppendLine("Actual:");
            ds.AppendLine("3W4W2B2W4B");
            ds.AppendLine("2B3W2W4W5W");
            ds.AppendLine("2W5B4W5W2B");
            ds.AppendLine("4W5W1W5B2W");
            ds.AppendLine("5W3B3W1W4W");
            return ds.ToString();
        }
    }
}
