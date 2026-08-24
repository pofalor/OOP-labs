using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSemestr_3
{
    abstract class BaseContainer
    {
        protected int countFigures;
        protected Random random = new Random();
        protected Bitmap bitmap;

        public BaseContainer(Bitmap _bitmap)
        {
            bitmap = _bitmap;
        }

        public abstract void AddRandomFigure();

        protected abstract void Iterator(string command, string figureType, float[] additionalArgs);

        public abstract void ActionWithFigures(string action, string figureType = "Все", float[] additionalArgs = null);


        public abstract void DestroyFigures();
    }
}
