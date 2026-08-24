using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabSemestr_3
{
    class Ring : TFigure
    {
        private Circle _externalCircle;
        private Circle _innerCircle;

        public Ring(Bitmap bitmap) : base(bitmap)
        {
            _bitmap = bitmap;
            _color = Color.Black;
            MessageBox.Show("Создано кольцо.");
        }

        public Ring(Bitmap bitmap, Color color, float x, float y, float innerRadius, float externalRadius, bool isVisible) : base(bitmap, color, x, y, isVisible)
        {
            if (innerRadius < externalRadius)
            {
                _innerCircle = new Circle(bitmap, color, x, y, innerRadius, isVisible);
                _externalCircle = new Circle(bitmap, color, x, y, externalRadius, isVisible);
            }
            else
            {
                _innerCircle = new Circle(bitmap, color, x, y, externalRadius, isVisible);
                _externalCircle = new Circle(bitmap, color, x, y, innerRadius, isVisible);
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
            Draw();
        }

        public Color GetColor()
        {
            return _color;
        }

        public float GetRingSize()
        {
            return _externalCircle.getRadius() - _innerCircle.getRadius();
        }

        public Circle getInnerCircle()
        {
            return _innerCircle;
        }

        public Circle getExternalCircle()
        {
            return _externalCircle;
        }
        public override void Draw()
        {
            if(_isVisible)
            {
                _externalCircle.Fill(_color);
                _innerCircle.Fill(Color.White);
            }
        }
    }
}
