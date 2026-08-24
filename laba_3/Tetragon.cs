using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Tetragon : TFigure
    {
        protected CustomPoint _dot2;
        protected CustomPoint _dot3;
        protected CustomPoint _dot4;

        public Tetragon(Bitmap bitmap) : base(bitmap)
        {
            MessageBox.Show("Создан четырехугольник.");
        }

        public Tetragon(int id, Bitmap bitmap, Color color, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) 
            : base(id, bitmap, color, x1, y1)
        {
            _dot2 = new CustomPoint(x2, y2);
            _dot3 = new CustomPoint(x3, y3);
            _dot4 = new CustomPoint(x4, y4);
        }

        public virtual void MoveTo(float x, float y)
        {
            var points = new[] { _point, _dot2, _dot3, _dot4 };
            var arrayX = points.Select(z => z.GetX()).ToArray();
            var arrayY = points.Select(z => z.GetY()).ToArray();

            //если x больше нуля, значит смещение вправо или вниз, иначе вверх или влево
            var extremeCoordinateX = x >= 0 ? arrayX.Max() : arrayX.Min();
            //если y больше нуля, значит смещение вправо или вниз, иначе вверх или влево
            var extremeCoordinateY = y >= 0 ? arrayY.Max() : arrayY.Min();

            _point.MoveX(getOffsetCoordinate(x, extremeCoordinateX, true));
            _point.MoveY(getOffsetCoordinate(y, extremeCoordinateY, false));

            _dot2.MoveX(getOffsetCoordinate(x, extremeCoordinateX, true));
            _dot2.MoveY(getOffsetCoordinate(y, extremeCoordinateY, false));

            _dot3.MoveX(getOffsetCoordinate(x, extremeCoordinateX, true));
            _dot3.MoveY(getOffsetCoordinate(y, extremeCoordinateY, false));

            _dot4.MoveX(getOffsetCoordinate(x, extremeCoordinateX, true));
            _dot4.MoveY(getOffsetCoordinate(y, extremeCoordinateY, false));
        }

        public virtual float getOffsetCoordinate(float newOffset, float extremeCoordinate, bool isX)
        {
            float offset = 0;
            float windowSize = (isX ? _bitmap.Width : _bitmap.Height) - 1;

            //если x больше нуля, значит смещение вправо или вниз
            if (newOffset >= 0)
            {
                var extremePoint = extremeCoordinate + newOffset;
                offset = windowSize >= extremePoint ? newOffset : windowSize - extremeCoordinate;
            }
            //если x меньше нуля, то смещение влево или вверх
            else
            {
                var extremePoint = extremeCoordinate + newOffset;
                //и если смещение больше нуля, то можно двигать,
                //иначе двигаем на ноль
                offset = extremePoint >= 0 ? newOffset : -extremeCoordinate;
            }
            return offset;
        }

        public virtual void MoveDot(CustomPoint point, float offsetX, float offsetY = 0)
        {
            point.MoveX(getOffsetCoordinate(offsetX, point.GetX(), true));

            if(offsetY != 0)
                point.MoveY(getOffsetCoordinate(offsetY, point.GetY(), false));

            //var extremePointX = point.GetX() + offsetX;
            //var extremePointY = point.GetY() + offsetY;

            //if (extremePointX <= _bitmap.Width)
            //{
            //    point.MoveX(offsetX);
            //}
            //else
            //{
            //    var availableSpace = _bitmap.Width - 1 - point.GetX();
            //    point.MoveX(availableSpace);
            //}
            //if (extremePointY <= _bitmap.Height)
            //{
            //    point.MoveY(offsetY);
            //}
            //else
            //{
            //    var availableSpace = _bitmap.Height - 1 - point.GetY();
            //    _point.MoveY(availableSpace);
            //}
        }

        public override void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            PointF[] points = new PointF[]
            {
                new PointF(_point.GetX(), _point.GetY()),
                new PointF(_dot2.GetX(), _dot2.GetY()), 
                new PointF(_dot3.GetX(), _dot3.GetY()),
                new PointF(_dot4.GetX(), _dot4.GetY())
            };
            graphics.DrawPolygon(new Pen(_color), points);
        }
    }
}
