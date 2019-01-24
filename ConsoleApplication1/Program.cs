using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        
        static double h = 0.05;
        static double x0 = 0;
        static double x1 = 0.5;
        static double y0 = 0;
        static double eps = 0.000001;
        class Point
        {
            public double X;
            public double Y;
            public Point(double x_, double y_)
            {
                X = x_;
                Y = y_;
            }
            public override string ToString()
            {
                return X.ToString("f6") + "\t" + Y.ToString("f6");
            }
        }
        static double f(double x, double y)
        {
            return 1 + (1.5 - x) * Math.Sin(y) - (2 + x) * y;
        }

        static double delta_y(double x, double y)
        {
            return h * f(x + h / 2, y + h / 2 * f(x, y));
        }

        static double delta_y_r(double x, double y, double h_)
        {
            var k1 = f(x, y);
            var k2 = f(x + h_ / 2, y + h_ * k1 / 2);
            var k3 = f(x + h_ / 2, y + h_ * k2 / 2);
            var k4 = f(x + h_, y + h_ * k3);

            return h_ / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
        }

        static List<Point> eyler ()
        {
            List<Point> points = new List<Point>();
            var x = x0;
            var y = y0;
            while (x <= x1)
            {
                points.Add(new Point(x, y));
                x += h;
                y += h * f(x, y);
            }
            return points;
        }


        static List<Point> eyler_modified()
        {
            List<Point> points = new List<Point>();
            var x = x0;
            var y = y0;
            while (x <= x1)
            {
                points.Add(new Point(x, y));
                x += h;
                y += delta_y(x, y);
            }
            return points;
        }

        static List<Point> runge_kutta()
        {
            List<Point> points = new List<Point>();
            var x = x0;
            var y = y0;
            while (x <= x1)
            {
                points.Add(new Point(x, y));
                x += h;
                y += delta_y_r(x, y, h);
            }
            return points;
        }

        static double lenth(Point A, Point B)
        {
            return Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }
        static Point runge_kutta_minimal_step()
        {
            //List<Point> points = new List<Point>();
            var eps_s = h;
            Point last_point = null;
            var h_s = h;
            var x = x0;
            var y = y0;
            while (eps_s>eps)
            {
                x = x0;
                y = y0;
                while (x < x1)
                {
                    x += h_s;
                    y += delta_y_r(x, y, h_s);
                }
                if (last_point != null)
                {
                    eps_s = lenth(last_point, new Point(x, y));
                }
                last_point = new Point(x, y);
                h_s /= 2;
            }
            return new Point(h_s, eps_s);
        }

        static void list_points_view(List<Point> points)
        {
            if (points != null && points.Count() > 0)
            {
                Console.WriteLine("X\t\tY");
                foreach (var point in points)
                {
                    Console.WriteLine(point.ToString());
                }
            }
            else
            {
                Console.WriteLine("Нет значений для вывода!");
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Численные методы"); 
            Console.WriteLine("Приближённые вычисления уравнений первого порядка");
            Console.WriteLine("Начальные условия (10 вариант):");
            Console.WriteLine("y' = 1 + (1.5 - x) * sin(y) - (2 + x) * y");
            Console.WriteLine("h = 0.05, x0 = 0, x1 = 0.5, y(x0) = 0, eps = 0.000001");
            Console.WriteLine("Решение методом Эйлера:");
            list_points_view(eyler());
            Console.WriteLine("Решение модифицированным методом Эйлера:");
            list_points_view(eyler_modified());
            Console.WriteLine("Решение методом Рунге-Кутты 4го порядка:");
            list_points_view(runge_kutta());
            var h_eps = runge_kutta_minimal_step();
            Console.WriteLine("Заданная точность достигается при");
            Console.WriteLine("h = {0}, eps = {1}", h_eps.X.ToString("f8"), h_eps.Y.ToString("f8"));
            Console.ReadKey();
        }        
    }
}
