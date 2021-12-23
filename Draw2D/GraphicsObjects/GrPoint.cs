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

namespace Draw2D.GraphicsObjects
{
    [Serializable]
    public class GrPoint: BaseGraphic
    {
        public GrPoint(Position location) : base(GraphicObject.Point)
        {
            Location = location;
            LineColor = Color.Black;
        }

        public override void Calculate()
        {
            Location.Point = ParentLayer.ParentDrawing.CalculatePoint(Location);
        }

        public override void Draw(Graphics gr)
        {
            if (!ParentLayer.Visible || LineColor == null)
                return;
            var state = gr.Save();
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //gr.FillRegion(Brushes.LightCoral, GetRegion());
            gr.FillEllipse(LineBrush, Location.PixelX - 1, Location.PixelY - 1, 2, 2);

            gr.Restore(state);
        }

        public override Region CalculateRegion()
        {
            _path = new GraphicsPath();

            _path.AddEllipse(Location.PixelX - 4, Location.PixelY - 4, 8, 8);

            Region region = new Region(_path);

            return region;
        }

        public override BaseGraphic Clone()
        {
            var clone = new GrPoint(Location.Clone());
            clone.ParentLayer = ParentLayer;
            clone.Recalculate();
            clone.LineColor = LineColor;

            return clone;
        }

        public override void Move(BaseGraphic original, Position shift)
        {
            Location.X = original.Location.X + shift.X;
            Location.Y = original.Location.Y + shift.Y;
            Recalculate();
        }
    }
}
