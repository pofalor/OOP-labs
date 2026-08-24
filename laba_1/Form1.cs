using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabSemestr_3
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Graphics graphics;
        Random random = new Random();
        List<Circle> circles = new List<Circle>();
        List<Rectangle> rectangles = new List<Rectangle>();
        List<Triangle> triangles = new List<Triangle>();
        int countFigures;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            CreateFigures();
            UpdatePicture();
        }

        public void CreateFigures()
        {
            countFigures = random.Next(1, 5);
            for (int i = 1; i <= countFigures; i++)
            {
                var circle = new Circle(bitmap, Color.Green, random.Next(10, 500), random.Next(10, 200), random.Next(10, 50));
                var rectangle = new Rectangle(bitmap, Color.Blue, random.Next(10, 500), random.Next(10, 200), random.Next(10, 50), random.Next(10, 50));
                var triangle = new Triangle(bitmap, Color.Red, random.Next(10, 500), random.Next(10, 200), random.Next(10, 50),
                    random.Next(10, 50), random.Next(10, 50), random.Next(10, 50));
                circles.Add(circle);
                rectangles.Add(rectangle);
                triangles.Add(triangle);
            }
        }

        private void UpdatePicture()
        {
            graphics.Clear(pictureBox1.BackColor);

            for (int i = 0; i < countFigures; i++)
            {
                circles[i].Draw();
                rectangles[i].Draw();
                triangles[i].Draw();
            }

            pictureBox1.Image = bitmap;
        }

        private void MoveFigures(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип фигуры.");
                return;
            }
            var selectedFigure = this.comboBox1.SelectedItem.ToString();

            var stringX = this.textBox1.Text;
            var stringY = this.textBox2.Text;

            var resX = int.TryParse(stringX, out int x);
            var resY = int.TryParse(stringY, out int y);

            bool size1Updated = !string.IsNullOrEmpty(this.textBox4.Text);
            var stringSize = this.textBox4.Text;
            var resSize1 = int.TryParse(stringSize, out int size1) || !size1Updated;

            if (!resX || !resY || !resSize1)
            {
                MessageBox.Show("Введите целое число.");
                return;
            }

            bool positionUpdated = this.textBox1.Text != "0" || this.textBox2.Text != "0";

            bool colorUpdated = this.comboBox2.SelectedItem != null;
            Color color = Color.Transparent;
            if (colorUpdated)
            {
                color = Color.FromName(this.comboBox2.SelectedItem.ToString());
            }

            switch (selectedFigure)
            {
                case "Круг":
                    foreach (var circle in circles)
                    {
                        if (positionUpdated) circle.MoveTo(x, y);
                        if (colorUpdated) circle.SetColor(color);
                        if (size1Updated) circle.setRadius(size1);
                    }
                    break;
                case "Прямоугольник":
                    var verticalUpdated = !string.IsNullOrEmpty(this.textBox5.Text);
                    var stringVertical = this.textBox5.Text;
                    var resVertical = int.TryParse(stringVertical, out int vertical) || !verticalUpdated;
                    if (!resVertical)
                    {
                        MessageBox.Show("Введите целое число.");
                        return;
                    }
                    foreach (var rectangle in rectangles)
                    {
                        if (positionUpdated) rectangle.MoveTo(x, y);
                        if (colorUpdated) rectangle.SetColor(color);
                        if (size1Updated || verticalUpdated)
                        { 
                            size1 = size1Updated ? size1 : rectangle.getHorizontal();
                            vertical = size1Updated ? vertical : rectangle.getVertical();
                            rectangle.setSize(size1, vertical);
                        }
                    }
                    break;
                    
                case "Треугольник":
                    var rightUpdated = !string.IsNullOrEmpty(this.textBox5.Text);
                    var leftHeightUpdated = !string.IsNullOrEmpty(this.textBox6.Text);
                    var rightHeightUpdated = !string.IsNullOrEmpty(this.textBox7.Text);
                    var stringRight = this.textBox5.Text;
                    var stringLeftHeight = this.textBox6.Text;
                    var stringRightHeight = this.textBox7.Text;
                    var resRight = int.TryParse(stringRight, out int right) || !rightUpdated;
                    var resLeftHeight = int.TryParse(stringLeftHeight, out int leftHeight) || !leftHeightUpdated;
                    var resRightHeight = int.TryParse(stringRightHeight, out int rightHeight) || !rightHeightUpdated;
                    if (!resRight || !resLeftHeight || !resRightHeight)
                    {
                        MessageBox.Show("Введите целое число.");
                        return;
                    }
                    foreach (var triangle in triangles)
                    {
                        if (positionUpdated) triangle.MoveTo(x, y);
                        if (colorUpdated) triangle.SetColor(color);
                        if (size1Updated || rightUpdated || leftHeightUpdated || rightHeightUpdated)
                        {
                            size1 = size1Updated ? size1 : triangle.getLeft();
                            right = rightUpdated ? right : triangle.getRight();
                            leftHeight = leftHeightUpdated ? leftHeight : triangle.getLeftHeight();
                            rightHeight = rightHeightUpdated ? rightHeight : triangle.getRightHeight();
                            triangle.setSize(size1, right, leftHeight, rightHeight);
                        }
                    }
                    break;
            }
            UpdatePicture();
        }

        private void GenerateValues(object sender, EventArgs e)
        {
            this.textBox1.Text = random.Next(-50, 50).ToString();
            this.textBox2.Text = random.Next(-50, 50).ToString();
        }

        private void CheckSelected(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedItem.ToString())
            {
                case "Круг":
                    this.label7.Text = "Радиус";
                    this.label7.Visible = true;
                    this.textBox4.Visible = true;

                    this.label8.Visible = false;
                    this.label9.Visible = false;
                    this.label10.Visible = false;
                    this.textBox5.Visible = false;
                    this.textBox6.Visible = false;
                    this.textBox7.Visible = false;
                    break;
                case "Прямоугольник":
                    this.label7.Text = "Горизонталь";
                    this.label8.Text = "Вертикаль";
                    this.label7.Visible = true;
                    this.label8.Visible = true;
                    this.textBox4.Visible = true;
                    this.textBox5.Visible = true;

                    this.label9.Visible = false;
                    this.label10.Visible = false;
                    this.textBox6.Visible = false;
                    this.textBox7.Visible = false;
                    break;
                case "Треугольник":
                    this.label7.Text = "Смещение по x левой точки";
                    this.label8.Text = "Смещение по x правой точки";
                    this.label7.Visible = true;
                    this.label8.Visible = true;
                    this.label9.Visible = true;
                    this.label10.Visible = true;
                    this.textBox4.Visible = true;
                    this.textBox5.Visible = true;
                    this.textBox6.Visible = true;
                    this.textBox7.Visible = true;
                    break;
            }
        }

        private void Form1Closed(object sender, FormClosedEventArgs e)
        {
            circles = null;
            rectangles = null;
            triangles = null;
            random = null;
            graphics = null;
            bitmap = null;
            System.GC.Collect();
        }
    }
}
