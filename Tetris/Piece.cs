using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    class Piece
    {
        public int pos_x_block1 {get; set; } //x position of left topmost piece
        public int pos_x_block2 { get; set; }
        public int pos_x_block3 { get; set; }
        public int pos_x_block4 { get; set; }

        public int pos_y_block1 { get; set; } //y position of left topmost piece
        public int pos_y_block2 { get; set; }
        public int pos_y_block3 { get; set; }
        public int pos_y_block4 { get; set; }

        public double left_block1 { get; set; }
        public double left_block2 { get; set; }
        public double left_block3 { get; set; }
        public double left_block4 { get; set; }

        public double top_block1 { get; set; }
        public double top_block2 { get; set; }
        public double top_block3 { get; set; }
        public double top_block4 { get; set; }

        Rectangle block1;
        Rectangle block2;
        Rectangle block3;
        Rectangle block4;

        public bool block1_exist { get; set; }
        public bool block2_exist { get; set; }
        public bool block3_exist { get; set; }
        public bool block4_exist { get; set; }


        private static int block_dimension = 30;
        private static double block_opacity = 1;

        public string type { get; set; }
        public int position { get; set; }
        public int size { get; private set; }

        public Piece(int pos_x, int pos_y, int num)
        {
            this.pos_x_block1 = pos_x;
            this.pos_y_block1 = pos_y;
            position = 0;
            size = 4;
            left_block1 = pos_x * block_dimension;
            top_block1 = pos_y * block_dimension;
            block1_exist = true;
            block2_exist = true;
            block3_exist = true;
            block4_exist = true;

            //LINE
            if (num == 1)
            {
                type = "line";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.Cyan;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x + 1;
                pos_y_block2 = pos_y;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 2;
                pos_y_block3 = pos_y;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x + 3;
                pos_y_block4 = pos_y;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }

            //Left-L
            else if (num == 2)
            {
                type = "left-l";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.DarkBlue;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x;
                pos_y_block2 = pos_y + 1;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 1;
                pos_y_block3 = pos_y + 1;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x + 2;
                pos_y_block4 = pos_y + 1;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }

            //Right-L
            else if (num == 3)
            {
                type = "right-l";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.Orange;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x + 1;
                pos_y_block2 = pos_y;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 2;
                pos_y_block3 = pos_y;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x + 2;
                pos_y_block4 = pos_y - 1;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }

            //S
            else if (num == 4)
            {
                type = "s";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.LimeGreen;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x + 1;
                pos_y_block2 = pos_y;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 1;
                pos_y_block3 = pos_y - 1;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x + 2;
                pos_y_block4 = pos_y - 1;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }

            //Z
            else if (num == 5)
            {
                type = "z";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.Red;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x + 1;
                pos_y_block2 = pos_y;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 1;
                pos_y_block3 = pos_y + 1;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x + 2;
                pos_y_block4 = pos_y + 1;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }

            //Square
            else if (num == 6)
            {
                type = "square";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.Yellow;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x + 1;
                pos_y_block2 = pos_y;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 1;
                pos_y_block3 = pos_y + 1;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x;
                pos_y_block4 = pos_y + 1;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }

            //T
            else if (num == 7)
            {
                type = "t";
                block1 = new Rectangle();
                setupBlock(block1);
                block1.Fill = Brushes.Purple;

                block2 = copyBlock(block1);
                pos_x_block2 = pos_x + 1;
                pos_y_block2 = pos_y;
                left_block2 = pos_x_block2 * block_dimension;
                top_block2 = pos_y_block2 * block_dimension;

                block3 = copyBlock(block1);
                pos_x_block3 = pos_x + 1;
                pos_y_block3 = pos_y - 1;
                left_block3 = pos_x_block3 * block_dimension;
                top_block3 = pos_y_block3 * block_dimension;

                block4 = copyBlock(block1);
                pos_x_block4 = pos_x + 2;
                pos_y_block4 = pos_y;
                left_block4 = pos_x_block4 * block_dimension;
                top_block4 = pos_y_block4 * block_dimension;
            }
        }

        private void setupBlock(Rectangle block)
        {
            block.Width = block_dimension;
            block.Height = block_dimension;
            block.Stroke = Brushes.White;
            block.StrokeThickness = 1;
            block.Opacity = block_opacity;
        }

        private Rectangle copyBlock(Rectangle block)
        {
            Rectangle new_block = new Rectangle();
            new_block.Width = block.Width;
            new_block.Height = block.Height;
            new_block.Fill = block.Fill;
            new_block.Stroke = block.Stroke;
            new_block.StrokeThickness = block.StrokeThickness;
            new_block.Opacity = block.Opacity;

            return new_block;
        }


        public Rectangle getBlock1()
        {
            return block1;
        }

        public Rectangle getBlock2()
        {
            return block2;
        }

        public Rectangle getBlock3()
        {
            return block3;
        }

        public Rectangle getBlock4()
        {
            return block4;
        }

        public void removeBlock1()
        {
            if (block1_exist)
            {
                size--;
                block1_exist = false;
            }
        }

        public void removeBlock2()
        {
            if (block2_exist)
            {
                size--;
                block2_exist = false;
            }
        }

        public void removeBlock3()
        {
            if (block3_exist)
            {
                size--;
                block3_exist = false;
            }
        }

        public void removeBlock4()
        {
            if (block4_exist)
            {
                size--;
                block4_exist = false;
            }
        }
    }
}
