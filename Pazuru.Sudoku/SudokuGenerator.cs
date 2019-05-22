using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Pazuru.Domain;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pazuru.Sudoku
{
    public sealed class SudokuGenerator : PuzzleGenerator<SudokuPuzzle>
    {
        private const int Length = 9;
        private byte[] _puzzle = new byte[81];
        private List<int> indexList;

        public override SudokuPuzzle Generate()
        {
            _puzzle = new byte[81];
            
            for (int index = 0; index < Length; index += 3)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                List<byte> list = Enumerable.Range(1, 9).Select(a => (byte)a).ToList();

                List<byte> sudokuNumbers = list.Shuffle();
                _puzzle[GetIndex(index, index)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index, index + 1)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index, index + 2)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index + 1, index)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index + 1, index + 1)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index + 1, index + 2)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index + 2, index)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index + 2, index + 1)] = TakeAndRemoveFirst(sudokuNumbers);
                _puzzle[GetIndex(index + 2, index + 2)] = TakeAndRemoveFirst(sudokuNumbers);
            }

            RecursiveFill();
            indexList = Enumerable.Range(0, 80).ToList().Shuffle();
            RecursiveRemove();

            PuzzleState puzzleState = new PuzzleState(_puzzle);
            SudokuPuzzle sudoku = new SudokuPuzzle(puzzleState);
            return sudoku;

            byte TakeAndRemoveFirst(IList<byte> list)
            {
                byte num = list[0];
                list.RemoveAt(0);
                return num;
            }
        }

        private bool RecursiveRemove()
        {
            foreach (int index in indexList)
            {
                byte previousNumber = _puzzle[index];
                if (!TryRemoveNumber(index))
                    continue;
                if (RecursiveRemove())
                {
                    return true;
                }

                _puzzle[index] = previousNumber;
            }

            return false;
        }

        private static int GetIndex(int row, int column)
        {
            return row * Length + column;
        }

        private bool RecursiveFill()
        {
            if (!TryFindEmptyCell(out int row, out int column))
            {
                return true;
            }

            for (byte number = 1; number <= 9; number++)
            {
                byte previousNumber = _puzzle[GetIndex(row, column)];
                if (!TryPlaceNumber(number, row, column))
                    continue;
                if (RecursiveFill())
                {
                    return true;
                }
                _puzzle[GetIndex(row, column)] = previousNumber;
            }

            return false;
        }

        private bool TryRemoveNumber(int index)
        {
            byte number = _puzzle[index];
            _puzzle[index] = 0;
            int solutionCount = 0;

            Thread thread = new Thread(() => { solutionCount = GetSolutionCount(); }, 192_000_000);
            thread.Start();
            thread.Join();
            Debug.WriteLine($"solution count: {solutionCount}");
            if (solutionCount == 1)
            {
                return true;
            }
            _puzzle[index] = number;
            return false;
        }

        private int GetSolutionCount()
        {
            int solutionCount = 0;
            RecursiveSolve();
            return solutionCount;

            bool RecursiveSolve()
            {
                if (!TryFindEmptyCell(out int row, out int column))
                {
                    return true;
                }

                for (byte number = 1; number <= 9; number++)
                {
                    byte previousNumber = _puzzle[GetIndex(row, column)];
                    if (!TryPlaceNumber(number, row, column))
                        continue;
                    if (RecursiveSolve())
                    {
                        solutionCount++;
                    }
                    _puzzle[GetIndex(row, column)] = previousNumber;
                }

                return false;
            }
        }
        private bool TryPlaceNumber(byte number, int row, int column)
        {
            if (!IsNumberUniqueIn3By3Box(number, row, column) ||
                !IsNumberUniqueInBothRowAndColumn(number, row, column))
                return false;
            _puzzle[GetIndex(row, column)] = number;
            return true;
        }

        private bool TryFindEmptyCell(out int row, out int column)
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (_puzzle[GetIndex(i, j)].Equals(0))
                    {
                        row = i;
                        column = j;
                        return true;
                    }
                }
            }
            row = 0;
            column = 0;
            return false;
        }

        private bool IsNumberUniqueIn3By3Box(int number, int row, int column)
        {
            int sqrt = (int)Math.Sqrt(Length);
            int x = row - row % sqrt;
            int y = column - column % sqrt;
            for (int i = x; i < x + sqrt; i++)
            for (int j = y; j < y + sqrt; j++)
                if (_puzzle[GetIndex(i, j)].Equals((byte)number))
                    return false;
            return true;
        }

        private bool IsNumberUniqueInBothRowAndColumn(int number, int row, int column)
        {
            for (int i = 0; i < Length; i++)
                if (_puzzle[GetIndex(row, i)].Equals((byte)number) || _puzzle[GetIndex(i, column)].Equals((byte)number))
                    return false;
            return true;
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
