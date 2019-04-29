using Pazuru.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Pazuru.Domain
{
    public abstract class PuzzleSolver<TPuzzle> : IDisposable where TPuzzle : Puzzle
    {
        private readonly LinkedList<PuzzleState> _puzzleStates = new LinkedList<PuzzleState>();
        protected TPuzzle Puzzle { get; }
        protected bool IsSolved { get; set; }
        public IReadOnlyCollection<PuzzleState> PuzzleStates => _puzzleStates;

        protected PuzzleSolver(TPuzzle puzzle)
        {
            Puzzle = puzzle;
            _puzzleStates.AddFirst(Puzzle.CopyPuzzleState());
        }

        protected void AddPuzzleState()
        {
            _puzzleStates.AddLast(Puzzle.CopyPuzzleState());
        }
        public PuzzleState GetSolvedState()
        {
            if (!IsSolved)
                throw new PuzzleIsNotYetSolvedException();
            return _puzzleStates.Last.Value;
        }
        public abstract bool Solve();
      
        public void Dispose()
        {
            _puzzleStates.Clear();
            Puzzle.Dispose();
        }
    }
}
