using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSemestr_3
{
    class ArrayContainer : BaseContainer
    {
        private CustomArray<FigureWrapper> figures;

        public ArrayContainer(Bitmap _bitmap) : base(_bitmap)
        {
            figures = new CustomArray<FigureWrapper>();
        }

        public override void AddRandomFigure()
        {
            int width = bitmap.Width - 50;
            int height = bitmap.Height - 50;

            var figureType = random.Next(5);

            var randomColor = Color.FromKnownColor((KnownColor)random.Next(1, 175));
            
            switch (figureType)
            {
                case 0:
                    figures.Add(new FigureWrapper(new Circle(bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50), false)));
                    break;
                case 1:
                    figures.Add(new FigureWrapper(new Rectangle(bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50), random.Next(10, 50), false)));
                    break;
                case 2:
                    figures.Add(new FigureWrapper(new Triangle(bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 50),
                        random.Next(10, 50), random.Next(10, 50), random.Next(10, 50), false)));
                    break;
                case 3:
                    figures.Add(new FigureWrapper(new Ring(bitmap, randomColor, random.Next(50, width), random.Next(50, height), random.Next(10, 30),
                        random.Next(30, 60), false)));
                    break;
                case 4:
                    figures.Add(new FigureWrapper(new Ellipse(bitmap, randomColor, random.Next(50, width), random.Next(50, height),
                        random.Next(10, 50), random.Next(10, 50), false)));
                    break;
            }
            countFigures++;
        }

        protected override void Iterator(string command, string figureType, float[] additionalArgs)
        {
            if (figures != null)
            {
                for (var i = 0; i < figures.Items.Length; i++)
                {
                    figures.Items[i].MakeAction(command, figureType, additionalArgs);
                }
            }
        }

        public override void ActionWithFigures(string action, string figureType = "Все", float[] additionalArgs = null)
        {
            Iterator(action, figureType, additionalArgs);
        }

        public override void DestroyFigures()
        {
            if (figures != null)
            {
                figures.Clear();
            }
        }
    }
}
