namespace Pazuru.Domain
{
    public abstract class PuzzlePrinter<TPuzzle> where TPuzzle : Puzzle
    {
        protected const char LeftTop = '┌';
        protected const char RightTop = '┐';
        protected const char LeftBottom = '└';
        protected const char RightBottom = '┘';
        protected const char HorizontalJointTop = '┬';
        protected const char HorizontalJointBottom = '┴';
        protected const char VerticalJointLeft = '├';
        protected const char Joint = '┼';
        protected const char VerticalJointRight = '┤';
        protected const char HorizontalLine = '─';
        protected const char VerticalLine = '│';

        public abstract string Print(TPuzzle puzzle);
    }
}
