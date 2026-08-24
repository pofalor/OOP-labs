using System.Drawing;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Rectangle : Tetragon
    {
        public Rectangle(Bitmap bitmap) : base(bitmap)
        {
            MessageBox.Show("Создан прямоугольник.");
        }

        public Rectangle(int id, Bitmap bitmap, Color color, int x, int y, int horizontal, int vertical) 
            : base(id, bitmap, color, x, y, x, y + vertical, x + horizontal, y + vertical, x + horizontal, y)
        {

        }

        public float getHorizontal()
        {
            //правый верхний угол минус левый верхний
            return _dot4.GetY() - _point.GetY();
        }

        public float getVertical()
        {
            //левый нижний угол минус левый верхний
            return _dot2.GetY() - _point.GetY();
        }

        public void setSize(float horizontal, float vertical)
        {
            var basePointX = _point.GetX();
            var basePointY = _point.GetY();

            var newHorizontal = basePointX + horizontal;
            var newVertical = basePointY + vertical;

            MoveDot(_dot4, newHorizontal - _dot4.GetX());
            MoveDot(_dot3, newHorizontal - _dot3.GetX(), newVertical - _dot3.GetY());
            MoveDot(_dot2, 0, newVertical - _dot2.GetY());
        }

    }
}
