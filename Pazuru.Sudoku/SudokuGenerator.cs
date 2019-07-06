using System;
using System.Collections.Generic;
using System.Linq;
using DlxLib;
using Pazuru.Domain;

namespace Pazuru.Sudoku
{
    // TODO: Refactor to use the SudokuPuzzle class instead of copying the puzzle rule code.
    // TODO: Create my own DancingLinks implementation.
    public sealed class SudokuGenerator : PuzzleGenerator<SudokuPuzzle>
    {
        private const int SudokuSize = 9;
        private const int MaxRows = SudokuSize * SudokuSize * SudokuSize;
        private const int MaxColumns = SudokuSize * SudokuSize * 4;
        private const int Divisor = 3;
        private const int K = 50;
        private List<int> _indexList;
        private int[] _puzzle;
        private int[,] _problemMatrix;

        public override SudokuPuzzle Generate()
        {
            PrepareValidSudokuPuzzle();
            SudokuPuzzle sudokuPuzzle = CreateValidSudokuPuzzle();
            return sudokuPuzzle;
        }

        private void PrepareValidSudokuPuzzle()
        {
            _puzzle = new int[81];
            for (int index = 0; index < SudokuSize; index += 3)
            {
                List<int> sudokuNumbers = Enumerable.Range(1, 9).ToList().Shuffle();
                for (int i = 0; i <= 2; i++)
                {
                    _puzzle[GetSudokuIndex(index, index + i)] = TakeAndRemoveFirst(sudokuNumbers);
                    _puzzle[GetSudokuIndex(index + 1, index + i)] = TakeAndRemoveFirst(sudokuNumbers);
                    _puzzle[GetSudokuIndex(index + 2, index + i)] = TakeAndRemoveFirst(sudokuNumbers);
                }
            }
            RecursiveFill();

            int TakeAndRemoveFirst(IList<int> list)
            {
                int num = list[0];
                list.RemoveAt(0);
                return num;
            }
        }
        private bool RecursiveFill()
        {
            if (!TryFindEmptyCell(out int row, out int column))
                return true;
            for (byte number = 1; number <= 9; number++)
            {
                int previousNumber = _puzzle[GetSudokuIndex(row, column)];
                if (!TryPlaceNumber(number, row, column))
                    continue;
                if (RecursiveFill())
                    return true;
                _puzzle[GetSudokuIndex(row, column)] = previousNumber;
            }
            return false;
        }
        private bool TryPlaceNumber(byte number, int row, int column)
        {
            if (!IsNumberUniqueIn3By3Box(number, row, column) ||
                !IsNumberUniqueInBothRowAndColumn(number, row, column))
                return false;
            _puzzle[GetSudokuIndex(row, column)] = number;
            return true;
        }
        private bool TryFindEmptyCell(out int row, out int column)
        {
            for (int i = 0; i < SudokuSize; i++)
            {
                for (int j = 0; j < SudokuSize; j++)
                {
                    if (!_puzzle[GetSudokuIndex(i, j)].Equals(0))
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
        private bool IsNumberUniqueIn3By3Box(int number, int row, int column)
        {
            int sqrt = (int)Math.Sqrt(SudokuSize);
            int x = row - row % sqrt;
            int y = column - column % sqrt;
            for (int i = x; i < x + sqrt; i++)
            for (int j = y; j < y + sqrt; j++)
                if (_puzzle[GetSudokuIndex(i, j)].Equals((byte)number))
                    return false;
            return true;
        }
        private bool IsNumberUniqueInBothRowAndColumn(int number, int row, int column)
        {
            for (int i = 0; i < SudokuSize; i++)
                if (_puzzle[GetSudokuIndex(row, i)].Equals((byte)number) || _puzzle[GetSudokuIndex(i, column)].Equals((byte)number))
                    return false;
            return true;
        }
        private static int GetSudokuIndex(int row, int column)
        {
            return row * SudokuSize + column;
        }
        private void SudokuExactCover()
        {
            _problemMatrix = new int[MaxRows, MaxColumns];
            int hBase = 0;

            // row-column constraints
            for (int r = 1; r <= SudokuSize; r++)
            {
                for (int c = 1; c <= SudokuSize; c++, hBase++)
                {
                    for (int n = 1; n <= SudokuSize; n++)
                    {
                        _problemMatrix[GetIndex(r, c, n), hBase] = 1;
                    }
                }
            }

            // row-number constraints
            for (int r = 1; r <= SudokuSize; r++)
            {
                for (int n = 1; n <= SudokuSize; n++, hBase++)
                {
                    for (int c1 = 1; c1 <= SudokuSize; c1++)
                    {
                        _problemMatrix[GetIndex(r, c1, n), hBase] = 1;
                    }
                }
            }

            // column-number constraints

            for (int c = 1; c <= SudokuSize; c++)
            {
                for (int n = 1; n <= SudokuSize; n++, hBase++)
                {
                    for (int r1 = 1; r1 <= SudokuSize; r1++)
                    {
                        _problemMatrix[GetIndex(r1, c, n), hBase] = 1;
                    }
                }
            }

            // box-number constraints

            for (int br = 1; br <= SudokuSize; br += Divisor)
            {
                for (int bc = 1; bc <= SudokuSize; bc += Divisor)
                {
                    for (int n = 1; n <= SudokuSize; n++, hBase++)
                    {
                        for (int rDelta = 0; rDelta < Divisor; rDelta++)
                        {
                            for (int cDelta = 0; cDelta < Divisor; cDelta++)
                            {
                                _problemMatrix[GetIndex(br + rDelta, bc + cDelta, n), hBase] = 1;
                            }
                        }
                    }
                }
            }
        }
        private void MakeExactCoverGridFromSudoku()
        {
            for (int i = 1; i <= SudokuSize; i++)
            {
                for (int j = 1; j <= SudokuSize; j++)
                {
                    int n = _puzzle[GetSudokuIndex(i - 1, j - 1)];
                    if (n == 0)
                        continue;
                    for (int num = 1; num <= SudokuSize; num++)
                    {
                        if (num == n)
                            continue;
                        int index = GetIndex(i, j, num);
                        for (int k = 0; k < MaxColumns; k++)
                        {
                            _problemMatrix[index, k] = 0;
                        }
                    }
                }
            }
        }
        private static int GetIndex(int row, int col, int num)
        {
            return (row - 1) * SudokuSize * SudokuSize + (col - 1) * SudokuSize + (num - 1);
        }
        private SudokuPuzzle CreateValidSudokuPuzzle()
        {
            _indexList = Enumerable.Range(0, 81).ToList().Shuffle();
            RecursiveRemove();
            byte[] bytes = _puzzle.Select(n => (byte) (n + 48)).ToArray();
            PuzzleState puzzleState = new PuzzleState(bytes);
            SudokuPuzzle sudoku = new SudokuPuzzle(puzzleState);
            return sudoku;
        }
        private bool RecursiveRemove()
        {
            _indexList = Enumerable.Range(0, 81).ToList().Shuffle();

            foreach (int index in _indexList)
            {
                int previousNumber = _puzzle[index];
                if (!TryRemoveNumber(index))
                    continue;
                if (_puzzle.Count(b => b == 0) > K)
                    return true;
                if (RecursiveRemove())
                {
                    return true;
                }

                _puzzle[index] = previousNumber;
            }

            return false;
        }
        private bool TryRemoveNumber(int index)
        {
            int number = _puzzle[index];
            _puzzle[index] = 0;
            SudokuExactCover();
            MakeExactCoverGridFromSudoku();
            IEnumerable<Solution> solutions = new Dlx().Solve(_problemMatrix);
            if (solutions.Count() == 1)
                return true;
            _puzzle[index] = number;
            return false;
        }
    }

    public static class ListExtensions
    {
        private static readonly Random Random = new Random();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                int j = Random.Next(i, list.Count);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            return list;
        }
    }
}
