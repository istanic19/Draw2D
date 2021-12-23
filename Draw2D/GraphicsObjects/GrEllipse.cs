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
    public class GrEllipse : BaseGraphic
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Angle { get; set; }

        private int _widthPixels;
        private int _heightPixels;
        public GrEllipse(Position location) : base(GraphicObject.Ellipse)
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

            var state = gr.Save();
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (ExcludedArea != null)
            { 
                gr.ExcludeClip(ExcludedArea.GetRegion());
            }

            //gr.FillPath(Brushes.LightBlue, GetPath());

            gr.TranslateTransform(Location.PixelX, Location.PixelY);
            if (Angle != 0)
                gr.RotateTransform((float) Angle);

            

            if (FillColor != null)
                gr.FillEllipse(FillBrush,  -_widthPixels/2, -_heightPixels / 2, _widthPixels, _heightPixels);

            gr.DrawEllipse(LinePen, -_widthPixels / 2, -_heightPixels / 2, _widthPixels, _heightPixels);

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
            _path.AddEllipse(- _widthPixels / 2, -_heightPixels / 2, _widthPixels, _heightPixels);
            
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
            var clone = new GrEllipse(Location.Clone());
            clone.ParentLayer = ParentLayer;
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
