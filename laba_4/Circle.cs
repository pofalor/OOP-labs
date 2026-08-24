using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Circle : TFigure
    {
        protected float _radius;

        public Circle(Bitmap bitmap) : base(bitmap)
        {
            _radius = 10;
            MessageBox.Show("Создана окружность.");
        }

        public Circle(int id, Bitmap bitmap, Color color, float x, float y, float radius) : base(id, bitmap, color, x, y)
        {
            _radius = radius;
        }

        public float getRadius()
        {
            return _radius;
        }

        public virtual void setRadius(float radius)
        {
            if(radius > 0)
            {
                var extremePointRight = _point.GetX() + radius;
                var extremePointLeft = _point.GetX() - radius;
                var extremePointUp = _point.GetY() + radius;
                var extremePointDown = _point.GetY() - radius;

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
                        _bitmap.Width - 1 - _point.GetX() - _radius,
                        _point.GetX() - _radius,
                        _bitmap.Height - 1 - _point.GetY() - _radius,
                        _point.GetY() - _radius,
                    };
                    var minSpace = spaces.Min();
                    _radius += minSpace;
                }
            }
            else
            {
                _radius = 0;
            }
            Draw();
        }

        public virtual float getOffsetCoordinate(float newOffset, bool isX, float ringSize = 0)
        {
            float offset = 0;
            var size = isX ? _bitmap.Width - 1 : _bitmap.Height - 1;
            var currentCoordinate = isX ? _point.GetX() : _point.GetY();
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

        public override void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            var x = _point.GetX() - _radius;
            var y = _point.GetY() - _radius;
            graphics.DrawEllipse(new Pen(_color), x, y, _radius * 2, _radius * 2);
        }

        public void Fill(Color color)
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            var x = _point.GetX() - _radius;
            var y = _point.GetY() - _radius;
            graphics.FillEllipse(new SolidBrush(color), x, y, _radius * 2, _radius * 2);
        }
    }
}
