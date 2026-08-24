using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabSemestr_3
{
    abstract class TFigure
    {
        public int _id;
        protected CustomPoint _point;
        protected Bitmap _bitmap;
        protected Color _color;

        public TFigure(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _color = Color.Black;
            _point = new CustomPoint();
            MessageBox.Show("Создана фигура.");
        }

        public TFigure(int id, Bitmap bitmap, Color color, float x, float y)
        {
            _id = id;
            _bitmap = bitmap;
            _color = color;
            _point = new CustomPoint(x, y);
        }

        public virtual void SetColor(Color color)
        {
            _color = color;
            Draw();
        }

        public virtual void MoveTo(float x, float y)
        {
            _point.MoveX(x);
            _point.MoveY(y);
            Draw();
        }

        public virtual void Draw() { }
    }
}
