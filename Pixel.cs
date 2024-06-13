using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_WindowsFormsApp
{
    internal class Pixel
    {
        public Point location;
        private bool value;
        private Color color;

        public Pixel(Point l)
        {
            location = l;
            color = Constants.BackGroundColor;
        }

        public void SetColor(Color c)
        {
            value = c != Constants.BackGroundColor;
            color = c;
        }

        public Color GetColor()
        {
            return color;
        }

        public bool GetValue()
        {
            return value;
        }
    }
}
