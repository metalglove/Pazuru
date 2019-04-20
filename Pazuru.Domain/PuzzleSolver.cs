using Pazuru.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Pazuru.Domain
{
    public abstract class PuzzleSolver<TPuzzle> : IDisposable where TPuzzle : Puzzle
    {
        private readonly LinkedList<PuzzleState> _puzzleStates = new LinkedList<PuzzleState>();
        private TPuzzle Puzzle { get; }
        protected bool IsSolved { get; set; }
        protected int Length => Puzzle.Length;
        protected int this[int row, int column]
        {
            get => Puzzle[row, column];
            set
            {
                Puzzle[row, column] = value;
                _puzzleStates.AddLast(CopyPuzzleState());
            }
        }
        public IReadOnlyCollection<PuzzleState> PuzzleStates => _puzzleStates;

        protected PuzzleSolver(TPuzzle puzzle)
        {
            Puzzle = puzzle;
            PuzzleState puzzleState = puzzle.PuzzleState;
            _puzzleStates.AddFirst(puzzleState);
        }

        public PuzzleState GetSolvedState()
        {
            if (!IsSolved)
                throw new PuzzleIsNotYetSolvedException();
            return _puzzleStates.Last.Value;
        }
        public abstract bool Solve();
        protected bool IsAssignmentValid(int row, int column, int number)
        {
            foreach (IPuzzleRule item in Puzzle.PuzzleRules)
                if (!item.IsValid(row, column, number))
                    return false;
            return true;
        }
        private PuzzleState CopyPuzzleState()
        {
            byte[] bytes = new byte[Puzzle.PuzzleState.Value.Length];
            Buffer.BlockCopy(Puzzle.PuzzleState.Value, 0, bytes, 0, Puzzle.PuzzleState.Value.Length);
            return new PuzzleState(bytes);
        }

        public void Dispose()
        {
            _puzzleStates.Clear();
            Puzzle.Dispose();
        }
    }
}
