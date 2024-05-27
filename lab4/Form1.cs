using System;
using System.Windows.Forms;
using NCalc;

namespace lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //метод построения графика
        public void PlotFunction(double a, double b, double h)
        {
            try
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                chart1.Series[2].Points.Clear();
                chart1.Series[3].Points.Clear();

                chart1.Series[1].Points.AddXY(a, function(a));
                chart1.Series[1].Points.AddXY(a, 0);
                chart1.Series[2].Points.AddXY(b, function(b));
                chart1.Series[2].Points.AddXY(b, 0);
                chart1.Series[1].Points.AddXY(a, 0);
                chart1.Series[1].Points.AddXY(b, 0);
                while (a <= b)
                {
                    chart1.Series[0].Points.AddXY(a, function(a));
                    a += h;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при построении графика");
            }
        }

        //метод Трапеций
        double trapezoid(double a, double b, double h)
        {
            try
            {
                double res;
                double n = (b - a) / h;

                double sum = 0;
                for (int i = 1; i <= n - 1; i++)
                {
                    sum += function(a + h * i);
                }

                res = h * ((function(a) + function(b)) / 2 + sum);
                return res;
            }
            catch 
            {
                MessageBox.Show("Функция введена неверно");
                return 0;
            }
        }

        //Метод Симпсона
        double simpson(double a, double b, double h)
        {
            try
            {
                double res;
                double n = (b - a) / h;

                double sum2 = 0;
                double sum4 = 0;
                for (int i = 1; i <= n / 2; i++)
                {
                    sum4 += function(a + h * (2 * i - 1));
                }

                for (int i = 1; i <= n / 2 - 1; i++)
                {
                    sum2 += function(a + h * (2 * i));
                }

                res = h / 3 * (function(a) + 4 * sum4 + 2 * sum2 + function(b));
                return res;
            }
            catch 
            {
                MessageBox.Show("Функция введена неверно");
                return 0;
            }
        }

        //Метод, который считывает введённую пользователем функцию и возвращает результат, в зависимости от x
        public double function(double x)
        {
            string function = textBox1.Text;
            Expression y = new Expression(function);
            y.Parameters["x"] = x;
            object res = y.Evaluate();
            return Convert.ToDouble(res); 
        }

        //Метод получения информации из Edit
        public void getData(ref double a, ref double b, ref double h)
        {
            try
            {
                a = Convert.ToDouble(textBox3.Text);
                b = Convert.ToDouble(textBox4.Text);
                h = Convert.ToDouble(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Пределы или шаг интегрирования заполнены неверно");
            }
        }

        //Заполнение данных по варианту
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Exp(-0.4*Cos(1/x))";
            textBox2.Text = "0,4";
            textBox3.Text = "1";
            textBox4.Text = "2,6";
        }

        //Метод решения интеграла заданным методом
        private void button1_Click(object sender, EventArgs e)
        {
            double a = 0, b = 0, h = 0, answer = 0;
            double errorRate = 0;
            getData(ref a, ref b, ref h); // получение введённых данных

            //Проверка на выбранную функцию и решение заданным методом
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Выберите метод интегрирования");
            }
            else
            {
                PlotFunction(a, b, h);

                if (radioButton1.Checked)
                {
                    answer = trapezoid(a, b, h);
                }
                if (radioButton2.Checked)
                {
                    answer = simpson(a, b, h);
                }
            }
            label2.Text = "Ответ: " + Math.Round(answer, 6).ToString();
        }
    }
}
