using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_WindowsFormsApp
{
    internal class Game
    {
        public Game(Display b, Display s, Display n, Label scoreLabel, Label levelLabel, Timer _timer)
        {
            board = b;
            statistics = s;
            next = n;
            pieceStatistics = new int[7];
            random = new Random();
            currentPiece = new Piece(random.Next(7));
            nextPiece = new Piece(random.Next(7));
            currentPiece.SetPieceLocation(3, 0);
            score = scoreLabel;
            level = levelLabel;
            scoreValue = 0;
            levelValue = 1;
            timer = _timer;
        }

        private Display board;
        private Display statistics;
        private Display next;
        private Label score;
        private Label level;
        private Timer timer;
        private int scoreValue, levelValue;
        private int[] pieceStatistics;
        private Random random;
        private Piece currentPiece;
        private Piece nextPiece;

        private void DrawStatistics()
        {
            for (int i = 0; i < 7; i++)
            {
                Piece p = new Piece(i);
                p.SetPieceLocation(0, i * 3);
                statistics.DrawString(pieceStatistics[i], p.PieceColor, i);
                statistics.DrawPiece(p);
            }
        }

        private void UpdateStatistics(int index)
        {
            statistics.DrawString(pieceStatistics[index], Constants.BackGroundColor, index);
            pieceStatistics[index]++;
            statistics.DrawString(pieceStatistics[index], new Piece(index).PieceColor, index);
        }

        private void UpdatePieces()
        {
            next.DrawPiece(nextPiece, false, true);// erase next piece
            board.SetPiece(currentPiece);
            LineCheck();
            currentPiece = nextPiece;
            currentPiece.SetPieceLocation(3, 0);
            UpdateStatistics(currentPiece.PieceIndex);
            nextPiece = new Piece(random.Next(7));
            next.DrawPiece(nextPiece, true, true);// draw next piece
            board.DrawPiece(currentPiece);
            GameOverCheck();
        }
        private void GameOverCheck()
        {
            bool test = false;
            for (int i = 0; i < currentPiece.PixelMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < currentPiece.PixelMatrix.GetLength(1); j++)
                {
                    if (currentPiece.PixelMatrix[i, j].GetValue() &&
                        board.pixels[currentPiece.PixelMatrix[i, j].location.Y,
                                        currentPiece.PixelMatrix[i, j].location.X].GetValue())
                    {
                        test = true;
                        break;
                    }
                }
                if (test)
                {
                    break;
                }
            }
            if (test)
            {
                timer.Stop();
                MessageBox.Show("Game over! Your score is " + scoreValue);
                if (MessageBox.Show("Start new game?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Restart();
                }
            }
        }

        private void LineCheck()
        {
            List<int> lineIndices = new List<int>();
            for (int i = 0; i < board.pixels.GetLength(0); i++)
            {
                bool test = true;
                for (int j = 0; j < board.pixels.GetLength(1); j++)
                {
                    if (!board.pixels[i, j].GetValue())
                    {
                        test = false;
                        break;
                    }
                }
                if (test)
                {
                    lineIndices.Add(i);
                }
            }
            if (lineIndices.Count > 0)
            {
                LineBlow(lineIndices);
                scoreValue += lineIndices.Count;
                score.Text = scoreValue.ToString("00000");
                levelValue = 1 + scoreValue / 10;
                level.Text = levelValue.ToString();
                timer.Interval = Convert.ToInt32(1000.0 * Math.Pow(0.9, levelValue - 1)); // interval decrease 10%
            }
        }

        private void LineBlow(List<int> lineIndices)
        {
            int lineCount = 0;
            for (int i = lineIndices.Count - 1; i >= 0; i--)
            {
                for (int j = lineIndices[i] + lineCount - 1; j >= 0; j--)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        board.SetPixel(board.pixels[j, k].GetColor(), new Point(k, j + 1));
                    }
                }
                lineCount++;
            }
            board.Draw();
        }

        // false for collision
        private bool CollisionCheck(Constants.Direction direction)
        {
            switch (direction)
            {
                case Constants.Direction.Up:
                    Pixel[,] rotatedPiece = Piece.GetPixelMatrix(
                        currentPiece.PieceIndex,
                        (currentPiece.PieceState + 1) % 4,
                        currentPiece.PieceLocation);
                    for (int i = 0; i < rotatedPiece.GetLength(0); i++)
                    {
                        for (int j = 0; j < rotatedPiece.GetLength(1); j++)
                        {
                            int x = rotatedPiece[i, j].location.X;
                            int y = rotatedPiece[i, j].location.Y;
                            if (rotatedPiece[i, j].GetValue() && (x < 0 || x > board.pixels.GetLength(1) - 1 || y < 0 || y > board.pixels.GetLength(0) - 1)) { return false; }
                            if (rotatedPiece[i, j].GetValue() && board.pixels[y, x].GetValue()) { return false; }
                        }
                    }
                    break;
                case Constants.Direction.Left:
                    for (int i = 0; i < currentPiece.PixelMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < currentPiece.PixelMatrix.GetLength(1); j++)
                        {
                            int x = currentPiece.PixelMatrix[i, j].location.X;
                            int y = currentPiece.PixelMatrix[i, j].location.Y;
                            if (currentPiece.PixelMatrix[i, j].GetValue() && x == 0) { return false; }
                            if (currentPiece.PixelMatrix[i, j].GetValue() && board.pixels[y, x - 1].GetValue()) { return false; }
                        }
                    }
                    break;
                case Constants.Direction.Right:
                    for (int i = 0; i < currentPiece.PixelMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < currentPiece.PixelMatrix.GetLength(1); j++)
                        {
                            int x = currentPiece.PixelMatrix[i, j].location.X;
                            int y = currentPiece.PixelMatrix[i, j].location.Y;
                            if (currentPiece.PixelMatrix[i, j].GetValue() && x == board.pixels.GetLength(1) - 1) { return false; }
                            if (currentPiece.PixelMatrix[i, j].GetValue() && board.pixels[y, x + 1].GetValue()) { return false; }
                        }
                    }
                    break;
                case Constants.Direction.Down:
                    for (int i = 0; i < currentPiece.PixelMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < currentPiece.PixelMatrix.GetLength(1); j++)
                        {
                            int x = currentPiece.PixelMatrix[i, j].location.X;
                            int y = currentPiece.PixelMatrix[i, j].location.Y;
                            if (currentPiece.PixelMatrix[i, j].GetValue() && y == board.pixels.GetLength(0) - 1) { return false; }
                            if (currentPiece.PixelMatrix[i, j].GetValue() && board.pixels[y + 1, x].GetValue()) { return false; }
                        }
                    }
                    break;
            }
            return true;
        }

        public void Move(Constants.Direction direction)
        {
            board.DrawPiece(currentPiece, false, false); // false for remove

            switch (direction)
            {
                case Constants.Direction.Up:
                    if (CollisionCheck(direction))
                    {
                        currentPiece.PixelMatrix = currentPiece.Rotate();
                    }
                    break;
                case Constants.Direction.Down:
                    if (CollisionCheck(direction))
                    {
                        currentPiece.SetPieceLocation(currentPiece.PieceLocation.X, currentPiece.PieceLocation.Y + 1);
                    }
                    else
                    {
                        board.DrawPiece(currentPiece);
                        UpdatePieces();
                    }
                    break;
                case Constants.Direction.Left:
                    if (CollisionCheck(direction))
                    {
                        currentPiece.SetPieceLocation(currentPiece.PieceLocation.X - 1, currentPiece.PieceLocation.Y);
                    }
                    break;
                case Constants.Direction.Right:
                    if (CollisionCheck(direction))
                    {
                        currentPiece.SetPieceLocation(currentPiece.PieceLocation.X + 1, currentPiece.PieceLocation.Y);
                    }
                    break;
                default:
                    break;
            }

            board.DrawPiece(currentPiece); // true(by default) for add
        }

        public void Start()
        {
            board.DrawPiece(currentPiece);
            next.DrawPiece(nextPiece, true, true);
            DrawStatistics();
            UpdateStatistics(currentPiece.PieceIndex);
        }
    }
}
