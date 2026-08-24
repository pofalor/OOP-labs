using System.Drawing;


namespace LabSemestr_3
{
    class Triangle
    {
        private int _x;
        private int _y;
        private int _left;
        private int _right;
        private int _leftHeight;
        private int _rightHeight;
        private bool _isVisible;
        private Bitmap _bitmap;
        private Color _color;

        public Triangle(Bitmap bitmap, Color color, int x, int y, int left, int right, int leftHeight, int rightHeight)
        {
            _isVisible = true;
            _bitmap = bitmap;
            _color = color;
            _x = x;
            _y = y;
            _left = left;
            _right = right;
            _leftHeight = leftHeight;
            _rightHeight = rightHeight;
        }

        public int getLeft()
        {
            return _left;
        }

        public int getRight()
        {
            return _left;
        }
        public int getLeftHeight()
        {
            return _left;
        }
        public int getRightHeight()
        {
            return _left;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void MoveTo(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public void setSize(int left, int right, int leftHeight, int rightHeight)
        {
            _left = left > 0 ? left : 0;
            _right = right > 0 ? right : 0;
            _leftHeight = leftHeight > 0 ? leftHeight : 0;
            _rightHeight = rightHeight > 0 ? rightHeight : 0;
        }

        public void Draw()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            Point[] points = new Point[]
            {
                new Point(_x, _y), // верх
                new Point(_x - _left , _y + _leftHeight), // левый нижний угол
                new Point(_x + _right, _y + _rightHeight), // правый нижний угол                    
            };

            graphics.DrawPolygon(new Pen(_color), points);
        }
    }
}
