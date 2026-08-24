using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace LabSemestr_3
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Graphics graphics;
        Random random = new Random();
        List<TFigure> figures = new List<TFigure>();
        int countFigures;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
           
            UpdatePicture();
        }

        public void CreateFigures()
        {
            countFigures = random.Next(20, 30);

            int width = bitmap.Width - 50;
            int height = bitmap.Height - 50;

            for (int i = 1; i <= countFigures; i++)
            {
                var figureType = random.Next(5);
               
                var randomColor = Color.FromKnownColor((KnownColor)random.Next(1, 175));
                switch (figureType)
                {
                    case 0:
                        figures.Add(new Circle(i, bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50), false));
                        break;
                    case 1:
                        figures.Add(new Rectangle(i, bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50), random.Next(10, 50), false));
                        break;
                    case 2:
                        figures.Add(new Triangle(i, bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50),
                        random.Next(10, 50), random.Next(10, 50), random.Next(10, 50), false));
                        break;
                    case 3:
                        figures.Add(new Ring(i, bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 30), random.Next(30, 60), false));
                        break;
                    case 4:
                        figures.Add(new Ellipse(i, bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50), random.Next(10, 50), false));
                        break;
                }
            }
        }

        public void DestroyFigures(bool needShow = false)
        {
            SetVisibleFigures(needShow);
            figures = new List<TFigure>();
            UpdateMove();
        }

        private void SetVisibleFigures(bool isVisible)
        {
            graphics.Clear(pictureBox1.BackColor);
            foreach (var figure in figures)
            {
                figure.SetVisible(isVisible);
            }
            pictureBox1.Image = bitmap;
        }

        private void UpdatePicture()
        {
            graphics.Clear(pictureBox1.BackColor);

            foreach (var figure in figures)
            {
                figure.Draw();
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
            if (figures == null && !figures.Any())
            {
                MessageBox.Show("Заполните массив.");
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
             
            switch (selectedFigure)
            {
                case "Круг":
                    var _circles = figures.Where(z => z is Circle && !(z is Ellipse)).Select(z => z as Circle).ToArray();
                    foreach (var circle in _circles)
                    {
                        if (positionUpdated) MoveCircle(circle, x, y);
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
                    var _rectangles = figures.Where(z => z is Rectangle).Select(z => z as Rectangle).ToArray();
                    foreach (var rectangle in _rectangles)
                    {
                        if (positionUpdated) MoveRectangle(rectangle, x, y);
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
                    var _triangles = figures.Where(z => z is Triangle).Select(z => z as Triangle).ToArray();
                    foreach (var triangle in _triangles)
                    {
                        if (positionUpdated) MoveTriangle(triangle, x, y);
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
                    var _rings = figures.Where(z => z is Ring).Select(z => z as Ring).ToArray();
                    foreach (var ring in _rings)
                    {
                        if (positionUpdated) MoveRing(ring, x, y);
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
                    var _ellipses = figures.Where(z => z is Ellipse).Select(z => z as Ellipse).ToArray();
                    foreach (var ellipse in _ellipses)
                    {
                        if (positionUpdated) MoveEllipse(ellipse, x, y);
                        if (colorUpdated) ellipse.SetColor(color);
                        if (size1Updated || heightUpdated)
                        {
                            size1 = size1Updated ? size1 : ellipse.getRadius();
                            vertical = heightUpdated ? height : ellipse.getRadiusHeight();
                            ellipse.setSize(size1, vertical);
                        }
                    }
                    break;
                case "Все": 
                    foreach (var figure in figures)
                    {
                        if (figure is Ellipse) MoveEllipse(figure as Ellipse, x, y);
                        else if (figure is Rectangle) MoveRectangle(figure as Rectangle, x, y);
                        else if (figure is Triangle) MoveTriangle(figure as Triangle, x, y);
                        else if (figure is Ring) MoveRing(figure as Ring, x, y);
                        else if (figure is Circle) MoveCircle(figure as Circle, x, y);

                        if (colorUpdated) figure.SetColor(color);
                    }
                    break;
            }
            UpdateMove();
        }

        private void UpdateMove()
        {
            graphics.Clear(pictureBox1.BackColor);

            if(figures != null)
            {
                foreach (var figure in figures)
                {
                    figure.MoveTo(0, 0);
                }
            }
            pictureBox1.Image = bitmap;
        }

        private void GenerateValues(object sender, EventArgs e)
        {
            var selectedFigure = this.comboBox1.SelectedItem?.ToString();

            this.textBox1.Text = random.Next(-50, 50).ToString();
            this.textBox2.Text = random.Next(-50, 50).ToString();
           
            if (selectedFigure == "Кольцо")
            {
                this.textBox4.Text = random.Next(10, 30).ToString();
                this.textBox5.Text = random.Next(30, 60).ToString();
            }
            else
            {
                this.textBox4.Text = random.Next(10, 50).ToString();
                this.textBox5.Text = random.Next(10, 50).ToString();
            }

            this.textBox6.Text = random.Next(10, 50).ToString();
            this.textBox7.Text = random.Next(10, 50).ToString();
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
                case "Все":
                    this.label7.Visible = false;
                    this.textBox4.Visible = false;
                    this.label8.Visible = false;
                    this.label9.Visible = false;
                    this.label10.Visible = false;
                    this.textBox5.Visible = false;
                    this.textBox6.Visible = false;
                    this.textBox7.Visible = false;
                    break;
            }
        }

        private void Form1Closed(object sender, FormClosedEventArgs e)
        {
            DestroyFigures();
            random = null;
            graphics = null;
            bitmap = null;
            System.GC.Collect();
        }

        #region Методы перемещения каждой из фигур
        private void MoveCircle(Circle circle, int x, int y)
        {
            circle.MoveTo(circle.getOffsetCoordinate(x, true), circle.getOffsetCoordinate(y, false));
        }
        private void MoveRectangle(Rectangle rectangle, int x, int y)
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

        private void MoveTriangle(Triangle triangle, int x, int y)
        {
            triangle.MoveTo(triangle.getOffsetCoordinate(x, true), triangle.getOffsetCoordinate(y, false));
        }

        private void MoveRing(Ring ring, int x, int y)
        {
            var ringSize = ring.GetRingSize();
            var extCircle = ring.getExternalCircle();
            var innCircle = ring.getInnerCircle();
            extCircle.MoveTo(extCircle.getOffsetCoordinate(x, true), extCircle.getOffsetCoordinate(y, false));
            innCircle.MoveTo(innCircle.getOffsetCoordinate(x, true, ringSize), innCircle.getOffsetCoordinate(y, false, ringSize));
        }

        private void MoveEllipse(Ellipse ellipse, int x, int y)
        {
            ellipse.MoveTo(ellipse.getOffsetCoordinate(x, true), ellipse.getOffsetCoordinate(y, false));
        }
        #endregion

        #region Обработка кнопок в блоке массив
        private void buttonCreateArray_Click(object sender, EventArgs e)
        {
            CreateFigures();
        }

        private void buttonDestroyArray_Click(object sender, EventArgs e)
        {
            DestroyFigures(true);
        }

        private void buttonShowArray_Click(object sender, EventArgs e)
        {
            SetVisibleFigures(true);
        }

        private void buttonHideArray_Click(object sender, EventArgs e)
        {
            SetVisibleFigures(false);
        }
        #endregion

        #region Обработка нажатия кнопок на форме
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var keysNeedProcess = new List<Keys>
            {
                Keys.Right,
                Keys.D,
                Keys.Left,
                Keys.A,
                Keys.Down,
                Keys.S,
                Keys.Up,
                Keys.W
            };
            if (!msg.HWnd.Equals(this.Handle) &&
                keysNeedProcess.Contains(keyData))
            {
                Form1_KeyDown(keyData);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyDown(Keys keyData)
        {
            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип фигуры.");
                return;
            }
            var selectedFigure = this.comboBox1.SelectedItem.ToString();
            int x = 0;
            int y = 0;

            switch (keyData)
            {
                case Keys.Up:
                case Keys.W:
                    y = -10;
                    break;
                case Keys.Down:
                case Keys.S:
                    y = 10;
                    break;
                case Keys.Left:
                case Keys.A:
                    x = -10;
                    break;
                case Keys.Right:
                case Keys.D:
                    x = 10;
                    break;
                default:
                    return;
            }

            switch (selectedFigure)
            {
                case "Круг":
                    var _circles = figures.Where(z => z is Circle && !(z is Ellipse)).Select(z => z as Circle).ToArray();
                    foreach (var circle in _circles)
                    {
                        MoveCircle(circle, x, y);
                    }
                    break;
                case "Прямоугольник":
                    var _rectangles = figures.Where(z => z is Rectangle).Select(z => z as Rectangle).ToArray();
                    foreach (var rectangle in _rectangles)
                    {
                        MoveRectangle(rectangle as Rectangle, x, y);
                    }
                    break;

                case "Треугольник":
                    var _triangles = figures.Where(z => z is Triangle).Select(z => z as Triangle).ToArray();
                    foreach (var triangle in _triangles)
                    {
                        MoveTriangle(triangle as Triangle, x, y);
                    }
                    break;
                case "Кольцо":
                    var _rings = figures.Where(z => z is Ring).Select(z => z as Ring).ToArray();
                    foreach (var ring in _rings)
                    {
                        MoveRing(ring as Ring, x, y);
                    }
                    break;
                case "Эллипс":
                    var _ellipses = figures.Where(z => z is Ellipse).Select(z => z as Ellipse).ToArray();
                    foreach (var ellipse in _ellipses)
                    {
                        MoveEllipse(ellipse as Ellipse, x, y);
                    }
                    break;
                case "Все":
                    foreach (var figure in figures)
                    {
                        if (figure is Ellipse) MoveEllipse(figure as Ellipse, x, y);
                        else if (figure is Rectangle) MoveRectangle(figure as Rectangle, x, y);
                        else if (figure is Triangle) MoveTriangle(figure as Triangle, x, y);
                        else if (figure is Ring) MoveRing(figure as Ring, x, y);
                        else if (figure is Circle) MoveCircle(figure as Circle, x, y);
                    }
                    break;
            }
            UpdateMove();
        }
        #endregion
    }
}
