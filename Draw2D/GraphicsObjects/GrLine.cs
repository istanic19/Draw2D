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
    public class GrLine : BaseGraphic
    {
        public GrLine(Position location) : base(GraphicObject.Line)
        {
            Location = location;
            EndPoint = Location.Clone();
            LineColor = Color.Black;
        }

        public override void Calculate()
        {
            Location.Point = ParentLayer.ParentDrawing.CalculatePoint(Location);
            EndPoint.Point= ParentLayer.ParentDrawing.CalculatePoint(EndPoint);
        }

        public override void Draw(Graphics gr)
        {
            if (!ParentLayer.Visible || LineColor == null)
                return;
            var state = gr.Save();
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //gr.FillPath(Brushes.LightBlue, GetPath());
            gr.DrawLine(LinePen, StartPoint.PixelX, StartPoint.PixelY, EndPoint.PixelX, EndPoint.PixelY);

            gr.Restore(state);
        }

        public override void Rotate(Position referentPoint, decimal angle)
        {
            Location = Rotate(Location, referentPoint, angle);
            EndPoint = Rotate(EndPoint, referentPoint, angle);
            Recalculate();
        }

        public override Region CalculateRegion()
        {
            _path = new GraphicsPath();

            _path.AddLine(StartPoint.Point,EndPoint.Point);

            Matrix widenMatrix = new Matrix();
            _path.Widen(GraphicsService.PathPen, widenMatrix, 1.0f);

            Region region = new Region();
            region.MakeEmpty();

            return region;
        }

        public override BaseGraphic Clone()
        {
            var clone = new GrLine(Location.Clone());
            clone.ParentLayer = ParentLayer;
            clone.EndPoint = EndPoint.Clone();


            clone.Recalculate();
            clone.LineColor = LineColor;
            clone.LineWidth = LineWidth;

            return clone;
        }

        public override void Move(BaseGraphic original, Position shift)
        {
            Location.X = original.Location.X + shift.X;
            Location.Y = original.Location.Y + shift.Y;

            EndPoint.X = original.EndPoint.X + shift.X;
            EndPoint.Y = original.EndPoint.Y + shift.Y;
            Recalculate();
        }


    }
}
