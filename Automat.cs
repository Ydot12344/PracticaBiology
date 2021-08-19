using System;

namespace Practica
{
    public class Automat
    {

        private static Random rnd = new Random();
        public int width, height;
        private byte[,] area;
        private double deathProbability;
        private double birthProbability;
        private double competePower;
        private int cnt = 0;

        public Automat(double dp, double bp, double cp, int width, int height)
        {
            area = new byte[height, width];
            this.width = width;
            this.height = height;
            deathProbability = dp;
            birthProbability = bp;
            competePower = cp;
            init();
        }

        private void init()
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    area[i, j] = (byte)rnd.Next(2);
                    cnt += area[i, j];
                }
        }

        private double w(int x)
        {
            double exp = -0.5 * (3 * x * x + 4 * x * x * x * x) / (1 + x * x);
            return Math.Pow(Math.E, exp);
        }

        private int NormalRnd()
        {
            int times = 10;
            double avr = 0.5;
            double sigma = Math.Sqrt(1.0 / 12.0);
            double sum = 0;
            for (int i = 0; i < times; i++)
            {
                sum += rnd.NextDouble();
            }

            return (int)Math.Round((sum - avr * times) / (sigma * Math.Sqrt(times)));
        }

        public byte[,] next()
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int tmp = area[i, j];
                    for (int t = 0; t < tmp; t++)
                    {
                        if (rnd.NextDouble() <= birthProbability)
                        {
                            int del_x = NormalRnd();
                            int del_y = NormalRnd();

                            int x = j + del_x;
                            int y = i + del_y;

                            if (x >= 0 && y >= 0 && x < width && y < height)
                            {
                                area[y, x] += 1;
                                cnt++;
                            }
                        }

                        double pr = 0;
                        bool fl = true;
                        for (int a = i - 1; a <= i + 1 && fl; a++)
                            for (int b = j - 1; b <= j + 1 && fl; b++)
                            {
                                if ((a == i && b == j) || a < 0 || b < 0 || a == height || b == width)
                                    continue;
                                if (area[a, b] != 0)
                                    pr += w(1);
                            }
                        pr *= competePower;
                        pr += deathProbability;

                        if (rnd.NextDouble() <= pr)
                        {
                            area[i, j]--;
                            cnt--;
                        }
                    }
                }

            return area;
        }

        public int getCol()
        {
            return cnt;
        }
    }
}
