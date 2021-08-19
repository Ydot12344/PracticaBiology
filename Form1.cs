using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Practica;
using System.IO;

namespace WindowsAutomat
{
    public partial class Form1 : Form
    {
        private List<long> stats;
        private Graphics graphics;
        private int resolution_x, resolution_y;
        private Automat automat;
        private int iter_num;
        public Form1()
        {
            InitializeComponent();
            ColLabel.Text = "Num: 0";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            stats.Add(automat.getCol());
            ColLabel.Text = "Num: " + automat.getCol();
            this.Text = iter_num.ToString();
            iter_num++;
            graphics.Clear(Color.Black);

            byte[,] area = automat.next();

            for(int x = 0; x < automat.height; x++)
                for(int y = 0; y < automat.width; y++)
                {
                    int tmp = 80;
                    if (area[x, y] == 0)
                        tmp = 0;
                    Brush color = new SolidBrush(Color.FromArgb(0, Math.Min(tmp + area[x, y], 255), 0));
                    graphics.FillRectangle(color, y * resolution_y, x * resolution_x, resolution_y, resolution_x);
                }
            pictureBox1.Refresh();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            stats = new List<long>();
            iter_num = 0;
            resolution_x = resolution_y = (int)numericResolution.Value;
            automat = new Automat((double)numdp.Value, (double)numbt.Value, (double)numcp.Value, pictureBox1.Width / resolution_y, pictureBox1.Height / resolution_x);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            String folder = String.Empty;
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    folder = folderBrowserDialog.SelectedPath;
                }
            }

            using (FileStream stream = new FileStream($"{folder}\\stats.csv", FileMode.OpenOrCreate))
            {
                String text = "";
                for (int i = 0; i < stats.Count; i++)
                {
                    if (i != stats.Count - 1)
                        text += i + ";";
                    else
                        text += i + "\n";
                }

                for(int i = 0; i < stats.Count; i++)
                {
                    if (i != stats.Count - 1)
                        text += stats[i] + ";";
                    else
                        text += stats[i] + "\n";
                }

                byte[] array = System.Text.Encoding.Default.GetBytes(text);

                stream.Write(array, 0, array.Length);
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

    }
}
