namespace LabSemestr_3
{
    class CustomPoint
    {
        private float _x;
        private float _y;

        public CustomPoint()
        {
            _x = 0;
            _y = 0;
        }
        public CustomPoint(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public void MoveX(float x)
        {
            _x += x;
        }

        public void MoveY(float y)
        {
            _y += y;
        }

        public void SetX(float x)
        {
            _x = x;
        }

        public void SetY(float y)
        {
            _y = y;
        }

        public float GetX()
        {
            return _x;
        }

        public float GetY()
        {
            return _y;
        }
    }
}
