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
    public class GrPie : BaseGraphic
    {
        public decimal StartAngle { get; set; }
        public decimal EndAngle { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Angle { get; set; }
        private float _widthPixels;
        private float _heightPixels;
        public GrPie(Position location) : base(GraphicObject.Pie)
        {
            Location = location;
            LineColor = Color.Black;
        }

        public override void Calculate()
        {
            Location.Point = ParentLayer.ParentDrawing.CalculatePoint(Location);
            _widthPixels = ParentLayer.ParentDrawing.CalculatePixels(Width);
            _heightPixels = ParentLayer.ParentDrawing.CalculatePixels(Height);
        }

        public override void Draw(Graphics gr)
        {
            if (!ParentLayer.Visible || LineColor == null)
                return;
            if (StartAngle == EndAngle)
                return;

            var state = gr.Save();
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            if (ExcludedArea != null)
            {
                gr.ExcludeClip(ExcludedArea.GetRegion());
            }

            //gr.FillPath(Brushes.LightBlue, GetPath());

            gr.TranslateTransform(Location.PixelX, Location.PixelY);
            if (Angle != 0)
                gr.RotateTransform((float)Angle);

            if (FillColor != null)
                gr.FillPie(FillBrush, -_widthPixels / 2, -_heightPixels / 2, _widthPixels, _heightPixels, (float)StartAngle, (float)EndAngle);

            gr.DrawPie(LinePen, -_widthPixels / 2, -_heightPixels / 2, _widthPixels, _heightPixels, (float)StartAngle, (float)EndAngle);

            gr.Restore(state);

        }

        public override void Rotate(Position referentPoint, decimal angle)
        {
            if (!Location.IsEquel(referentPoint))
                Location = Rotate(Location, referentPoint, angle);
            Angle -= angle;
            Recalculate();
        }

        public override Region CalculateRegion()
        {
            _path = new GraphicsPath();
            _path.AddPie(-_widthPixels / 2, -_heightPixels / 2, _widthPixels, _heightPixels, (float)StartAngle, (float)EndAngle);
            
            Matrix tm = new Matrix();
            tm.Translate(Location.PixelX, Location.PixelY);

            if (Angle != 0)
            {
                var angle = ((double)Angle * Math.PI) / 180;
                tm.Rotate((float)Angle);
            }

            _path.Transform(tm);
            Region region = new Region(_path);

            Matrix widenMatrix = new Matrix();
            _path.Widen(GraphicsService.PathPen, widenMatrix, 1.0f);

            return region;
        }

        public override BaseGraphic Clone()
        {
            var clone = new GrPie(Location.Clone());
            clone.ParentLayer = ParentLayer;
            clone.StartAngle = StartAngle;
            clone.EndAngle = EndAngle;
            clone.Width = Width;
            clone.Height = Height;
            clone.Angle = Angle;


            clone.Recalculate();
            clone.LineColor = LineColor;
            clone.LineWidth = LineWidth;

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
