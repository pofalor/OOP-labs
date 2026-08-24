using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Ring
    {
        public int _id;
        private Bitmap _bitmap;
        private Color _color;
        private Circle _externalCircle;
        private Circle _innerCircle;

        public Ring(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _color = Color.Black;
            MessageBox.Show("Создано кольцо.");
        }

        public Ring(int id, Bitmap bitmap, Color color, float x, float y, float innerRadius, float externalRadius)
        {
            _id = id;
            _bitmap = bitmap;
            _color = color;
            if (innerRadius < externalRadius)
            {
                _innerCircle = new Circle(0, bitmap, color, x, y, innerRadius);
                _externalCircle = new Circle(1, bitmap, color, x, y, externalRadius);
            }
            else
            {
                _innerCircle = new Circle(0, bitmap, color, x, y, externalRadius);
                _externalCircle = new Circle(1, bitmap, color, x, y, innerRadius);
            }
        }

        public float getInnerRadius()
        {
            return _innerCircle.getRadius();
        }

        public float getExternalRadius()
        {
            return _externalCircle.getRadius();
        }

        public void setSize(float innerRadius, float externalRadius)
        {
            _innerCircle.setRadius(innerRadius);
            _externalCircle.setRadius(externalRadius);
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void MoveTo(float x, float y)
        {
            var ringSize = _externalCircle.getRadius() - _innerCircle.getRadius();
            _externalCircle.MoveTo(x, y);
            _innerCircle.MoveTo(x, y, ringSize);
        }

        public void Draw()
        {
            _externalCircle.Fill(_color);
            _innerCircle.Fill(Color.White);
        }
    }
}
