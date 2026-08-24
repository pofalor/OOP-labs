using System.Drawing;
using System.Linq;

namespace LabSemestr_3
{
    class FigureWrapper
    {
        TFigure _figure;

        public FigureWrapper(TFigure figure) 
        { 
            _figure = figure;
        }

        public void MakeAction(string action, string figureType, float[] additionalArgs)
        {
            switch (figureType)
            {
                case "Круг":
                    {
                        if(!(_figure is Circle) || (_figure is Ellipse))
                        {
                            return;
                        }
                        break;
                    }
                case "Прямоугольник":
                    {
                        if (!(_figure is Rectangle))
                        {
                            return;
                        }
                        break;
                    }
                case "Треугольник":
                    {
                        if (!(_figure is Triangle))
                        {
                            return;
                        }
                        break;
                    }
                case "Кольцо":
                    {
                        if (!(_figure is Ring))
                        {
                            return;
                        }
                        break;
                    }
                case "Эллипс":
                    {
                        if (!(_figure is Ellipse))
                        {
                            return;
                        }
                        break;
                    }
            }
            switch (action)
            {
                case "Draw":
                    _figure.Draw();
                    break;

                case "Show":
                    _figure.SetVisible(true);
                    break;
                
                case "Hide":
                    _figure.SetVisible(false);
                    break;

                case "MoveFigure":
                    if (additionalArgs != null)
                    {
                        MoveFigure(figureType, additionalArgs[0], additionalArgs[1]);
                    }
                    break;
                case "SetColor":
                    if (additionalArgs != null)
                    {
                        _figure.SetColor(Color.FromKnownColor((KnownColor)additionalArgs[0]));
                    }
                    break;
                case "SetSize":
                    if (additionalArgs != null)
                    {
                        SetSize(figureType, additionalArgs);
                    }
                    break;
            }
        }

        private void SetSize(string typeFigure, float[] sizes)
        {
            switch (typeFigure)
            {
                case "Круг":
                    var circle = _figure as Circle;
                    circle.setRadius(sizes[0]);
                    break;

                case "Прямоугольник":
                    var rectangle = _figure as Rectangle;
                    rectangle.setSize(sizes[0] >= 0 ? sizes[0] : rectangle.getHorizontal(), sizes[1] >= 0 ? sizes[1] : rectangle.getVertical());
                    break;

                case "Треугольник":
                    var triangle = _figure as Triangle;
                    triangle.setSize(sizes[0] >= 0 ? sizes[0] : triangle.getLeft(), sizes[1] >= 0 ? sizes[1] : triangle.getRight(), 
                        sizes[2] >= 0 ? sizes[2] : triangle.getLeftHeight(), sizes[3] >= 0 ? sizes[3] : triangle.getRightHeight());
                    break;

                case "Кольцо":
                    var ring = _figure as Ring;
                    ring.setSize(sizes[0], sizes[1]);
                    break;

                case "Эллипс":
                    var ellipse = _figure as Ellipse;
                    ellipse.setSize(sizes[0] >= 0 ? sizes[0] : ellipse.getRadius(), sizes[1] >= 0 ? sizes[1] : ellipse.getRadiusHeight());
                    break;
            }

        }

        private void MoveFigure(string typeFigure, float x, float y)
        {
            switch(typeFigure)
            {
                case "Круг":
                    var circle = _figure as Circle;
                    MoveCircle(circle, x, y);
                    break;

                case "Прямоугольник":
                    var rectangle = _figure as Rectangle;
                    MoveRectangle(rectangle, x, y);
                    break;

                case "Треугольник":
                    var triangle = _figure as Triangle;
                    MoveTriangle(triangle, x, y);
                    break;

                case "Кольцо":
                    var ring = _figure as Ring;
                    MoveRing(ring, x, y);
                    break;

                case "Эллипс":
                    var ellipse = _figure as Ellipse;
                    MoveEllipse(ellipse, x, y);
                    break;

                case "Все":
                    if (_figure is Ellipse) MoveEllipse(_figure as Ellipse, x, y);
                    else if (_figure is Rectangle) MoveRectangle(_figure as Rectangle, x, y);
                    else if (_figure is Triangle) MoveTriangle(_figure as Triangle, x, y);
                    else if (_figure is Ring) MoveRing(_figure as Ring, x, y);
                    else if (_figure is Circle) MoveCircle(_figure as Circle, x, y);
                    break;
            }
        }

        #region Методы перемещения каждой из фигур
        private void MoveCircle(Circle circle, float x, float y)
        {
            circle.MoveTo(circle.getOffsetCoordinate(x, true), circle.getOffsetCoordinate(y, false));
        }
        private void MoveRectangle(Rectangle rectangle, float x, float y)
        {
            var points = new[]
            {
                rectangle.GetDot(1),
                rectangle.GetDot(2),
                rectangle.GetDot(3),
                rectangle.GetDot(4)
            };
            var arrayX = points.Select(z => z.GetX()).ToArray();
            var arrayY = points.Select(z => z.GetY()).ToArray();

            //если x больше нуля, значит смещение вправо или вниз, иначе вверх или влево
            var extremeCoordinateX = x >= 0 ? arrayX.Max() : arrayX.Min();
            //если y больше нуля, значит смещение вправо или вниз, иначе вверх или влево
            var extremeCoordinateY = y >= 0 ? arrayY.Max() : arrayY.Min();

            points[0].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
            points[0].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));

            points[1].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
            points[1].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));

            points[2].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
            points[2].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));

            points[3].MoveX(rectangle.getOffsetCoordinate(x, extremeCoordinateX, true));
            points[3].MoveY(rectangle.getOffsetCoordinate(y, extremeCoordinateY, false));
            rectangle.Draw();
        }

        private void MoveTriangle(Triangle triangle, float x, float y)
        {
            triangle.MoveTo(triangle.getOffsetCoordinate(x, true), triangle.getOffsetCoordinate(y, false));
        }

        private void MoveRing(Ring ring, float x, float y)
        {
            var ringSize = ring.GetRingSize();
            var extCircle = ring.getExternalCircle();
            var innCircle = ring.getInnerCircle();
            extCircle.MoveTo(extCircle.getOffsetCoordinate(x, true), extCircle.getOffsetCoordinate(y, false));
            innCircle.MoveTo(innCircle.getOffsetCoordinate(x, true, ringSize), innCircle.getOffsetCoordinate(y, false, ringSize));
            ring.Draw();
        }

        private void MoveEllipse(Ellipse ellipse, float x, float y)
        {
            ellipse.MoveTo(ellipse.getOffsetCoordinate(x, true), ellipse.getOffsetCoordinate(y, false));
        }
        #endregion
    }
}
