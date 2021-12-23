using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Draw2D.Entities;
using Draw2D.GraphicsObjects.Enums;
using Draw2D.Services;

namespace Draw2D.GraphicsObjects.Base
{
    [Serializable]
    public abstract class BaseGraphic
    {
        public GraphicObject ObjectType { get; private set; }

        private Color? _lineColor;
        private int _lineWidth;
        private DashStyle _lineDashStyle;
        private float[] _lineDashPatern;
        
        private Color? _fillColor;

        //[NonSerialized]
        //protected Region _clipRegion;

        [NonSerialized]
        protected Region _region;
        [NonSerialized]
        protected GraphicsPath _path;

        [NonSerialized]
        private Pen _linePen;
        [NonSerialized]
        private Brush _lineBrush;
        [NonSerialized]
        private Brush _fillBrush;
        [NonSerialized] 
        private bool _selected;
        [NonSerialized]
        private Layer _parentLayer;

        public Position Location { get; set; }
        public BaseGraphic ExcludedArea { get; set; }

        public Position EndPoint { get; set; }
        public Position StartPoint
        {
            get { return Location; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public Layer ParentLayer
        {
            get { return _parentLayer; }
            set { _parentLayer = value; }
        }

        public Pen LinePen
        {
            get { return _linePen; }
        }
        public Brush LineBrush
        {
            get { return _lineBrush; }
        }
        public Brush FillBrush
        {
            get { return _fillBrush; }
        }

        public Color? LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                if (_lineColor.HasValue)
                {
                    _linePen = GraphicsService.GetPen(_lineColor.Value, _lineWidth, _lineDashStyle, _lineDashPatern);
                    _lineBrush = GraphicsService.GetBrush(_lineColor.Value);
                }
                else
                {
                    _linePen = null;
                    _lineBrush = null;
                }
            }
        }
        public int LineWidth
        {
            get { return _lineWidth; }
            set
            {
                _lineWidth = value;
                if (_lineColor.HasValue)
                {
                    _linePen = GraphicsService.GetPen(_lineColor.Value, _lineWidth, _lineDashStyle, _lineDashPatern);
                    _lineBrush = GraphicsService.GetBrush(_lineColor.Value);
                }
                else
                {
                    _linePen = null;
                }
            }
        }
        public float[] LineDashPattern
        {
            get { return _lineDashPatern; }
            set
            {
                _lineDashPatern = value;
                if (_lineColor.HasValue)
                {
                    _linePen = GraphicsService.GetPen(_lineColor.Value, _lineWidth, _lineDashStyle, _lineDashPatern);
                    _lineBrush = GraphicsService.GetBrush(_lineColor.Value);
                }
                else
                {
                    _linePen = null;
                }
            }
        }
        public DashStyle LineDashStyle
        {
            get { return _lineDashStyle; }
            set
            {
                _lineDashStyle = value;
                if (_lineColor.HasValue)
                {
                    _linePen = GraphicsService.GetPen(_lineColor.Value, _lineWidth, _lineDashStyle, _lineDashPatern);
                    _lineBrush = GraphicsService.GetBrush(_lineColor.Value);
                }
                else
                {
                    _linePen = null;
                }
            }
        }

        public Color? FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                if (_fillColor.HasValue)
                {
                    _fillBrush = GraphicsService.GetBrush(_fillColor.Value);
                }
                else
                {
                    _fillBrush = null;
                }
            }
        }

        protected BaseGraphic(GraphicObject objectType)
        {
            ObjectType = objectType;
            _lineDashStyle = DashStyle.Solid;
            _lineWidth = 1;
            Selected = false;
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (_lineColor.HasValue)
            {
                _linePen = GraphicsService.GetPen(_lineColor.Value, _lineWidth, _lineDashStyle, _lineDashPatern);
                _lineBrush = GraphicsService.GetBrush(_lineColor.Value);
            }
            if (_fillColor.HasValue)
            {
                _fillBrush = GraphicsService.GetBrush(_fillColor.Value);
            }
            Selected = false;
        }

        public override string ToString()
        {
            return ObjectType.ToString();
        }

        public virtual void Draw(Graphics gr)
        {
            throw new NotImplementedException();
        }

        public void Recalculate()
        {
            if (_region != null)
            {
                _region.Dispose();
                _region = null;
            }
            if (_path != null)
            {
                _path.Dispose();
                _path = null;
            }

            Calculate();
        }

        public Region GetRegion()
        {
            if (_region == null)
                _region = CalculateRegion();

            return _region;
        }

        public GraphicsPath GetPath()
        {
            if (_path == null)
                CalculateRegion();

            return _path;
        }

        public virtual void Calculate()
        {
            throw new NotImplementedException();
        }

        public virtual void Rotate(Position referentPoint, decimal angle)
        {

        }

        public Position Rotate(Position point, Position referentPoint, decimal rotationAngle)
        {
            if (point.IsEquel(referentPoint))
                return point;

            decimal deltaX = point.X - referentPoint.X;
            decimal deltaY = point.Y - referentPoint.Y;

            var r = Math.Sqrt((double)(deltaX * deltaX + deltaY * deltaY));

            double angleRadians = 0;

            if (deltaX >= 0 && deltaY >= 0)//I kvadrant
            {
                if (deltaX == 0 && deltaY == 0)
                    angleRadians = 0;
                else if (deltaX == 0)
                    angleRadians = Math.PI / 2;
                else if (deltaY == 0)
                    angleRadians = 0;
                else
                    angleRadians = Math.Atan((double)(deltaY / deltaX));
            }
            else if (deltaX < 0 && deltaY >= 0) //II kvadrant
            {
                if (deltaY == 0)
                    angleRadians = Math.PI;
                else
                    angleRadians = Math.PI - Math.Atan((double)(deltaY / (-deltaX)));
            }
            else if (deltaX < 0 && deltaY < 0) //III kvadrant
            {
                angleRadians = Math.PI + Math.Atan((double)((-deltaY) / (-deltaX)));
            }
            else// deltaX >= 0 && deltaY < 0   IV kvadrant
            {
                if (deltaX == 0)
                    angleRadians = 1.5 * Math.PI;
                else
                    angleRadians = 1.5 * Math.PI + Math.Atan((double)((deltaX) / (-deltaY)));
            }

            angleRadians = angleRadians + (((double)rotationAngle) / 180) * Math.PI;
            if (angleRadians >= (2 * Math.PI))
                angleRadians = angleRadians % (2 * Math.PI);

            deltaX = (decimal)(r * Math.Cos(angleRadians));
            deltaY = (decimal)(r * Math.Sin(angleRadians));


            return new Position(referentPoint.X + deltaX, referentPoint.Y + deltaY);

        }

        public virtual Region CalculateRegion()
        {
            return null;
        }

        public bool ContainsPoint(Point pt)
        {
            if (GetRegion().IsVisible(pt) || GetPath().IsVisible(pt))
                return true;
            return false;
        }

        public void Dispose()
        {
            if (_region != null)
                _region.Dispose();
            if (_path != null)
                _path.Dispose();
        }

        public virtual BaseGraphic Clone()
        {
            throw new NotImplementedException();
        }

        public virtual void Move(BaseGraphic original, Position shift)
        {
            throw new NotImplementedException();
        }

        
    }
}
