using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Triangle : TFigure
    {
        protected float _left;
        protected float _right;
        protected float _leftHeight;
        protected float _rightHeight;

        public Triangle(Bitmap bitmap) : base(bitmap) 
        {
            _left = 10;
            _right = 20;
            _leftHeight = 5;
            _rightHeight = 30;
            MessageBox.Show("Создан треугольник.");
        }

        public Triangle(Bitmap bitmap, Color color, int x, int y, int left, int right, int leftHeight, int rightHeight, bool isVisible) :
            base(bitmap, color, x, y, isVisible)
        {
            _left = left;
            _right = right;
            _leftHeight = leftHeight;
            _rightHeight = rightHeight;
        }

        public float getLeft()
        {
            return _left;
        }

        public float getRight()
        {
            return _left;
        }
        public float getLeftHeight()
        {
            return _left;
        }
        public float getRightHeight()
        {
            return _left;
        }

        public void setSize(float left, float right, float leftHeight, float rightHeight)
        {
            _left = left > 0 ? left : 0;
            _right = right > 0 ? right : 0;
            _leftHeight = leftHeight > 0 ? leftHeight : 0;
            _rightHeight = rightHeight > 0 ? rightHeight : 0;

            var extremePointXL = _point.GetX() - left;
            var extremePointXR = _point.GetX() + right;
            var extremePointYL = _point.GetY() + leftHeight;
            var extremePointYR = _point.GetY() + rightHeight;

            if (extremePointXL >= 0)
            {
                if (left > 0)
                {
                    _left = left;
                }
                else
                {
                    _left = 0;
                }
            }
            else
            {
                var availableSpace = _point.GetX() - 1;
                _left = availableSpace;
            }
            if (extremePointXR <= _bitmap.Width)
            {
                if (right > 0)
                {
                    _right = right;
                }
                else
                {
                    _right = 0;
                }
            }
            else
            {
                var availableSpace = _bitmap.Width - 1 - _point.GetX() - _right;
                _right += availableSpace;
            }
            if (extremePointYL <= _bitmap.Height)
            {
                if (leftHeight > 0)
                {
                    _leftHeight = leftHeight;
                }
                else
                {
                    _leftHeight = 0;
                }
            }
            else
            {
                var availableSpace = _bitmap.Height - 1 - _point.GetY() - _leftHeight;
                _leftHeight += availableSpace;
            }
            if (extremePointYR <= _bitmap.Height)
            {
                if (leftHeight > 0)
                {
                    _rightHeight = rightHeight;
                }
                else
                {
                    _rightHeight = 0;
                }
            }
            else
            {
                var availableSpace = _bitmap.Height - 1 - _point.GetY() - _rightHeight;
                _rightHeight += availableSpace;
            }
            Draw();
        }

        public float getOffsetCoordinate(float newOffset, bool isX)
        {
            float offset = 0;
            var size = isX ? _bitmap.Width: _bitmap.Height;
            size--;

            var listPoints = new List<CustomPoint>() 
            {
                _point,
                new CustomPoint(_point.GetX() - _left, _point.GetY() + _leftHeight),
                new CustomPoint(_point.GetX() + _right, _point.GetY() + _rightHeight)
            };

            //если x больше нуля, значит смещение вправо или вниз
            if (newOffset >= 0)
            {
                //находим макс координату
                var maxCoordinate = isX ? listPoints.Max(x => x.GetX()) : listPoints.Max(x=> x.GetY());
                var extremePoint = maxCoordinate + newOffset;
                offset = size >= extremePoint ? newOffset : size - maxCoordinate;
            }
            //если x меньше нуля, то смещение влево или вверх
            else
            {
                //находим мин координату
                var minCoordinate = isX ? listPoints.Min(x => x.GetX()) : listPoints.Min(x => x.GetY());
                var extremePoint = minCoordinate + newOffset;
                offset = extremePoint >= 0 ? newOffset : -minCoordinate;
            }
            return offset;
        }

        public override void Draw()
        {
            if(_isVisible)
            {
                Graphics graphics = Graphics.FromImage(_bitmap);
                PointF[] points = new PointF[]
                {
                new PointF(_point.GetX(), _point.GetY()), // верх
                new PointF(_point.GetX() - _left , _point.GetY() + _leftHeight), // левый нижний угол
                new PointF(_point.GetX() + _right, _point.GetY() + _rightHeight), // правый нижний угол                    
                };

                graphics.DrawPolygon(new Pen(_color), points);
            }
        }
    }
}
