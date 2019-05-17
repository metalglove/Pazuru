using Pazuru.Domain;
using System.Text;

namespace Pazuru.Hitori
{
    public sealed class HitoriGenerator : PuzzleGenerator<HitoriPuzzle>
    {
        public override HitoriPuzzle Generate()
        {
            byte[] grid = Encoding.Default.GetBytes(
                "2N7N1N5N4N7N2N5N9N" +
                "2N8N5N6N3N4N9N5N3N" +
                "4N4N8N2N9N5N5N7N2N" +
                "6N9N9N7N7N1N3N1N5N" +
                "9N9N5N8N6N7N7N3N2N" +
                "5N5N9N8N8N9N6N4N4N" +
                "5N4N2N1N7N9N1N2N6N" +
                "8N7N2N4N3N5N7N1N4N" +
                "8N2N6N4N5N4N1N2N8N");

            byte[] grid5 = Encoding.Default.GetBytes(
                "2N2N1N5N3N" +
                  "2N3N1N4N5N" +
                  "1N1N1N3N5N" +
                  "1N3N5N4N2N" + 
                  "5N4N3N2N1N");

            byte[] gri2d5 = Encoding.Default.GetBytes(
                  "3N4N2N2N4N" +
                  "2N3N2N4N5N" +
                  "2N5N4N5N2N" +
                  "4N5N1N5N2N" +
                  "5N3N3N1N4N");
            //271547259285634953448295572699771315995867732559889644542179126872435714826454128
            //BWWWWBWBWWWBWBWWWWWBWWWBWWBWWBWBWWBWWBWWWWBWWBWWBWBWWBWWBWWWBWWWBWBWWWWWBWWWWBWBW
            PuzzleState puzzleState = new PuzzleState(gri2d5);
            HitoriPuzzle hitoriPuzzle = new HitoriPuzzle(puzzleState, 5);
            return hitoriPuzzle;
        }
    }
}
