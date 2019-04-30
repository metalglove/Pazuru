using Pazuru.Domain;
using System;

namespace Pazuru.Hitori
{
    public sealed class HitoriSolver : PuzzleSolver<HitoriPuzzle>
    {
        public HitoriSolver(HitoriPuzzle puzzle) : base(puzzle)
        {

        }

        public override bool Solve()
        {
            // No number appears in a row or column more than once.
            // Shaded(black) squares do not touch each other vertically or horizontally.
            // When completed, all un - shaded(white) squares create a single continuous area.

            // TODO: BFS

            throw new NotImplementedException();
        }
    }
}
