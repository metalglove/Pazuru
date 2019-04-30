using Pazuru.Domain;
using System.Text;

namespace Pazuru.Hitori
{
    public sealed class HitoriGenerator : PuzzleGenerator<HitoriPuzzle>
    {
        public override HitoriPuzzle Generate()
        {
            byte[] grid = Encoding.Default.GetBytes(
                "2G7G1G5G4G7G2G5G9G" +
                "2G8G5G6G3G4G9G5G3G" +
                "4G4G8G2G9G5G5G7G2G" +
                "6G9G9G7G7G1G3G1G5G" +
                "9G9G5G8G6G7G7G3G2G" +
                "5G5G9G8G8G9G6G4G4G" +
                "5G4G2G1G7G9G1G2G6G" +
                "8G7G2G4G3G5G7G1G4G" +
                "8G2G6G4G5G4G1G2G8G");
            //271547259285634953448295572699771315995867732559889644542179126872435714826454128
            //BWWWWBWBWWWBWBWWWWWBWWWBWWBWWBWBWWBWWBWWWWBWWBWWBWBWWBWWBWWWBWWWBWBWWWWWBWWWWBWBW
            PuzzleState puzzleState = new PuzzleState(grid);
            HitoriPuzzle hitoriPuzzle = new HitoriPuzzle(puzzleState, 9);
            return hitoriPuzzle;
        }
    }
}
