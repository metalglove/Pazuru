using System.ComponentModel;

namespace Pazuru.Hitori
{
    public enum HitoriMoveColorKey
    {
        [Description("Unset")]
        None = 'N',
        [Description("Undecided")]
        Grey = 'G',
        [Description("Eliminated")]
        Black = 'B',
        [Description("In final solution")]
        White = 'W'
    }
}