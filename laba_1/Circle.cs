using System.Drawing;

namespace LabSemestr_3
{
    class Circle
    {
        private int _x;
        private int _y;
        private int _radius;
        private Bitmap _bitmap;
        private Color _color;

        public Circle(Bitmap bitmap, Color color, int x, int y, int radius)
        {
            _bitmap = bitmap;
            _color = color;
            _radius = radius;
            _x = x;
            _y = y;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void setRadius(int radius)
        {
            _radius = radius > 0 ? radius : 0;
        }

        public void MoveTo(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            graphics.DrawEllipse(new Pen(_color), _x, _y, _radius * 2, _radius * 2);
        }
    }
}
