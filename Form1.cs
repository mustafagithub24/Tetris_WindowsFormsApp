using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Tetris_WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            g_Board = pb_Board.CreateGraphics();
            g_Statistics = pb_Statistics.CreateGraphics();
            g_Next = pb_Next.CreateGraphics();

            d_Board = new Display(g_Board, Constants.BoardSize, Constants.BoardMargin);
            d_Statistics = new Display(g_Statistics, Constants.StatisticsSize, Constants.StatisticsMargin);
            d_Next = new Display(g_Next, Constants.NextSize, Constants.NextMargin);

            game_Tetris = new Game(d_Board, d_Statistics, d_Next, lbl_Score, lbl_Level, tmr_Timer);
        }
        Graphics g_Board, g_Statistics, g_Next;
        Display d_Board, d_Statistics, d_Next;
        Game game_Tetris;

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (MessageBox.Show("Start new game?", "",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                game_Tetris.Start();
                tmr_Timer.Enabled = true;
            }
        }

        private void tmr_Timer_Tick(object sender, EventArgs e)
        {
            game_Tetris.Move(Constants.Direction.Down);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    game_Tetris.Move(Constants.Direction.Up);
                    break;
                case Keys.Down:
                    game_Tetris.Move(Constants.Direction.Down);
                    break;
                case Keys.Left:
                    game_Tetris.Move(Constants.Direction.Left);
                    break;
                case Keys.Right:
                    game_Tetris.Move(Constants.Direction.Right);
                    break;
                default:
                    break;
            }
        }
    }
}
