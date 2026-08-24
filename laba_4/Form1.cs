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
        List<Ring> rings = new List<Ring>();
        List<Ellipse> ellipses = new List<Ellipse>();
        int countFigures;
        Dictionary<int, CheckBox> checkBoxes = new Dictionary<int, CheckBox>();

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            checkBoxes.Add(1, checkBox_1);
            checkBoxes.Add(2, checkBox_2);
            checkBoxes.Add(3, checkBox_3);
            checkBoxes.Add(4, checkBox_4);
            checkBoxes.Add(5, checkBox_5);
            CreateFigures();
            UpdatePicture();
        }

        public void CreateFigures()
        {
            countFigures = random.Next(1, 5);

            int width = bitmap.Width - 50;
            int height = bitmap.Height - 50;

            for (int i = 1; i <= countFigures; i++)
            {
                var circle = new Circle(i, bitmap, Color.Green, random.Next(50, width), random.Next(50, height), random.Next(10, 50));
                var rectangle = new Rectangle(i, bitmap, Color.Blue, random.Next(50, width), random.Next(50, height), random.Next(10, 50), random.Next(10, 50));
                var triangle = new Triangle(i, bitmap, Color.Red, random.Next(50, width), random.Next(50, height), random.Next(10, 50),
                    random.Next(10, 50), random.Next(10, 50), random.Next(10, 50));
                var ring = new Ring(i, bitmap, Color.Yellow, random.Next(50, width), random.Next(50, height), random.Next(10, 30), random.Next(30, 60));
                var ellipse = new Ellipse(i, bitmap, Color.Black, random.Next(50, width), random.Next(50, height), random.Next(10, 50), random.Next(10, 50));
                circles.Add(circle);
                rectangles.Add(rectangle);
                triangles.Add(triangle);
                rings.Add(ring);
                ellipses.Add(ellipse);
                checkBoxes[i].Visible = true;
                checkBoxes[i].Checked = true;
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
                rings[i].Draw();
                ellipses[i].Draw();
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
            var resSize1 = float.TryParse(stringSize, out float size1) || !size1Updated;

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

            var selectedFigures = checkBoxes.Values.Where(z => z.Checked).Select(z => int.Parse(z.Name.Split('_')[1])).ToArray();
             
            switch (selectedFigure)
            {
                case "Круг":
                    var _circles = circles.Where(z=> selectedFigures.Contains(z._id)).ToArray();
                    foreach (var circle in _circles)
                    {
                        if (positionUpdated) circle.MoveTo(circle.getOffsetCoordinate(x, true), circle.getOffsetCoordinate(y, false));
                        if (colorUpdated) circle.SetColor(color);
                        if (size1Updated) circle.setRadius(size1);
                    }
                    break;
                case "Прямоугольник":
                    var verticalUpdated = !string.IsNullOrEmpty(this.textBox5.Text);
                    var stringVertical = this.textBox5.Text;
                    var resVertical = float.TryParse(stringVertical, out float vertical) || !verticalUpdated;
                    if (!resVertical)
                    {
                        MessageBox.Show("Введите целое число.");
                        return;
                    }
                    var _rectangles = rectangles.Where(z => selectedFigures.Contains(z._id)).ToArray();
                    foreach (var rectangle in _rectangles)
                    {
                        if (positionUpdated) 
                        {
                            var points = new[] 
                            {   
                                rectangle.GetDot(1), 
                                rectangle.GetDot(2), 
                                rectangle.GetDot(3), 
                                rectangle.GetDot(4) 
                            };
                            var arrayX = points.Select(z => z.GetX()).ToArray();
                            var arrayY = points.Select(z => z.GetY()).ToArray();

                            //если x больше нуля, значит смещение вправо или вниз, иначе вверх или влево
                            var extremeCoordinateX = x >= 0 ? arrayX.Max() : arrayX.Min();
                            //если y больше нуля, значит смещение вправо или вниз, иначе вверх или влево
                            var extremeCoordinateY = y >= 0 ? arrayY.Max() : arrayY.Min();

                            points[0].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
                            points[0].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));

                            points[1].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
                            points[1].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));

                            points[2].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
                            points[2].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));

                            points[3].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
                            points[3].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));
                        }
                        if (colorUpdated) rectangle.SetColor(color);
                        if (size1Updated || verticalUpdated)
                        { 
                            size1 = size1Updated ? size1 : rectangle.getHorizontal();
                            vertical = verticalUpdated ? vertical : rectangle.getVertical();
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
                    var resRight = float.TryParse(stringRight, out float right) || !rightUpdated;
                    var resLeftHeight = float.TryParse(stringLeftHeight, out float leftHeight) || !leftHeightUpdated;
                    var resRightHeight = float.TryParse(stringRightHeight, out float rightHeight) || !rightHeightUpdated;
                    if (!resRight || !resLeftHeight || !resRightHeight)
                    {
                        MessageBox.Show("Введите целое число.");
                        return;
                    }
                    var _triangles = triangles.Where(z => selectedFigures.Contains(z._id)).ToArray();
                    foreach (var triangle in _triangles)
                    {
                        if (positionUpdated) triangle.MoveTo(triangle.getOffsetCoordinate(x, true), triangle.getOffsetCoordinate(y, false));
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
                case "Кольцо":
                    var externalRadiusUpdated = !string.IsNullOrEmpty(this.textBox5.Text);
                    var stringExternalRadius = this.textBox5.Text;
                    var resExternalRadius = float.TryParse(stringExternalRadius, out float externalRadius) || !externalRadiusUpdated;
                    if (!resExternalRadius)
                    {
                        MessageBox.Show("Введите целое число.");
                        return;
                    }
                    var _rings = rings.Where(z => selectedFigures.Contains(z._id)).ToArray();
                    foreach (var ring in _rings)
                    {
                        if (positionUpdated) 
                        {
                            var ringSize = ring.GetRingSize();
                            var extCircle = ring.getExternalCircle();
                            var innCircle = ring.getInnerCircle();
                            extCircle.MoveTo(extCircle.getOffsetCoordinate(x, true), extCircle.getOffsetCoordinate(y, false));
                            innCircle.MoveTo(innCircle.getOffsetCoordinate(x, true, ringSize), innCircle.getOffsetCoordinate(y, false, ringSize));
                        }
                        if (colorUpdated) ring.SetColor(color);
                        if (size1Updated || externalRadiusUpdated)
                        {
                            size1 = size1Updated ? size1 : ring.getInnerRadius();
                            externalRadius = externalRadiusUpdated ? externalRadius : ring.getExternalRadius();
                            if(size1 > externalRadius)
                            {
                                MessageBox.Show("Внутренний радиус должен быть меньше внешнего.");
                                return;
                            }
                            ring.setSize(size1, externalRadius);
                        }
                    }
                    break;
                case "Эллипс":
                    var heightUpdated = !string.IsNullOrEmpty(this.textBox5.Text);
                    var stringHeight = this.textBox5.Text;
                    var resHeight = float.TryParse(stringHeight, out float height) || !heightUpdated;
                    if (!resHeight)
                    {
                        MessageBox.Show("Введите целое число.");
                        return;
                    }
                    var _ellipses = ellipses.Where(z => selectedFigures.Contains(z._id)).ToArray();
                    foreach (var ellipse in _ellipses)
                    {
                        if (positionUpdated) ellipse.MoveTo(ellipse.getOffsetCoordinate(x, true), ellipse.getOffsetCoordinate(y, false));
                        if (colorUpdated) ellipse.SetColor(color);
                        if (size1Updated || heightUpdated)
                        {
                            size1 = size1Updated ? size1 : ellipse.getRadius();
                            vertical = heightUpdated ? height : ellipse.getRadiusHeight();
                            ellipse.setSize(size1, vertical);
                        }
                    }
                    break;
            }
            MoveFigures();
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
                case "Кольцо":
                    this.label7.Text = "Внутренний радиус";
                    this.label8.Text = "Внешний радиус";
                    this.label7.Visible = true;
                    this.label8.Visible = true;
                    this.textBox4.Visible = true;
                    this.textBox5.Visible = true;

                    this.label9.Visible = false;
                    this.label10.Visible = false;
                    this.textBox6.Visible = false;
                    this.textBox7.Visible = false;
                    break;
                case "Эллипс":
                    this.label7.Text = "Радиус";
                    this.label8.Text = "Высота";
                    this.label7.Visible = true;
                    this.label8.Visible = true;
                    this.textBox4.Visible = true;
                    this.textBox5.Visible = true;

                    this.label9.Visible = false;
                    this.label10.Visible = false;
                    this.textBox6.Visible = false;
                    this.textBox7.Visible = false;
                    break;
            }
        }

        private void MoveFigures()
        {
            graphics.Clear(pictureBox1.BackColor);
            for (int i = 0; i < countFigures; i++)
            {
                circles[i].MoveTo(0, 0);
                rectangles[i].MoveTo(0, 0);
                triangles[i].MoveTo(0, 0);
                rings[i].SetColor(rings[i].GetColor());
                ellipses[i].MoveTo(0, 0);
            }
            pictureBox1.Image = bitmap;
        }

        private void Form1Closed(object sender, FormClosedEventArgs e)
        {
            circles = null;
            rectangles = null;
            triangles = null;
            random = null;
            graphics = null;
            bitmap = null;
            checkBoxes = null;
            rings = null;
            ellipses = null;
            System.GC.Collect();
        }
    }
}
