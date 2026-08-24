using System.Drawing;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Rectangle
    {
        public int _id;
        private CustomPoint _points;
        private float _horizontal;
        private float _vertical;
        private Bitmap _bitmap;
        private Color _color;

        public Rectangle(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _color = Color.Black;
            _horizontal = 20;
            _vertical = 10;
            _points = new CustomPoint();
            MessageBox.Show("Создан прямоугольник.");
        }

        public Rectangle(int id, Bitmap bitmap, Color color, int x, int y, int horizontal, int vertical)
        {
            _id = id;
            _bitmap = bitmap;
            _color = color;
            _horizontal = horizontal;
            _vertical = vertical;
            _points = new CustomPoint(getOffsetCoordinate(x, _bitmap.Width, _horizontal), 
                getOffsetCoordinate(y, _bitmap.Height, _vertical));
        }

        public float getHorizontal()
        {
            return _horizontal;
        }

        public float getVertical()
        {
            return _vertical;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void setSize(float horizontal, float vertical)
        {
            var extremePointX = _points.GetX() + horizontal;
            var extremePointY = _points.GetY() + vertical;

            if (extremePointX <= _bitmap.Width) 
            {
                if(horizontal > 0)
                {
                    _horizontal = horizontal;
                }
                else
                {
                    _horizontal = 0;
                }
            }
            else
            {
                var availableSpace = _bitmap.Width - 1 - _points.GetX() - _horizontal;
                _horizontal += availableSpace;
            }
            if(extremePointY <= _bitmap.Height)
            {
                if (vertical > 0)
                {
                    _vertical = vertical;
                }
                else
                {
                    _vertical = 0;
                }
            }
            else
            {
                var availableSpace = _bitmap.Height - 1 - _points.GetY() - _vertical;
                _vertical += availableSpace;
            }
        }

        public void MoveTo(int x, int y)
        {
            _points.MoveX(getOffsetCoordinate(x, _bitmap.Width - 1, _horizontal, _points.GetX()));
            _points.MoveY(getOffsetCoordinate(y, _bitmap.Height - 1, _vertical, _points.GetY()));
        }

        private float getOffsetCoordinate(float newOffset, float size, float side, float currentCoordinate = 0)
        {
            float offset = 0;
            //если x больше нуля, значит смещение вправо или вниз
            if (newOffset >= 0)
            {
                var extremePoint = currentCoordinate + newOffset + side;
                offset = size >= extremePoint ? newOffset : size - currentCoordinate - side;
            }
            //если x меньше нуля, то смещение влево или вверх
            else
            {
                var extremePoint = currentCoordinate + newOffset;
                //и если смещение больше нуля, то можно двигать,
                //иначе двигаем на ноль
                offset = extremePoint >= 0 ? newOffset : -currentCoordinate;
            }
            return offset;
        }

        public void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            PointF[] points = new PointF[]
            {
                new PointF(_points.GetX(), _points.GetY()), // левый верхний угол
                new PointF(_points.GetX(), _points.GetY() + _vertical), // левый нижний угол
                new PointF(_points.GetX() + _horizontal, _points.GetY() + _vertical), // правый нижний угол
                new PointF(_points.GetX() + _horizontal, _points.GetY()) // правый верхний угол
            };
            graphics.DrawPolygon(new Pen(_color), points);
        }
    }
}
