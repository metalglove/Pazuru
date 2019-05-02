using System.Collections.Generic;
using System.Linq;
using Pazuru.Domain;

namespace Pazuru.Hitori
{
    public sealed class HitoriSolver : PuzzleSolver<HitoriPuzzle>
    {
        public readonly List<HitoriMove> HitoriMoves = new List<HitoriMove>();

        public HitoriSolver(HitoriPuzzle puzzle) : base(puzzle)
        {

        }

        public override bool Solve()
        {
            //1. In the current state, check if every square is either “W” or “B”. If so, we
            //   have solved the puzzle. Success!
            IsSolved = IsAlreadySolved() || RecursiveSolve();
            return IsSolved;
        }

        private bool RecursiveSolve(HitoriMoveColorKey strategyColorKey = HitoriMoveColorKey.White)
        {
            while (true)
            {
                //2. Choose an “N” square.
                if (!TryFindCell(out int row, out int column, HitoriMoveColorKey.None))
                    return true;

                //3. Set as our strategy to set the target state to “W”.
                //HitoriMoveColorKey colorKeyStrategy = HitoriMoveColorKey.White;

                //4. Flip the selected square to the target state. Then follow-through,
                //   flipping other squares to “B” or “W” as necessitated by flipping the selected square.
                //   (More on follow through below.) If any of these flips violates the rules and the
                //   target state is “W”, set target state to “B” and go back to 4.
                //   If the target state was already “B”, we have failed to solve the board from this state.
                HitoriMove hitoriMove = new HitoriMove(row, column, strategyColorKey);
                bool isExecutedSuccessfully = hitoriMove.Execute(Puzzle);
                
                if (!isExecutedSuccessfully)
                {
                    //hitoriMove.Undo(Puzzle);
                    //_hitoriMoves.Remove(hitoriMove);
                    //AddPuzzleState();
                    if (strategyColorKey == HitoriMoveColorKey.White)
                        strategyColorKey = HitoriMoveColorKey.Black;
                    else
                    {
                        while (true)
                        {
                            HitoriMove hitoriMoveLast = HitoriMoves.LastOrDefault();
                            if (hitoriMoveLast == default)
                                return false;
                            hitoriMoveLast.Undo(Puzzle);
                            AddPuzzleState();
                            HitoriMoves.Remove(hitoriMoveLast);
                            if (hitoriMoveLast.HitoriMoveColorKey != HitoriMoveColorKey.White)
                                continue;
                            strategyColorKey = HitoriMoveColorKey.Black;
                            break;
                        }
                        
                        //return false;
                    }

                }
                else
                {
                    strategyColorKey = HitoriMoveColorKey.White;
                    AddPuzzleState();
                    HitoriMoves.Add(hitoriMove);
                }
                //// Follow through
                //if (hitoriMove.HitoriMoveColorKey == HitoriMoveColorKey.White)
                //{

                //}

                //5. Check if these flips have created an island. If so and the target state is “W”, set
                //   target state to “B” and go back to 4. If the target state was already “B”, we have failed
                //   to solve the board from this state.

                //6. Recursively solve the board from the board’s state that results from the flips in
                //   step 4. If this is successful, we are done! If not and the target state is “W”,
                //   set the target state to “B” and go back to 4. Otherwise, we have failed
                //   to solve the board from this state.
            }
        }

        private bool TryFindCell(out int row, out int column, HitoriMoveColorKey colorKey)
        {
            for (int i = 0; i < Puzzle.Size; i++)
            {
                for (int j = 0; j < Puzzle.Size; j++)
                {
                    if (Puzzle.GetColorKey(i, j) != colorKey)
                        continue;
                    row = i;
                    column = j;
                    return true;
                }
            }
            row = 0;
            column = 0;
            return false;
        }
        private bool IsAlreadySolved()
        {
            for (int i = 0; i < Puzzle.Size; i++)
            {
                for (int j = 0; j < Puzzle.Size; j++)
                {
                    HitoriMoveColorKey colorKey = Puzzle.GetColorKey(i, j);
                    if (colorKey != HitoriMoveColorKey.Black && colorKey != HitoriMoveColorKey.White)
                        return false;
                }
            }
            return true;
        }
    }
}
