using System;
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

        #region Utilities
        private bool IsPositionValid(int row, int column)
        {
            return row >= 0 &&
                   row < Puzzle.Size &&
                   column >= 0 &&
                   column < Puzzle.Size;
        }
        private IEnumerable<int> GetRow(int row)
        {
            for (int i = 0; i < Puzzle.Size; i++)
                yield return Puzzle[row, i];
        }
        private IEnumerable<int> GetColumn(int column)
        {
            for (int i = 0; i < Puzzle.Size; i++)
                yield return Puzzle[i, column];
        }

        private void IfPositionIsValidExecuteMoveAndAddToMovesList(int row, int column, HitoriMoveColorKey colorKey)
        {
            if (!IsPositionValid(row, column))
                return;
            HitoriMove hitoriMove = new HitoriMove(row, column, colorKey);
            Puzzle.ExecuteMove(hitoriMove);
            HitoriMoves.Add(hitoriMove);
        }
        #endregion Utilities

        #region Techniques
        /// <summary>
        /// The middle square of an adjacent triplet must always be Black.
        /// The first rule of Hitori says the same number cannot appear more than once
        /// in a row or a column so the outer adjacent numbers need to be White.
        /// </summary>
        private void SearchForAdjacentTriplets()
        {
            // NOTE: Is it possible for Hitori puzzles to have 4 or 5 numbers the same and adjacent in 1 column or row?...
            for (int i = 0; i < Puzzle.Size; i++)
            {
                for (int j = 0; j < Puzzle.Size - 3; j++)
                {
                    AdjacentTripletsTechnique(GetRow(i).Skip(j).Take(3), i, j);
                    AdjacentTripletsTechnique(GetColumn(i).Skip(j).Take(3), j, i);
                }
            }

            void AdjacentTripletsTechnique(IEnumerable<int> collection, int row, int column)
            {
                if (collection.Count() != 3)
                    throw new ArgumentOutOfRangeException(nameof(collection));
                if (collection.Distinct().Skip(1).Any())
                    return;
                // mark middle square Black
                int newColumn = column + 1;
                IfPositionIsValidExecuteMoveAndAddToMovesList(row, newColumn, HitoriMoveColorKey.Black);

                // mark connected squares White
                IfPositionIsValidExecuteMoveAndAddToMovesList(row + 1, newColumn, HitoriMoveColorKey.White);
                IfPositionIsValidExecuteMoveAndAddToMovesList(row - 1, newColumn, HitoriMoveColorKey.White);
                IfPositionIsValidExecuteMoveAndAddToMovesList(row, newColumn + 1, HitoriMoveColorKey.White);
                IfPositionIsValidExecuteMoveAndAddToMovesList(row, column, HitoriMoveColorKey.White);
            }
        }

        /// <summary>
        /// A square between a pair of same-numbers must be White.
        /// The second rule of Hitori says Black squares must not touch each other vertically or horizontally implying
        /// that a Black square is always surrounded by four White squares (or less, depending on whether it is located next to the edge or in a corner).
        /// </summary>
        private void SearchForSquareBetweenAPair()
        {
            for (int i = 0; i < Puzzle.Size; i++)
            {
                for (int j = 0; j < Puzzle.Size - 3; j++)
                {
                    SquareBetweenAPairTechnique(GetRow(i).Skip(j).Take(3), i, j, true);
                    SquareBetweenAPairTechnique(GetColumn(i).Skip(j).Take(3), j, i, false);
                }
            }

            void SquareBetweenAPairTechnique(IEnumerable<int> collection, int row, int column, bool horizontal)
            {
                int[] array = collection as int[] ?? collection.ToArray();
                if (array.Length != 3)
                    throw new ArgumentOutOfRangeException(nameof(collection));
                if (!array.First().Equals(array.Last()) || array.First() == array[1])
                    return;
                int newRow = horizontal ? row : row + 1;
                int newColumn = horizontal ? column + 1 : column;
                IfPositionIsValidExecuteMoveAndAddToMovesList(newRow, newColumn, HitoriMoveColorKey.White);
            }
        }

        /// <summary>
        /// Pair induction, where a row or column contains three same-numbers but only two of them are an adjacent pair.
        /// In row 4 of this example there are three 5s, two of which are an adjacent pair and one which is single.
        /// If the single 5 is un-shaded, then according to the first rule of Hitori the adjacent pair of 5s must be shaded.
        /// But this would violate the second rule which says shaded squares must not touch each other vertically or horizontally.
        /// Therefore the 5 in square b4 must be shaded.
        /// </summary>
        private void SearchForPairInduction()
        {
            for (int i = 0; i < Puzzle.Size; i++)
            {
                PairInductionTechnique(GetRow(i), i, true);
                PairInductionTechnique(GetColumn(i), i, false);
            }

            void PairInductionTechnique(IEnumerable<int> range, int indexer, bool horizontal)
            {
                int[] array = range as int[] ?? range.ToArray();
                if (array.Length != Puzzle.Size)
                    throw new ArgumentOutOfRangeException(nameof(range));

                IEnumerable<int> keys = array.GroupBy(num => num).Where(grouping => grouping.Count() == 3).Select(g => g.Key);
                if (!keys.Any())
                    return;
                
                foreach (int key in keys)
                {
                    List<int> numberIndexes = new List<int>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        int number = array[i];
                        if (number.Equals(key))
                        {
                            numberIndexes.Add(i);
                        }
                    }

                    if (numberIndexes.First() + 1 == numberIndexes[1])
                        IfPositionIsValidExecuteMoveAndAddToMovesList(
                            horizontal ? indexer : numberIndexes.Last(),
                            horizontal ? numberIndexes.Last() : indexer, 
                            HitoriMoveColorKey.Black);
                }
            }
        }
        #endregion Techniques
        

        public bool SolveV2()
        {
            // https://www.conceptispuzzles.com/index.aspx?uri=puzzle/hitori/techniques
            // 1.0 Starting techniques
            // 1.1 Searching for adjacent triplets
            SearchForAdjacentTriplets();
            // 1.2 Square between a pair
            SearchForSquareBetweenAPair();
            // 1.3 Pair induction
            SearchForPairInduction();

            // 2.0 Basic techniques
            // 2.1 Shading squares in rows and columns
            // 2.2 Un-shading around shaded squares
            // 2.3 Un-shading squares to avoid partitions

            // 3.0 Corner techniques
            // 3.1 Corner technique 1
            // 3.2 Corner technique 2

            // 4.0 Advanced techniques
            // 4.1 Advanced technique 1
            // 4.2 Advanced technique 2
            // 4.3 Advanced technique 3
            // 4.4 Advanced technique 4
            // 4.5 Advanced technique 5

            // 5.0 Remainder of squares
            // 5.1 Mark all remaining unique numbers White
            // 5.2 BFS against puzzle rules for the remaining squares?

            return true;
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
