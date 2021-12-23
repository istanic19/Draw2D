using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw2D.Entities
{
    [Serializable]
    public class Position
    {
        [NonSerialized]
        private Point _point;
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public Point Point
        {
            get { return _point; }
            set { _point = value; }
        }

        public int PixelX
        {
            get { return _point.X; }
        }
        public int PixelY
        {
            get { return _point.Y; }
        }

        public Position()
        {

        }
        public Position(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X} | {Y})";
        }

        public Position Clone()
        {
            var clone = new Position(X, Y);
            clone._point = new Point(Point.X, Point.Y);

            return clone;
        }

        public void CopyFrom(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        public bool IsEquel(Position pos)
        {
            return (X == pos.X && Y == pos.Y);
        }

        public static Position Zero()
        {
            return new Position(0, 0);
        }
    }
}
