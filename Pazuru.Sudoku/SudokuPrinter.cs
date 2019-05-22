using Pazuru.Domain;
using System;
using System.Text;

namespace Pazuru.Sudoku
{
    public sealed class SudokuPrinter : PuzzlePrinter<SudokuPuzzle>
    {
        public override string Print(SudokuPuzzle puzzle)
        {
            StringBuilder stringBuilder = new StringBuilder();
            const int deciderWidth = 2;
            int width = puzzle.Length;
            int length = puzzle.Length;
            int decider = width * deciderWidth * 2;
            int newDecider = (decider - 6) / 2;
            stringBuilder.Append($"{VerticalLine.ToString().PadRight(newDecider, ' ')}Sudoku{"".PadRight(newDecider, ' ')}");
            stringBuilder.Append($"{VerticalLine}");
            stringBuilder.AppendLine();
            string splitter = VerticalJointLeft.ToString().PadRight(stringBuilder.Length - 3, HorizontalLine);
            stringBuilder.Append(VerticalJointLeft);
            for (int j = 0; j < width; j++)
            {
                stringBuilder.Append($"{HorizontalLine}".PadRight(deciderWidth * 2 - 1, HorizontalLine) + HorizontalJointTop);
            }
            stringBuilder.Insert(0, splitter + RightTop + Environment.NewLine);
            stringBuilder.Remove(0, 1);
            stringBuilder.Insert(0, LeftTop);
            stringBuilder.Replace(HorizontalJointTop, VerticalJointRight, stringBuilder.Length - 1, 1);
            stringBuilder.AppendLine();

            for (int x = 0; x < length; x++)
            {
                int y = 0;
                for (; y < width; y++)
                {
                    stringBuilder.Append(puzzle[x, y] == default
                        ? $"{VerticalLine}".PadRight(deciderWidth * 2, ' ')
                        : $"{VerticalLine}".PadRight(deciderWidth, ' ') +
                          $"{puzzle[x, y]}".PadRight(deciderWidth, ' '));
                }
                if (x >= length - 1 && y >= width - 1)
                {
                    stringBuilder.AppendLine(VerticalLine.ToString());
                    continue;
                }
                stringBuilder.AppendLine(VerticalLine.ToString());
                stringBuilder.Append($"{VerticalJointLeft}".PadRight(deciderWidth * 2, HorizontalLine));
                for (int j = 1; j < width; j++)
                {
                    stringBuilder.Append($"{Joint}".PadRight(deciderWidth * 2, HorizontalLine));
                }
                stringBuilder.AppendLine(VerticalJointRight.ToString());
            }

            stringBuilder.Append(LeftBottom);
            for (int j = 0; j < width; j++)
            {
                stringBuilder.Append($"{HorizontalLine}".PadRight(deciderWidth * 2 - 1, HorizontalLine) + HorizontalJointBottom);
            }
            stringBuilder.Replace(HorizontalJointBottom, RightBottom, stringBuilder.Length - 1, 1);
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}
