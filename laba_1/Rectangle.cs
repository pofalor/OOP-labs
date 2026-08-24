using System.Drawing;

namespace LabSemestr_3
{
    class Rectangle
    {
        private int _x;
        private int _y;
        private int _horizontal;
        private int _vertical;
        private Bitmap _bitmap;
        private Color _color;

        public Rectangle(Bitmap bitmap, Color color, int x, int y, int horizontal, int vertical)
        {
            _bitmap = bitmap;
            _color = color;
            _horizontal = horizontal;
            _vertical = vertical;
            _x = x;
            _y = y;
        }

        public int getHorizontal()
        {
            return _horizontal;
        }

        public int getVertical()
        {
            return _vertical;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void setSize(int horizontal, int vertical)
        {
            _horizontal = horizontal > 0 ? horizontal : 0;
            _vertical = vertical > 0 ? vertical : 0;
        }

        public void MoveTo(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            Point[] points = new Point[]
            {
                new Point(_x, _y), // левый верхний угол
                new Point(_x, _y + _vertical), // левый нижний угол
                new Point(_x + _horizontal, _y + _vertical), // правый нижний угол
                new Point(_x + _horizontal, _y) // правый верхний угол
            };
            graphics.DrawPolygon(new Pen(_color), points);
        }
    }
}
