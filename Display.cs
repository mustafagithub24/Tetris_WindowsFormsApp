using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WindowsFormsApp
{
    internal class Display
    {
        public Pixel[,] pixels;
        private Graphics graphics;
        private Size margin;

        public Display(Graphics g, Size s, Size m)
        {
            graphics = g;
            margin = m;
            pixels = new Pixel[s.Height, s.Width];
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    pixels[i, j] = new Pixel(new Point(j, i));
                }
            }
        }

        public void SetPixel(Color color, Point location)
        {
            pixels[location.Y, location.X].SetColor(color);
        }

        public void SetPiece(Piece piece)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (piece.PixelMatrix[i, j].GetValue())
                    {
                        SetPixel(piece.PixelMatrix[i, j].GetColor(), piece.PixelMatrix[i, j].location);
                    }
                }
            }
        }

        private void DrawPixel(Pixel pixel, bool addOrRemove = true, bool isNext = false)
        {
            graphics.FillRectangle(new Pen(addOrRemove ? pixel.GetColor() : Constants.BackGroundColor).Brush,
                           pixel.location.X * Constants.PixelSize * (isNext ? 2 : 1) + Constants.PixelMargin * (isNext ? 2 : 1) + margin.Width,
                           pixel.location.Y * Constants.PixelSize * (isNext ? 2 : 1) + Constants.PixelMargin * (isNext ? 2 : 1) + margin.Height,
                           Constants.PixelSize * (isNext ? 2 : 1) - 2 * Constants.PixelMargin * (isNext ? 2 : 1),
                           Constants.PixelSize * (isNext ? 2 : 1) - 2 * Constants.PixelMargin * (isNext ? 2 : 1));
        }

        public void DrawPiece(Piece piece, bool addOrRemove = true, bool isNext = false)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (piece.PixelMatrix[i, j].GetValue())
                    {
                        DrawPixel(piece.PixelMatrix[i, j], addOrRemove, isNext);
                    }
                }
            }
        }

        public void DrawString(int stat, Color color, int index)
        {
            graphics.DrawString(stat.ToString("00000"), new Font("Consolas", 20), new Pen(color).Brush, new Point(100, 5 + 60 * index));
        }

        public void Draw(bool addOrRemove = true, bool isNext = false)
        {
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    DrawPixel(pixels[i, j], addOrRemove, isNext);
                }
            }
        }
    }
}
