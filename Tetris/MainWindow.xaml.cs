/*
 * Travis Cartmell
 * 
 * 8/5
 * Got the game panel set up, also got a block drawn and can move around
 * 
 * 8/6
 * Added a Piece class that keeps track of the piece's location and color and everything. Only one block right now though. It does fall every
 * second and can be manipulated left, right, and down without problems. Collision also works.
 * Also added another 2d array of labels that helps me keep track of the playing area with 0s, 1s, and 2s.
 * 
 * 8/7
 * Added all the actual shapes now instead of just one block. Collision seems to work, but can't test so well now since I don't have rotation
 * implemented yet.
 * 
 * 8/8
 * Can rotate the line piece 360 degrees now. Collision is ignored while rotating for now. Not looking forward to changing that.
 * 
 * 8/12
 * Refactored my rotation code to be waaaaay easier to edit, and about 1000 less lines of code. Have all the rotations in now for all the shapes, with all the collision logic in there.
 * Wasn't as bad as I thought it was going to be. Left now is to actually make it work like a normal tetris game with scoring, disappearing lines, advancing the level, etc.
 * 
 * 8/13
 * Added rows disappearing and blocks falling when a row disappears. Works perfectly for only one row, but doesn't work correctly for 2+. I have
 * no idea why it doesn't work, and it was very difficult to just get what I did implemented.
 * 
 * 8/14
 * I'm dumb. The problem was when I deleted say row 17, row 17 would delete and then everything would move down. Now it wants to delete row 16, but row 16
 * is now what row 15 was previously and the real row 16 is row 17. I just started deleteing from top to bottom row and everything works perfectly. Anyway,
 * I got the game working fine, got score, lines, and level to show up and increment properly, got them to advance correctly on level up. Got restart 
 * implemented finally. Got the next block to display.
 * 
 * BUGS:
 * If the user presses left and right really fast while the piece is at the bottom of the screen, weird things happen. <-- fixed itself somehow.
 * The shape's drawing wasn't getting updated correctly which caused it to be desynced with the array which was confusing to look at as well as
 * caused it to crash if you went far enough down. <-- It was fixed by adding an extra set of code in the tick method, I don't understand why
 * this wasn't a problem when I only had one block instead of four though. It should have been desynced then too.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();

        private static int game_piece_dx = 30;
        private static int game_piece_dy = 30;

        RoutedCommand newCmd = new RoutedCommand();

        private int score = 0;
        private int score_base = 100;
        private int score_bonus = 50;
        private int lines = 0;
        private int lvl = 1;
        private int time = 500;
        private static int lines_to_advance = 10;

        private int num;
        private int next_piece;
        private Random random = new Random();

        private int[][] board = new int[10][];
        private Label[][] board_rect = new Label[10][];

        Piece piece;
        List<Piece> pieces = new List<Piece>();
        int[] full_row_test = new int[10];
        bool new_piece = true;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, time);
            gameTimer.Tick += GameTimer_Tick;

            newCmd.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newCmd, Menu_restart_Click));

            for (int i = 0; i < board.Length; i++)
            {
                full_row_test[i] = 2;
                board[i] = new int[18];
                //board_rect[i] = new Label[18];

                for (int j = 0; j < board[i].Length; j++)
                {
                    board[i][j] = 0;
                    /*board_rect[i][j] = new Label();
                    board_rect[i][j].Content = "0";
                    Canvas.SetLeft(board_rect[i][j], i * game_piece_dx);
                    Canvas.SetTop(board_rect[i][j], j * game_piece_dy);
                    myGameCanvas.Children.Add(board_rect[i][j]);*/
                }
            }
            board[5][1] = 1;
            next_piece = random.Next(1, 8);

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            lbl_milli.Content = DateTime.Now.Millisecond;

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == 1)
                    {
                        if (new_piece)
                        {
                            piece = new Piece(i, j, next_piece);
                            pieces.Add(piece);

                            next_piece = random.Next(1, 8);
                            SetSource(next_piece);
                            
                            board[piece.pos_x_block2][piece.pos_y_block2] = 1;
                            board[piece.pos_x_block3][piece.pos_y_block3] = 1;
                            board[piece.pos_x_block4][piece.pos_y_block4] = 1;
                            /*board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "1";
                            board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "1";
                            board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "1";
                            board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "1";*/

                            Canvas.SetLeft(piece.getBlock1(), piece.pos_x_block1 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock1(), piece.pos_y_block1 * game_piece_dy);

                            Canvas.SetLeft(piece.getBlock2(), piece.pos_x_block2 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock2(), piece.pos_y_block2 * game_piece_dy);

                            Canvas.SetLeft(piece.getBlock3(), piece.pos_x_block3 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock3(), piece.pos_y_block3 * game_piece_dy);

                            Canvas.SetLeft(piece.getBlock4(), piece.pos_x_block4 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock4(), piece.pos_y_block4 * game_piece_dy);

                            myGameCanvas.Children.Add(piece.getBlock1());
                            myGameCanvas.Children.Add(piece.getBlock2());
                            myGameCanvas.Children.Add(piece.getBlock3());
                            myGameCanvas.Children.Add(piece.getBlock4());
                            new_piece = false;
                        }

                        else
                        {
                            if (piece.pos_y_block1 == board[i].Length - 1 || piece.pos_y_block2 == board[i].Length - 1 ||
                                piece.pos_y_block3 == board[i].Length - 1 || piece.pos_y_block4 == board[i].Length - 1 ||
                                board[piece.pos_x_block1][piece.pos_y_block1 + 1] == 2 || board[piece.pos_x_block2][piece.pos_y_block2 + 1] == 2 ||
                                board[piece.pos_x_block3][piece.pos_y_block3 + 1] == 2 || board[piece.pos_x_block4][piece.pos_y_block4 + 1] == 2)
                            {
                                board[piece.pos_x_block1][piece.pos_y_block1] = 2;
                                board[piece.pos_x_block2][piece.pos_y_block2] = 2;
                                board[piece.pos_x_block3][piece.pos_y_block3] = 2;
                                board[piece.pos_x_block4][piece.pos_y_block4] = 2;

                                /*board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "2";
                                board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "2";
                                board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "2";
                                board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "2";*/

                                List<int> rows_to_delete = RowFull();

                                lines += rows_to_delete.Count;

                                if (int.Parse(lbl_lines.Content.ToString()) < 10)
                                    lbl_lines.Content = "0" + lines.ToString();
                                else
                                    lbl_lines.Content = int.Parse(lbl_lines.Content.ToString()) + rows_to_delete.Count;

                                if (rows_to_delete.Count != 0)
                                {
                                    if (lines / lines_to_advance > 0)
                                    {
                                        Level();
                                    }

                                    score += rows_to_delete.Count * score_base + (rows_to_delete.Count - 1) * score_bonus;

                                    lbl_score.Content = score.ToString();

                                    foreach (int row in rows_to_delete)
                                    {

                                        foreach (Piece p in pieces)
                                        {
                                            if (p.pos_y_block1 == row)
                                            {
                                                p.removeBlock1();
                                                myGameCanvas.Children.Remove(p.getBlock1());
                                            }
                                            if (p.pos_y_block2 == row)
                                            {
                                                p.removeBlock2();
                                                myGameCanvas.Children.Remove(p.getBlock2());
                                            }
                                            if (p.pos_y_block3 == row)
                                            {
                                                p.removeBlock3();
                                                myGameCanvas.Children.Remove(p.getBlock3());
                                            }
                                            if (p.pos_y_block4 == row)
                                            {
                                                p.removeBlock4();
                                                myGameCanvas.Children.Remove(p.getBlock4());
                                            }
                                        }

                                        for (int k = 0; k < board.Length; k++)
                                        {
                                            board[k][row] += -2;
                                            //board_rect[k][row].Content = int.Parse(board_rect[k][row].Content.ToString()) + -2;
                                        }

                                        foreach (Piece p in pieces)
                                        {
                                            if (p.pos_y_block1 < row && p.block1_exist)
                                            {
                                                p.top_block1 += game_piece_dx;
                                                board[p.pos_x_block1][p.pos_y_block1] += -2;
                                                board[p.pos_x_block1][p.pos_y_block1 + 1] += 2;
                                                //board_rect[p.pos_x_block1][p.pos_y_block1].Content = int.Parse(board_rect[p.pos_x_block1][p.pos_y_block1].Content.ToString()) + -2;
                                                //board_rect[p.pos_x_block1][p.pos_y_block1 + 1].Content = int.Parse(board_rect[p.pos_x_block1][p.pos_y_block1 + 1].Content.ToString()) + 2;
                                                p.pos_y_block1++;
                                            }

                                            if (p.pos_y_block2 < row && p.block2_exist)
                                            {
                                                p.top_block2 += game_piece_dx;
                                                board[p.pos_x_block2][p.pos_y_block2] += -2;
                                                board[p.pos_x_block2][p.pos_y_block2 + 1] += 2;
                                                //board_rect[p.pos_x_block2][p.pos_y_block2].Content = int.Parse(board_rect[p.pos_x_block2][p.pos_y_block2].Content.ToString()) + -2;
                                                //board_rect[p.pos_x_block2][p.pos_y_block2 + 1].Content = int.Parse(board_rect[p.pos_x_block2][p.pos_y_block2 + 1].Content.ToString()) + 2;
                                                p.pos_y_block2++;
                                            }

                                            if (p.pos_y_block3 < row && p.block3_exist)
                                            {
                                                p.top_block3 += game_piece_dx;
                                                board[p.pos_x_block3][p.pos_y_block3] += -2;
                                                board[p.pos_x_block3][p.pos_y_block3 + 1] += 2;
                                                //board_rect[p.pos_x_block3][p.pos_y_block3].Content = int.Parse(board_rect[p.pos_x_block3][p.pos_y_block3].Content.ToString()) + -2;
                                                //board_rect[p.pos_x_block3][p.pos_y_block3 + 1].Content = int.Parse(board_rect[p.pos_x_block3][p.pos_y_block3 + 1].Content.ToString()) + 2;
                                                p.pos_y_block3++;
                                            }

                                            if (p.pos_y_block4 < row && p.block4_exist)
                                            {
                                                p.top_block4 += game_piece_dx;
                                                board[p.pos_x_block4][p.pos_y_block4] += -2;
                                                board[p.pos_x_block4][p.pos_y_block4 + 1] += 2;
                                                //board_rect[p.pos_x_block4][p.pos_y_block4].Content = int.Parse(board_rect[p.pos_x_block4][p.pos_y_block4].Content.ToString()) + -2;
                                                //board_rect[p.pos_x_block4][p.pos_y_block4 + 1].Content = int.Parse(board_rect[p.pos_x_block4][p.pos_y_block4 + 1].Content.ToString()) + 2;
                                                p.pos_y_block4++;
                                            }

                                            Canvas.SetTop(p.getBlock1(), p.top_block1);
                                            Canvas.SetTop(p.getBlock2(), p.top_block2);
                                            Canvas.SetTop(p.getBlock3(), p.top_block3);
                                            Canvas.SetTop(p.getBlock4(), p.top_block4);
                                        }
                                    }

                                    for (int k = 0; k < pieces.Count; k++)
                                    {
                                        if (pieces[k].size <= 0)
                                        {
                                            pieces.RemoveAt(k);
                                            k--;
                                        }
                                    }
                                }

                                board[5][1] = 1;
                                new_piece = true;
                                return;
                            }

                            board[piece.pos_x_block1][piece.pos_y_block1] = 0;
                            board[piece.pos_x_block1][piece.pos_y_block1 + 1] = 1;
                            board[piece.pos_x_block2][piece.pos_y_block2] = 0;
                            board[piece.pos_x_block2][piece.pos_y_block2 + 1] = 1;
                            board[piece.pos_x_block3][piece.pos_y_block3] = 0;
                            board[piece.pos_x_block3][piece.pos_y_block3 + 1] = 1;
                            board[piece.pos_x_block4][piece.pos_y_block4] = 0;
                            board[piece.pos_x_block4][piece.pos_y_block4 + 1] = 1;

                            /*board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "0";
                            board_rect[piece.pos_x_block1][piece.pos_y_block1 + 1].Content = "1";
                            board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "0";
                            board_rect[piece.pos_x_block2][piece.pos_y_block2 + 1].Content = "1";
                            board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "0";
                            board_rect[piece.pos_x_block3][piece.pos_y_block3 + 1].Content = "1";
                            board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "0";
                            board_rect[piece.pos_x_block4][piece.pos_y_block4 + 1].Content = "1";*/

                            piece.pos_y_block1++;
                            piece.pos_y_block2++;
                            piece.pos_y_block3++;
                            piece.pos_y_block4++;
                            piece.top_block1 += game_piece_dx;
                            piece.top_block2 += game_piece_dx;
                            piece.top_block3 += game_piece_dx;
                            piece.top_block4 += game_piece_dx;

                            Canvas.SetLeft(piece.getBlock1(), piece.pos_x_block1 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock1(), piece.pos_y_block1 * game_piece_dy);
                            Canvas.SetLeft(piece.getBlock2(), piece.pos_x_block2 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock2(), piece.pos_y_block2 * game_piece_dy);
                            Canvas.SetLeft(piece.getBlock3(), piece.pos_x_block3 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock3(), piece.pos_y_block3 * game_piece_dy);
                            Canvas.SetLeft(piece.getBlock4(), piece.pos_x_block4 * game_piece_dx);
                            Canvas.SetTop(piece.getBlock4(), piece.pos_y_block4 * game_piece_dy);
                        }
                        return;
                    }
                }
            }
        }

        private void SetSource(int num)
        {
            if (num == 1)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/line.png", UriKind.Relative));
            else if (num == 2)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/left-l.png", UriKind.Relative));
            else if (num == 3)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/right-l.png", UriKind.Relative));
            else if (num == 4)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/s.png", UriKind.Relative));
            else if (num == 5)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/z.png", UriKind.Relative));
            else if (num == 6)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/square.png", UriKind.Relative));
            else if (num == 7)
                img_next_piece.Source = new BitmapImage(new Uri(@"img/t.png", UriKind.Relative));
        }

        private void Level()
        {
            lvl++;
            score_base += 100;
            score_bonus += 50;

            lines -= lines_to_advance;
            if (lines < 0)
                lines = 0;

            int time_to_increase = (int)(time * .25);
            time -= time_to_increase;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, time);

            if (lvl < 10)
                lbl_level.Content = "0" + lvl.ToString();
            else
                lbl_level.Content = lvl.ToString();
        }

        private List<int> RowFull()
        {
            List<int> full_rows = new List<int>();
            int[] row = new int[10];

            //for (int i = board[0].Length - 1; i >= 0; i--)
            for (int i = 0; i < board[0].Length; i++)
            {
                if (board[0][i] == 2)
                {
                    row[0] = 2;
                    for (int k = 1; k < board.Length; k++)
                    {
                        row[k] = board[k][i];
                    }

                    if (row.SequenceEqual(full_row_test))
                    {
                        full_rows.Add(i);
                    }
                }
            }
            return full_rows;
        }
        private void Restart()
        {

            //game_piece_top = 0;
            //game_piece_left = 150;

            //Canvas.SetTop(game_piece, game_piece_top);

            lbl_score.Content = "00";
            lbl_level.Content = "01";
            lbl_lines.Content = "00";

            score = 0;
            score_base = 100;
            score_bonus = 50;
            lvl = 1;
            lines = 0;
            time = 500;

            foreach (Piece p in pieces)
            {
                myGameCanvas.Children.Remove(p.getBlock1());
                myGameCanvas.Children.Remove(p.getBlock2());
                myGameCanvas.Children.Remove(p.getBlock3());
                myGameCanvas.Children.Remove(p.getBlock4());
            }

            pieces = new List<Piece>();

            for (int i = 0; i < board.Length; i++)
            {
                board[i] = new int[18];

                for (int j = 0; j < board[i].Length; j++)
                {
                    board[i][j] = 0;
                }
            }
            board[5][1] = 1;
            new_piece = true;

            gameTimer.Stop();
        }

        private void GameOver()
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left && gameTimer.IsEnabled)
            {
                if (piece == null)
                    return;

                if (piece.left_block1 - game_piece_dx >= 0 && board[piece.pos_x_block1 - 1][piece.pos_y_block1] != 2 &&
                    piece.left_block2 - game_piece_dx >= 0 && board[piece.pos_x_block2 - 1][piece.pos_y_block2] != 2 &&
                    piece.left_block3 - game_piece_dx >= 0 && board[piece.pos_x_block3 - 1][piece.pos_y_block3] != 2 &&
                    piece.left_block4 - game_piece_dx >= 0 && board[piece.pos_x_block4 - 1][piece.pos_y_block4] != 2)
                {
                    piece.left_block1 -= game_piece_dx;
                    piece.left_block2 -= game_piece_dx;
                    piece.left_block3 -= game_piece_dx;
                    piece.left_block4 -= game_piece_dx;

                    board[piece.pos_x_block1][piece.pos_y_block1] = 0;
                    board[piece.pos_x_block1 - 1][piece.pos_y_block1] = 1;
                    board[piece.pos_x_block2][piece.pos_y_block2] = 0;
                    board[piece.pos_x_block2 - 1][piece.pos_y_block2] = 1;
                    board[piece.pos_x_block3][piece.pos_y_block3] = 0;
                    board[piece.pos_x_block3 - 1][piece.pos_y_block3] = 1;
                    board[piece.pos_x_block4][piece.pos_y_block4] = 0;
                    board[piece.pos_x_block4 - 1][piece.pos_y_block4] = 1;

                    /*board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "0";
                    board_rect[piece.pos_x_block1 - 1][piece.pos_y_block1].Content = "1";
                    board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "0";
                    board_rect[piece.pos_x_block2 - 1][piece.pos_y_block2].Content = "1";
                    board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "0";
                    board_rect[piece.pos_x_block3 - 1][piece.pos_y_block3].Content = "1";
                    board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "0";
                    board_rect[piece.pos_x_block4 - 1][piece.pos_y_block4].Content = "1";*/

                    piece.pos_x_block1--;
                    piece.pos_x_block2--;
                    piece.pos_x_block3--;
                    piece.pos_x_block4--;

                }
                Canvas.SetLeft(piece.getBlock1(), piece.left_block1);
                Canvas.SetLeft(piece.getBlock2(), piece.left_block2);
                Canvas.SetLeft(piece.getBlock3(), piece.left_block3);
                Canvas.SetLeft(piece.getBlock4(), piece.left_block4);

            }

            else if (e.Key == Key.Right && gameTimer.IsEnabled)
            {
                if (piece == null)
                    return;

                if (piece.left_block1 + game_piece_dx < 300 && board[piece.pos_x_block1 + 1][piece.pos_y_block1] != 2 &&
                    piece.left_block2 + game_piece_dx < 300 && board[piece.pos_x_block2 + 1][piece.pos_y_block2] != 2 &&
                    piece.left_block3 + game_piece_dx < 300 && board[piece.pos_x_block3 + 1][piece.pos_y_block3] != 2 &&
                    piece.left_block4 + game_piece_dx < 300 && board[piece.pos_x_block4 + 1][piece.pos_y_block4] != 2)
                {
                    piece.left_block1 += game_piece_dx;
                    piece.left_block2 += game_piece_dx;
                    piece.left_block3 += game_piece_dx;
                    piece.left_block4 += game_piece_dx;

                    board[piece.pos_x_block4][piece.pos_y_block4] = 0;
                    board[piece.pos_x_block4 + 1][piece.pos_y_block4] = 1;
                    board[piece.pos_x_block3][piece.pos_y_block3] = 0;
                    board[piece.pos_x_block3 + 1][piece.pos_y_block3] = 1;
                    board[piece.pos_x_block2][piece.pos_y_block2] = 0;
                    board[piece.pos_x_block2 + 1][piece.pos_y_block2] = 1;
                    board[piece.pos_x_block1][piece.pos_y_block1] = 0;
                    board[piece.pos_x_block1 + 1][piece.pos_y_block1] = 1;

                    /*board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "0";
                    board_rect[piece.pos_x_block4 + 1][piece.pos_y_block4].Content = "1";
                    board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "0";
                    board_rect[piece.pos_x_block3 + 1][piece.pos_y_block3].Content = "1";
                    board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "0";
                    board_rect[piece.pos_x_block2 + 1][piece.pos_y_block2].Content = "1";
                    board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "0";
                    board_rect[piece.pos_x_block1 + 1][piece.pos_y_block1].Content = "1";*/

                    piece.pos_x_block1++;
                    piece.pos_x_block2++;
                    piece.pos_x_block3++;
                    piece.pos_x_block4++;

                }
                Canvas.SetLeft(piece.getBlock1(), piece.left_block1);
                Canvas.SetLeft(piece.getBlock2(), piece.left_block2);
                Canvas.SetLeft(piece.getBlock3(), piece.left_block3);
                Canvas.SetLeft(piece.getBlock4(), piece.left_block4);
            }


            else if (e.Key == Key.Down && gameTimer.IsEnabled)
            {
                if (piece == null)
                    return;

                if (piece.top_block1 + 1 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block1][piece.pos_y_block1 + 1] != 2 &&
                    piece.top_block2 + 1 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block2][piece.pos_y_block2 + 1] != 2 &&
                    piece.top_block3 + 1 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block3][piece.pos_y_block3 + 1] != 2 &&
                    piece.top_block4 + 1 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block4][piece.pos_y_block4 + 1] != 2)
                {
                    piece.top_block1 += game_piece_dx;
                    piece.top_block2 += game_piece_dx;
                    piece.top_block3 += game_piece_dx;
                    piece.top_block4 += game_piece_dx;

                    board[piece.pos_x_block1][piece.pos_y_block1] = 0;
                    board[piece.pos_x_block1][piece.pos_y_block1 + 1] = 1;
                    board[piece.pos_x_block2][piece.pos_y_block2] = 0;
                    board[piece.pos_x_block2][piece.pos_y_block2 + 1] = 1;
                    board[piece.pos_x_block3][piece.pos_y_block3] = 0;
                    board[piece.pos_x_block3][piece.pos_y_block3 + 1] = 1;
                    board[piece.pos_x_block4][piece.pos_y_block4] = 0;
                    board[piece.pos_x_block4][piece.pos_y_block4 + 1] = 1;

                    /*board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "0";
                    board_rect[piece.pos_x_block1][piece.pos_y_block1 + 1].Content = "1";
                    board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "0";
                    board_rect[piece.pos_x_block2][piece.pos_y_block2 + 1].Content = "1";
                    board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "0";
                    board_rect[piece.pos_x_block3][piece.pos_y_block3 + 1].Content = "1";
                    board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "0";
                    board_rect[piece.pos_x_block4][piece.pos_y_block4 + 1].Content = "1";*/

                    piece.pos_y_block1++;
                    piece.pos_y_block2++;
                    piece.pos_y_block3++;
                    piece.pos_y_block4++;

                }
                Canvas.SetTop(piece.getBlock1(), piece.top_block1);
                Canvas.SetTop(piece.getBlock2(), piece.top_block2);
                Canvas.SetTop(piece.getBlock3(), piece.top_block3);
                Canvas.SetTop(piece.getBlock4(), piece.top_block4);
            }

            else if (e.Key == Key.Home)
            {
                Level();
            }

            else if (e.Key == Key.Up)
            {
                if (piece == null)
                    return;

                int x1 = 0;
                int x2 = 0;
                int x3 = 0;
                int x4 = 0;
                int y1 = 0;
                int y2 = 0;
                int y3 = 0;
                int y4 = 0;

                if (piece.type.Equals("line"))
                {
                    if (piece.position == 0)
                    {
                        x1 = 2;x2 = 1;x3 = 0;x4 = -1;
                        y1 = -1;y2 = 0;y3 = 1;y4 = 2;
                    }

                    else if (piece.position == 90)
                    {
                        x1 = 1;x2 = 0;x3 = -1;x4 = -2;
                        y1 = 2;y2 = 1;y3 = 0;y4 = -1;
                    }

                    else if (piece.position == 180)
                    {
                        x1 = -2;x2 = -1;x3 = 0;x4 = 1;
                        y1 = 1;y2 = 0; y3 = -1;y4 = -2;
                    }

                    else if (piece.position == 270)
                    {
                        x1 = -1;x2 = 0;x3 = 1;x4 = 2;
                        y1 = -2;y2 = -1;y3 = 0;y4 = 1;
                    }
                }

                else if (piece.type.Equals("left-l"))
                {
                    if (piece.position == 0)
                    {
                        x1 = 2;x2 = 1;x3 = 0;x4 = -1;
                        y1 = 0;y2 = -1;y3 = 0;y4 = 1;
                    }

                    else if (piece.position == 90)
                    {
                        x1 = 0;x2 = 1;x3 = 0;x4 = -1;
                        y1 = 2;y2 = 1;y3 = 0;y4 = -1;
                    }

                    else if (piece.position == 180)
                    {
                        x1 = -2;x2 = -1;x3 = 0;x4 = 1;
                        y1 = 0;y2 = 1; y3 = 0;y4 = -1;
                    }

                    else if (piece.position == 270)
                    {
                        x1 = 0;x2 = -1;x3 = 0;x4 = 1;
                        y1 = -2;y2 = -1;y3 = 0;y4 = 1;
                    }
                }

                else if (piece.type.Equals("right-l"))
                {
                    if (piece.position == 0)
                    {
                        x1 = 1;x2 = 0;x3 = -1;x4 = 0;
                        y1 = -1;y2 = 0;y3 = 1;y4 = 2;
                    }

                    else if (piece.position == 90)
                    {
                        x1 = 1;x2 = 0;x3 = -1;x4 = -2;
                        y1 = 1;y2 = 0;y3 = -1;y4 = 0;
                    }

                    else if (piece.position == 180)
                    {
                        x1 = -1;x2 = 0;x3 = 1;x4 = 0;
                        y1 = 1;y2 = 0; y3 = -1;y4 = -2;
                    }

                    else if (piece.position == 270)
                    {
                        x1 = -1;x2 = 0;x3 = 1;x4 = 2;
                        y1 = -1;y2 = 0;y3 = 1;y4 = 0;
                    }
                }

                else if (piece.type.Equals("s"))
                {
                    if (piece.position == 0)
                    {
                        x1 = 1;x2 = 0;x3 = 1;x4 = 0;
                        y1 = -1;y2 = 0;y3 = 1;y4 = 2;
                    }

                    else if (piece.position == 90)
                    {
                        x1 = 1;x2 = 0;x3 = -1;x4 = -2;
                        y1 = 1;y2 = 0;y3 = 1;y4 = 0;
                    }

                    else if (piece.position == 180)
                    {
                        x1 = -1;x2 = 0;x3 = -1;x4 = 0;
                        y1 = 1;y2 = 0;y3 = -1;y4 = -2;
                    }

                    else if (piece.position == 270)
                    {
                        x1 = -1;x2 = 0;x3 = 1;x4 = 2;
                        y1 = -1;y2 = 0;y3 = -1;y4 = 0;
                    }
                }

                else if (piece.type.Equals("z"))
                {
                    if (piece.position == 0)
                    {
                        x1 = 2;x2 = 1;x3 = 0;x4 = -1;
                        y1 = 0;y2 = 1;y3 = 0;y4 = 1;
                    }

                    else if (piece.position == 90)
                    {
                        x1 = 0;x2 = -1;x3 = 0;x4 = -1;
                        y1 = 2;y2 = 1;y3 = 0; y4 = -1;
                    }

                    else if (piece.position == 180)
                    {
                        x1 = -2;x2 = -1;x3 = 0;x4 = 1;
                        y1 = 0;y2 = -1;y3 = 0;y4 = -1;
                    }

                    else if (piece.position == 270)
                    {
                        x1 = 0;x2 = 1;x3 = 0;x4 = 1;
                        y1 = -2;y2 = -1;y3 = 0;y4 = 1;
                    }
                }

                else if (piece.type.Equals("t"))
                {
                    if (piece.position == 0)
                    {
                        x1 = 1;x2 = 0;x3 = 1;x4 = -1;
                        y1 = -1;y2 = 0;y3 = 1;y4 = 1;
                    }

                    else if (piece.position == 90)
                    {
                        x1 = 1;x2 = 0;x3 = -1;x4 = -1;
                        y1 = 1;y2 = 0;y3 = 1;y4 = -1;
                    }

                    else if (piece.position == 180)
                    {
                        x1 = -1;x2 = 0;x3 = -1;x4 = 1;
                        y1 = 1;y2 = 0;y3 = -1;y4 = -1;
                    }

                    else if (piece.position == 270)
                    {
                        x1 = -1;x2 = 0;x3 = 1;x4 = 1;
                        y1 = -1;y2 = 0;y3 = -1;y4 = 1;
                    }
                }

                if (piece.left_block1 + x1 * game_piece_dx >= 0 && piece.left_block1 + x1 * game_piece_dx < 300 && piece.top_block1 + y1 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block1 + x1][piece.pos_y_block1 + y1] != 2 &&
                    piece.left_block2 + x2 * game_piece_dx >= 0 && piece.left_block2 + x2 * game_piece_dx < 300 && piece.top_block2 + y2 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block2 + x2][piece.pos_y_block2 + y2] != 2 &&
                    piece.left_block3 + x3 * game_piece_dx >= 0 && piece.left_block3 + x3 * game_piece_dx < 300 && piece.top_block3 + y3 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block3 + x3][piece.pos_y_block3 + y3] != 2 &&
                    piece.left_block4 + x4 * game_piece_dx >= 0 && piece.left_block4 + x4 * game_piece_dx < 300 && piece.top_block4 + y4 * game_piece_dx < myGameCanvas.Height && board[piece.pos_x_block4 + x4][piece.pos_y_block4 + y4] != 2)
                {

                    piece.position += 90;
                    piece.position %= 360;

                    piece.left_block1 += x1 * game_piece_dx;
                    piece.left_block2 += x2 * game_piece_dx;
                    piece.left_block3 += x3 * game_piece_dx;
                    piece.left_block4 += x4 * game_piece_dx;

                    piece.top_block1 += y1 * game_piece_dx;
                    piece.top_block2 += y2 * game_piece_dx;
                    piece.top_block3 += y3 * game_piece_dx;
                    piece.top_block4 += y4 * game_piece_dx;

                    board[piece.pos_x_block1][piece.pos_y_block1] = 0;
                    board[piece.pos_x_block1 + x1][piece.pos_y_block1 + y1] = 1;
                    board[piece.pos_x_block2][piece.pos_y_block2] = 0;
                    board[piece.pos_x_block2 + x2][piece.pos_y_block2 + y2] = 1;
                    board[piece.pos_x_block3][piece.pos_y_block3] = 0;
                    board[piece.pos_x_block3 + x3][piece.pos_y_block3 + y3] = 1;
                    board[piece.pos_x_block4][piece.pos_y_block4] = 0;
                    board[piece.pos_x_block4 + x4][piece.pos_y_block4 + y4] = 1;

                    /*board_rect[piece.pos_x_block1][piece.pos_y_block1].Content = "0";
                    board_rect[piece.pos_x_block1 + x1][piece.pos_y_block1 + y1].Content = "1";
                    board_rect[piece.pos_x_block2][piece.pos_y_block2].Content = "0";
                    board_rect[piece.pos_x_block2 + x2][piece.pos_y_block2 + y2].Content = "1";
                    board_rect[piece.pos_x_block3][piece.pos_y_block3].Content = "0";
                    board_rect[piece.pos_x_block3 + x3][piece.pos_y_block3 + y3].Content = "1";
                    board_rect[piece.pos_x_block4][piece.pos_y_block4].Content = "0";
                    board_rect[piece.pos_x_block4 + x4][piece.pos_y_block4 + y4].Content = "1";*/

                    piece.pos_x_block1 += x1;
                    piece.pos_x_block2 += x2;
                    piece.pos_x_block3 += x3;
                    piece.pos_x_block4 += x4;

                    piece.pos_y_block1 += y1;
                    piece.pos_y_block2 += y2;
                    piece.pos_y_block3 += y3;
                    piece.pos_y_block4 += y4;
                }

                Canvas.SetTop(piece.getBlock1(), piece.top_block1);
                Canvas.SetTop(piece.getBlock2(), piece.top_block2);
                Canvas.SetTop(piece.getBlock3(), piece.top_block3);
                Canvas.SetTop(piece.getBlock4(), piece.top_block4);
                Canvas.SetLeft(piece.getBlock1(), piece.left_block1);
                Canvas.SetLeft(piece.getBlock2(), piece.left_block2);
                Canvas.SetLeft(piece.getBlock3(), piece.left_block3);
                Canvas.SetLeft(piece.getBlock4(), piece.left_block4);
            }

            else if (e.Key == Key.Space)
            {
                if (gameTimer.IsEnabled)
                    gameTimer.Stop();
                else
                    gameTimer.Start();
            }

            else if (e.Key == Key.R)
            {
                Restart();
            }

            else if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }

        }

        private void Menu_quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Menu_pause_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
        }

        private void Menu_start_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Start();
        }

        private void Menu_info_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();

            MessageBox.Show("Start/Stop: Space key\nRestart: R key\nExit: ESC key", "Useless Box");

        }

        private void Menu_restart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }
    }
}
