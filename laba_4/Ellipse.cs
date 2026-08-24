using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Ellipse : Circle
    {
        protected float _radiusHeight;
        public Ellipse(Bitmap bitmap) : base(bitmap)
        {
            _radiusHeight = 5;
            MessageBox.Show("Создан эллипс.");
        }

        public Ellipse(int id, Bitmap bitmap, Color color, float x, float y, float radius, float radiusHeight) : base(id, bitmap, color, x, y, radius)
        {
            _radiusHeight = radiusHeight;
        }

        public float getRadiusHeight()
        {
            return _radiusHeight;
        }

        public void setSize(float radius, float heightRadius)
        {
            if (radius > 0 && heightRadius > 0)
            {
                var extremePointRight = _point.GetX() + radius;
                var extremePointLeft = _point.GetX() - radius;
                var extremePointUp = _point.GetY() + heightRadius;
                var extremePointDown = _point.GetY() - heightRadius;

                var availableSpaceRight = _bitmap.Width - extremePointRight;
                var availableSpaceUp = _bitmap.Height - extremePointUp;

                var canChangeRadius = availableSpaceRight >= radius  && extremePointLeft >= 0;

                var canChangeHeight = availableSpaceUp >= heightRadius && extremePointDown >= 0;

                if (canChangeRadius)
                {
                    _radius = radius;
                }
                else
                {
                    var spaces = new List<float>
                    {
                        _bitmap.Width - 1 - _point.GetX() - _radius,
                        _point.GetX() - _radius
                    };
                    var minSpace = spaces.Min();
                    _radius += minSpace;
                }
                if (canChangeHeight)
                {
                    _radiusHeight = heightRadius;
                }
                else
                {
                    var spaces = new List<float>
                    {
                        _bitmap.Height - 1 - _point.GetY() - _radiusHeight,
                        _point.GetY() - _radiusHeight
                    };
                    var minSpace = spaces.Min();
                    _radiusHeight += minSpace;
                }
            }
            else
            {
                _radius = 0;
                _radiusHeight = 0;
            }
            Draw();
        }

        public override float getOffsetCoordinate(float newOffset, bool isX, float ringSize = 0)
        {
            float offset = 0;
            float windowSize = (isX ? _bitmap.Width : _bitmap.Height) - 1;
            float radius = isX ? _radius : _radiusHeight;
            var currentCoordinate = isX ? _point.GetX(): _point.GetY();
            //если x больше нуля, значит смещение вправо или вниз
            if (newOffset >= 0)
            {
                var extremePoint = currentCoordinate + newOffset + radius;
                offset = windowSize >= extremePoint ? newOffset : windowSize - currentCoordinate - radius;
            }
            //если x меньше нуля, то смещение влево или вверх
            else
            {
                var extremePoint = currentCoordinate + newOffset - radius;
                //и если смещение больше нуля, то можно двигать,
                //иначе двигаем на ноль
                offset = extremePoint >= 0 ? newOffset : -(currentCoordinate - radius);
            }
            return offset;
        }

        public override void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            var x = _point.GetX() - _radius;
            var y = _point.GetY() - _radiusHeight;
            graphics.DrawEllipse(new Pen(_color), x, y, _radius * 2, _radiusHeight * 2);
        }
    }
}
