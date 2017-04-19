using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _15Puzzle
{
    public class PuzzleForm : Form
    {
        public Tile[] tiles;
        int xOffset = 15, yOffset = 15, emptyPosition;

        public PuzzleForm()
        {
            this.Text = "15 Puzzle Game.";
            this.SetBounds(0, 0, 500, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            tiles = new Tile[16];
            int i = 0;

            foreach (string file in Directory.GetFiles("img", "*.png"))
            {
                Image img = Image.FromFile(file);
                tiles[i] = new Tile(img, i + 1, this);
                i++;
            }
            tiles[i] = new Tile(null, i + 1, this);
            randomize();    
        }

        public void randomize()
        {
            Tile temp;
            Random r = new Random();
            int index = 15;
            for (int i = 0; i <= 100000; i++)
            {
                int rInt = r.Next(0, 4);
                if (rInt == 0)  //Move Left.
                {
                    if ((index % 4) - 1 != -1)  //Checks to see if it's on the edge.
                    {
                        temp = tiles[index];
                        tiles[index] = tiles[index - 1];
                        tiles[index - 1] = temp;
                        index = index - 1;
                    }
                }
                else if (rInt == 1) //Move Up.
                {
                    if (index - 4 >= 0) //Edge Check.
                    {
                        temp = tiles[index];
                        tiles[index] = tiles[index - 4];
                        tiles[index - 4] = temp;
                        index = index - 4;
                    }
                }
                else if (rInt == 2) //Move right.
                {
                    if ((index % 4) + 1 != 4)   //Edge Check.
                    {
                        temp = tiles[index];
                        tiles[index] = tiles[index + 1];
                        tiles[index + 1] = temp;
                        index = index + 1;
                    }
                }
                else   //Move down.
                {
                    if (index + 4 <= 15)    //Edge Check
                    {
                        temp = tiles[index];
                        tiles[index] = tiles[index + 4];
                        tiles[index + 4] = temp;
                        index = index + 4;
                    }
                }
            }
            emptyPosition = index;
            //Console.WriteLine(emptyPosition);
            layoutTiles();
        }

        public void layoutTiles()   //Display the tiles array.
        {
            int i = 0, j = 0, k = 0;

            for (i = 0; i < tiles.Length; i++)
            {
                if (k == 4)
                {
                    k = 0;
                }
                j = i / 4;

                tiles[i].Image = tiles[i].getImage();
                tiles[i].Height = 110;
                tiles[i].Width = 110;
                tiles[i].Parent = this;
                tiles[i].SetBounds((k * 110) + xOffset, (j * 110) + yOffset, tiles[i].Width, tiles[i].Height);

                k++;
            }

            bool winCondition = true;

            for (int x = 0; x < tiles.Length - 1 && winCondition; x++)  //Display a solved message.
            {
                //Console.WriteLine(tiles[x].getValue());
                if (x + 1 == tiles[x].getValue())
                {
                    if (x == 14)
                    {
                        Console.WriteLine("Won");
                        this.Text = "15 Puzzle Game. Puzzle Solved!";
                    }
                }
                else
                {
                    this.Text = "15 Puzzle Game.";
                    winCondition = false;
                }
            }

        }
        public void adjacent(int index) //Move tiles method.
        {
            if ((index % 4) == (emptyPosition % 4)) //Can move up or down.
            {
                //Console.WriteLine(index + " " + emptyPosition);
                //Console.WriteLine(true);
                moveUpOrDown(index);
                return;
            }
            else
            {
                for (int i = index - 1; i >= 0 && (i % 4) < 3; i--)
                {
                    if (i == emptyPosition) //Move left.
                    {
                        //Console.WriteLine(index + " " + emptyPosition);
                        //Console.WriteLine("true 1");
                        moveLeft(index);
                        return;
                    }
                }
                for (int i = index + 1; i <= 15 && (i % 4) > 0; i++)
                {
                    if (i == emptyPosition) //Move right.
                    {
                        //Console.WriteLine(index + " " + emptyPosition);
                        //Console.WriteLine("true 2");
                        moveRight(index);
                        return;
                    }
                }
                //Console.WriteLine(index + " " + emptyPosition);
                //Console.WriteLine(false);
            }

        }

        public void moveUpOrDown(int index)
        {
            Tile temp;
            if (index < emptyPosition)  //Move Down
            {
                for (int i = emptyPosition; i > index; i -= 4)
                {
                    temp = tiles[i];
                    tiles[i] = tiles[i - 4];
                    tiles[i - 4] = temp;
                }
            }
            else    //Move up
            {
                for (int i = emptyPosition; i < index; i += 4)
                {
                    temp = tiles[i];
                    tiles[i] = tiles[i + 4];
                    tiles[i + 4] = temp;
                }
            }
            emptyPosition = index;
            layoutTiles();
        }

        public void moveLeft(int index)
        {
            Tile temp;
            //Console.WriteLine(index + " " + emptyPosition);
            for (int i = emptyPosition; i < index; i++)
            {
                temp = tiles[i];
                tiles[i] = tiles[i + 1];
                tiles[i + 1] = temp;
            }
            emptyPosition = index;
            layoutTiles();
        }

        public void moveRight(int index)
        {
            Tile temp;
            for (int i = emptyPosition; i > index; i--)
            {
                temp = tiles[i];
                tiles[i] = tiles[i - 1];
                tiles[i - 1] = temp;
            }
            emptyPosition = index;
            layoutTiles();
        }

        public static void Main()
        {	
            PuzzleForm f = new PuzzleForm();	// create Form
            Application.Run(f);		// special method to launch application
            // and provide event thread
        }
    }

    public class Tile : PictureBox
    {
        private Image img;
        private int value;
        private PuzzleForm form;

        public Tile(Image img, int index, PuzzleForm form)
        {
            this.img = img;
            this.value = index;
            this.form = form;
        }

        public Image getImage()
        {
            return this.img;
        }
        public int getValue(){
            return this.value;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int index = -1;
            for (int i = 0; i < form.tiles.Length; i++)
            {
                if (form.tiles[i] == this)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1 && img != null)
            {
                form.adjacent(index);
            }
            else
            {
                Console.WriteLine(false);
            }
        }
    }
}