using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draw2D.Entities;
using Draw2D.GraphicsObjects.Base;
using Draw2D.GraphicsObjects.Enums;
using Draw2D.Services;

namespace Draw2D.GraphicsObjects
{
    [Serializable]
    public class GrClosedCurve : BaseGraphic
    {
        public List<Position> Points { get; set; }
        public float Tension { get; set; }
        public FillMode FillMode { get; set; }
       
        
        public GrClosedCurve(Position location) : base(GraphicObject.ClosedCurve)
        {
            Points = new List<Position>();
            Location = location;
            Points.Add(Location);
            LineColor = Color.Black;
            Tension = 0.4f;
            FillMode = FillMode.Alternate;
        }

        public override void Calculate()
        {
            foreach (var pt in Points)
                pt.Point = ParentLayer.ParentDrawing.CalculatePoint(pt);
        }

        public override void Draw(Graphics gr)
        {
            if (!ParentLayer.Visible || LineColor == null)
                return;
            if (Points.Count < 2)
                return;

            var state = gr.Save();
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var pts = Points.Select(p => p.Point).ToArray();

            if (ExcludedArea != null)
            {
                gr.ExcludeClip(ExcludedArea.GetRegion());
            }

            //gr.FillPath(Brushes.LightBlue, GetPath());

            if (FillColor != null)
                gr.FillClosedCurve(FillBrush, pts, FillMode, Tension);
            
            gr.DrawClosedCurve(LinePen, pts, Tension, FillMode);
            gr.Restore(state);

        }

        public override void Rotate(Position referentPoint, decimal angle)
        {
            foreach (var pt in Points)
            {
                pt.CopyFrom(Rotate(pt, referentPoint, angle));
            }

            Recalculate();
        }


        public override Region CalculateRegion()
        {
            _path = new GraphicsPath();

            _path.AddClosedCurve(Points.Select(p => p.Point).ToArray(), Tension);

            Region region = new Region(_path);

            Matrix widenMatrix = new Matrix();
            _path.Widen(GraphicsService.PathPen, widenMatrix, 1.0f);

            return region;
        }

        public override BaseGraphic Clone()
        {
            var clone = new GrClosedCurve(Location.Clone());
            clone.ParentLayer = ParentLayer;
            clone.Tension = Tension;
            clone.FillMode = FillMode;
            for (int i = 1; i < Points.Count; ++i)
            {
                clone.Points.Add(Points[i].Clone());
            }


            clone.Recalculate();
            clone.LineColor = LineColor;
            clone.LineWidth = LineWidth;

            return clone;
        }

        public override void Move(BaseGraphic original, Position shift)
        {
            Location.X = original.Location.X + shift.X;
            Location.Y = original.Location.Y + shift.Y;
            for (int i = 0; i < Points.Count; ++i)
            {
                var startingPoint = ((GrClosedCurve) original).Points[i];
                Points[i].X = startingPoint.X + shift.X;
                Points[i].Y = startingPoint.Y + shift.Y;
            }
            Recalculate();
        }
    }
}
