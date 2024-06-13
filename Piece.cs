using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Tetris_WindowsFormsApp
{

    internal class Piece
    {
        public Piece(int index)
        {
            PieceIndex = index % 7;
            PieceState = 0;
            PieceLocation = new Point(0, 0);
            PixelMatrix = GetPixelMatrix(PieceIndex, PieceState, PieceLocation);
            PieceColor = PieceColors[PieceIndex];
        }

        public Color PieceColor;
        public Pixel[,] PixelMatrix;
        public Point PieceLocation;
        public int PieceIndex;
        public int PieceState;

        public void SetPieceLocation(int x, int y)
        {
            PieceLocation = new Point(x, y);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PixelMatrix[i, j].location = new Point(x + j, y + i);
                }
            }
        }

        private static Color[] PieceColors = { Color.Red, Color.Green, Color.Blue, Color.Cyan, Color.Magenta, Color.Yellow, Color.Orange };
        private static bool[,,,] PieceMatrices =
        {
            {// T-Piece
                {
                    { true, true, true },
                    { false, true, false },
                    { false, false, false }
                },
                {
                    { false, false, true },
                    { false, true, true },
                    { false, false, true }
                },
                {
                    { false, false, false },
                    { false, true, false },
                    { true, true, true }
                },
                {
                    { true, false, false },
                    { true, true, false },
                    { true, false, false }
                }
            },
            {// J-Piece
                {
                    { true, true, true },
                    { false, false, true },
                    { false, false, false }
                },
                {
                    { false, false, true },
                    { false, false, true },
                    { false, true, true }
                },
                {
                    { false, false, false },
                    { true, false, false },
                    { true, true, true }
                },
                {
                    { true, true, false },
                    { true, false, false },
                    { true, false, false }
                }
            },
            {// Z-Piece
                {
                    { true, true, false },
                    { false, true, true },
                    { false, false, false }
                },
                {
                    { false, false, true },
                    { false, true, true },
                    { false, true, false }
                },
                {
                    { true, true, false },
                    { false, true, true },
                    { false, false, false }
                },
                {
                    { false, false, true },
                    { false, true, true },
                    { false, true, false }
                }
            },
            {// O-Piece
                {
                    { false, true, true },
                    { false, true, true },
                    { false, false, false }
                },
                {
                    { false, true, true },
                    { false, true, true },
                    { false, false, false }
                },
                {
                    { false, true, true },
                    { false, true, true },
                    { false, false, false }
                },
                {
                    { false, true, true },
                    { false, true, true },
                    { false, false, false }
                }
            },
            {// S-Piece
                {
                    { false, true, true },
                    { true, true, false },
                    { false, false, false }
                },
                {
                    { false, true, false },
                    { false, true, true },
                    { false, false, true }
                },
                {
                    { false, true, true },
                    { true, true, false },
                    { false, false, false }
                },
                {
                    { false, true, false },
                    { false, true, true },
                    { false, false, true }
                }
            },
            {// L-Piece
                {
                    { true, true, true },
                    { true, false, false },
                    { false, false, false }
                },
                {
                    { false, true, true },
                    { false, false, true },
                    { false, false, true }
                },
                {
                    { false, false, false },
                    { false, false, true },
                    { true, true, true }
                },
                {
                    { true, false, false },
                    { true, false, false },
                    { true, true, false }
                }
            },
            {// I-Piece
                {
                    { true, true, true },
                    { false, false, false },
                    { false, false, false }
                },
                {
                    { false, true, false },
                    { false, true, false },
                    { false, true, false }
                },
                {
                    { true, true, true },
                    { false, false, false },
                    { false, false, false }
                },
                {
                    { false, true, false },
                    { false, true, false },
                    { false, true, false }
                }
            }
        };

        public static Pixel[,] GetPixelMatrix(int index, int state, Point location)
        {
            Pixel[,] result = new Pixel[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i, j] = new Pixel(new Point(j, i));
                    result[i, j].SetColor(PieceMatrices[index, state, i, j] ? PieceColors[index] : Constants.BackGroundColor);
                    result[i, j].location = new Point(location.X + j, location.Y + i);
                }
            }
            return result;
        }

        public Pixel[,] Rotate()
        {
            int pieceState = PieceState;
            pieceState++;
            pieceState %= 4;
            PieceState = pieceState;
            return GetPixelMatrix(PieceIndex, pieceState, PieceLocation);
        }
    }
}
