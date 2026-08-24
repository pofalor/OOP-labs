using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LabSemestr_3
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Graphics graphics;
        Random random = new Random();
        BaseContainer container;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DestroyFigures(bool needShow = false)
        {
            if(container != null)
            {
                if (needShow)
                {
                    container.ActionWithFigures("Show");
                }
                else
                {
                    container.ActionWithFigures("Hide");
                }
                container.DestroyFigures();
                container = null;
                UpdateMove();
                MessageBox.Show("Контейнер уничтожен.");
            }
        }

        private void SetVisibleFigures(bool isVisible)
        {
            if(container != null)
            {
                graphics.Clear(pictureBox1.BackColor);
                if (isVisible)
                {
                    container.ActionWithFigures("Show");
                }
                else
                {
                    container.ActionWithFigures("Hide");
                }
                pictureBox1.Image = bitmap;
            }
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

            var resX = float.TryParse(stringX, out float x);
            var resY = float.TryParse(stringY, out float y);

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

            if (positionUpdated && container != null) container.ActionWithFigures("MoveFigure", selectedFigure, new float[2] { x, y });
            if (colorUpdated && container != null) container.ActionWithFigures("SetColor", selectedFigure, new float[1] { (float)color.ToKnownColor() });

            switch (selectedFigure)
            {
                case "Круг":
                        if (size1Updated && container != null) container.ActionWithFigures("SetSize", selectedFigure, new float[1] { size1 });
                    
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
                    if (size1Updated || verticalUpdated)
                    { 
                        size1 = size1Updated ? size1 : -1;
                        vertical = verticalUpdated ? vertical : -1;
                        if (container != null) container.ActionWithFigures("SetSize", selectedFigure, new float[2] { size1, vertical });
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
                    if (size1Updated || rightUpdated || leftHeightUpdated || rightHeightUpdated)
                    {
                        size1 = size1Updated ? size1 : -1;
                        right = rightUpdated ? right : -1;
                        leftHeight = leftHeightUpdated ? leftHeight : -1;
                        rightHeight = rightHeightUpdated ? rightHeight : -1;
                        if (container != null) 
                            container.ActionWithFigures("SetSize", selectedFigure, new float[4] { size1, right, leftHeight, rightHeight });
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
                    if (size1Updated && externalRadiusUpdated)
                    {
                        if(size1 > externalRadius)
                        {
                            MessageBox.Show("Внутренний радиус должен быть меньше внешнего.");
                            return;
                        }
                        if (container != null)
                            container.ActionWithFigures("SetSize", selectedFigure, new float[2] { size1, externalRadius });
                    }
                    else if(size1Updated || externalRadiusUpdated)
                    {
                        MessageBox.Show("Введите два радиуса.");
                        return;
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
                    if (size1Updated || heightUpdated)
                    {
                        size1 = size1Updated ? size1 : -1;
                        vertical = heightUpdated ? height : -1;
                        if (container != null)
                            container.ActionWithFigures("SetSize", selectedFigure, new float[2] { size1, height });
                    }
                    break;
            }
            UpdateMove();
        }

        private void UpdateMove()
        {
            graphics.Clear(pictureBox1.BackColor);
            if(container != null)
            {
                container.ActionWithFigures("MoveFigure", additionalArgs: new float[2] { 0, 0 });
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
            random = null;
            graphics.Dispose();
            bitmap.Dispose();
            if(container != null)
            {
                container.DestroyFigures();
            }
            container = null;
            System.GC.Collect();
        }

        #region Обработка кнопок в блоке контейнер
        private void buttonCreateArray_Click(object sender, EventArgs e)
        {
            var selectedType = this.comboBox3.SelectedItem ;
            if (selectedType == null)
            {
                MessageBox.Show("Выберите тип контейнера.");
                return;
            }

            switch (selectedType)
            {
                case "Лист":
                    container = new FigureContainer(bitmap);
                    break;
                case "Массив":
                    container = new ArrayContainer(bitmap);
                    break;
            }

            var countFigures = random.Next(20, 30);

            for (int i = 1; i <= countFigures; i++)
            {
                container.AddRandomFigure();
            }
            SetVisibleFigures(true);
            MessageBox.Show("Контейнер создан.");
        }

        private void buttonDestroyArray_Click(object sender, EventArgs e)
        {
            DestroyFigures(true);
        }

        private void buttonShowArray_Click(object sender, EventArgs e)
        {
            SetVisibleFigures(true);
        }

        private void buttonAddElement_Click(object sender, EventArgs e)
        {
            if(container != null)
            {
                container.AddRandomFigure();
                container.ActionWithFigures("Show");
                UpdateMove();
            }
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
            if(container != null)
            {
                container.ActionWithFigures("MoveFigure", selectedFigure, new float[2] { x, y });
                UpdateMove();
            }
        }
        #endregion
    }
}
