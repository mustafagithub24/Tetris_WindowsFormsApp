using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WindowsFormsApp
{
    internal class Constants
    {
        public static int PixelSize = 20;
        public static int PixelMargin = 1;

        public static Size BoardSize = new Size(10, 20);
        public static Size BoardMargin = new Size(0, 0);

        public static Size StatisticsSize = new Size(3, 21);
        public static Size StatisticsMargin = new Size(30, 10);

        public static Size NextSize = new Size(3, 3);
        public static Size NextMargin = new Size(30, 50);

        public static Color BackGroundColor = Color.Black;
        public enum Direction { Left, Right, Down, Up }
    }
}
