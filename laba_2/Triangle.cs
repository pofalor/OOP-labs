using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Triangle
    {
        public int _id;
        private CustomPoint _points;
        private float _left;
        private float _right;
        private float _leftHeight;
        private float _rightHeight;
        private Bitmap _bitmap;
        private Color _color;

        public Triangle(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _color = Color.Black;
            _left = 10;
            _right = 20;
            _leftHeight = 5;
            _rightHeight = 30;
            _points = new CustomPoint();
            MessageBox.Show("Создан треугольник.");
        }

        public Triangle(int id, Bitmap bitmap, Color color, int x, int y, int left, int right, int leftHeight, int rightHeight)
        {
            _id = id;
            _bitmap = bitmap;
            _color = color;
            _left = left;
            _right = right;
            _leftHeight = leftHeight;
            _rightHeight = rightHeight;
            _points = new CustomPoint(x, y);
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

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void MoveTo(int x, int y)
        {
            _points.MoveX(getOffsetCoordinate(x, true));
            _points.MoveY(getOffsetCoordinate(y, false));
        }

        public void setSize(float left, float right, float leftHeight, float rightHeight)
        {
            _left = left > 0 ? left : 0;
            _right = right > 0 ? right : 0;
            _leftHeight = leftHeight > 0 ? leftHeight : 0;
            _rightHeight = rightHeight > 0 ? rightHeight : 0;

            var extremePointXL = _points.GetX() - left;
            var extremePointXR = _points.GetX() + right;
            var extremePointYL = _points.GetY() + leftHeight;
            var extremePointYR = _points.GetY() + rightHeight;

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
                var availableSpace = _points.GetX() - 1;
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
                var availableSpace = _bitmap.Width - 1 - _points.GetX() - _right;
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
                var availableSpace = _bitmap.Height - 1 - _points.GetY() - _leftHeight;
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
                var availableSpace = _bitmap.Height - 1 - _points.GetY() - _rightHeight;
                _rightHeight += availableSpace;
            }
        }

        private float getOffsetCoordinate(float newOffset, bool isX)
        {
            float offset = 0;
            var size = isX ? _bitmap.Width: _bitmap.Height;
            size--;

            var listPoints = new List<CustomPoint>() 
            {
                _points,
                new CustomPoint(_points.GetX() - _left, _points.GetY() + _leftHeight),
                new CustomPoint(_points.GetX() + _right, _points.GetY() + _rightHeight)
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

        public void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            PointF[] points = new PointF[]
            {
                new PointF(_points.GetX(), _points.GetY()), // верх
                new PointF(_points.GetX() - _left , _points.GetY() + _leftHeight), // левый нижний угол
                new PointF(_points.GetX() + _right, _points.GetY() + _rightHeight), // правый нижний угол                    
            };

            graphics.DrawPolygon(new Pen(_color), points);
        }
    }
}
