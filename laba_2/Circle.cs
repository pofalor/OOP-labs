using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Circle
    {
        public int _id;
        private CustomPoint _points;
        private float _radius;
        private Bitmap _bitmap;
        private Color _color;

        public Circle(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _color = Color.Black;
            _radius = 10;
            _points = new CustomPoint();
            MessageBox.Show("Создана окружность.");
        }

        public Circle(int id, Bitmap bitmap, Color color, float x, float y, float radius)
        {
            _id = id;
            _bitmap = bitmap;
            _color = color;
            _radius = radius;
            _points = new CustomPoint(getOffsetCoordinate(x, _bitmap.Width), getOffsetCoordinate(y, _bitmap.Height));
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public float getRadius()
        {
            return _radius;
        }

        public void setRadius(float radius)
        {
            if(radius > 0)
            {
                var extremePointRight = _points.GetX() + radius;
                var extremePointLeft = _points.GetX() - radius;
                var extremePointUp = _points.GetY() + radius;
                var extremePointDown = _points.GetY() - radius;

                var availableSpaceRight = _bitmap.Width - extremePointRight;
                var availableSpaceUp = _bitmap.Height - extremePointUp;

                var canChangeRadius = availableSpaceRight >= radius && availableSpaceUp >= radius &&
                    extremePointLeft >= 0 && extremePointDown >= 0;

                if (canChangeRadius)
                {
                    _radius = radius;
                }
                else
                {
                    var spaces = new List<float>
                    {
                        _bitmap.Width - 1 - _points.GetX() - _radius,
                        _points.GetX() - _radius,
                        _bitmap.Height - 1 - _points.GetY() - _radius,
                        _points.GetY() - _radius,
                    };
                    var minSpace = spaces.Min();
                    _radius += minSpace;
                }
            }
            else
            {
                _radius = 0;
            }
        }

        private float getOffsetCoordinate(float newOffset, float size, float currentCoordinate = 0, float ringSize = 0)
        {
            float offset = 0;
            //если x больше нуля, значит смещение вправо или вниз
            if (newOffset >= 0)
            {
                var extremePoint = currentCoordinate + newOffset + _radius + ringSize;
                offset = size >= extremePoint ? newOffset : size - currentCoordinate - _radius - ringSize;
            }
            //если x меньше нуля, то смещение влево или вверх
            else
            {
                var extremePoint = currentCoordinate + newOffset - _radius - ringSize;
                //и если смещение больше нуля, то можно двигать,
                //иначе двигаем на ноль
                offset = extremePoint >= 0 ? newOffset : -(currentCoordinate - _radius - ringSize);
            }
            return offset;
        }

        public void MoveTo(float x, float y, float ringSize = 0)
        {
            _points.MoveX(getOffsetCoordinate(x, _bitmap.Width - 1, _points.GetX(), ringSize));
            _points.MoveY(getOffsetCoordinate(y, _bitmap.Height - 1, _points.GetY(), ringSize));
        }

        public void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            var x = _points.GetX() - _radius;
            var y = _points.GetY() - _radius;
            graphics.DrawEllipse(new Pen(_color), x, y, _radius * 2, _radius * 2);
        }

        public void Fill(Color color)
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            var x = _points.GetX() - _radius;
            var y = _points.GetY() - _radius;
            graphics.FillEllipse(new SolidBrush(color), x, y, _radius * 2, _radius * 2);
        }
    }
}
