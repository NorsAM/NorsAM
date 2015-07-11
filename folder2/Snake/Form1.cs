using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Snake _snake;
        private Snake.EDirection _direction;
        private bool _flag;
        
        private void Initialize()
        {
            _direction = Snake.EDirection.E;
            var size = new Size(70, 50);
            _snake = new Snake(3, size);
            picturebox.Size = Visualizer.MapSize = new Size(size.Width * Visualizer.CellSize, size.Height * Visualizer.CellSize);
            picturebox.Image = Visualizer.Paint(_snake.GetCoords());
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            _flag = false;
            List<Visualizer.Terrain> list;
            if (_snake.Move(_direction,out list))
            {
                timer.Stop();
                if (MessageBox.Show("Game Over\n\n Заново?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Initialize();
                }
                else Application.Exit();
            }
            else picturebox.Image = Visualizer.PaintOnlySomething(list);
            label1.Text = "Score: " + Snake.Score;
        }

        private void picturebox_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (_flag) return;
            _flag = true;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (_direction != Snake.EDirection.S) _direction = Snake.EDirection.N;
                    break;
                case Keys.Down:
                    if (_direction != Snake.EDirection.N) _direction = Snake.EDirection.S;
                    break;
                case Keys.Left:
                    if (_direction != Snake.EDirection.E) _direction = Snake.EDirection.W;
                    break;
                case Keys.Right:
                    if (_direction != Snake.EDirection.W) _direction = Snake.EDirection.E;
                    break;
            }
        }
    }
}
