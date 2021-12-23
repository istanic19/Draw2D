﻿using System;
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
    public class GrMultiline : BaseGraphic
    {
        public List<Position> Points { get; set; }
        public GrMultiline(Position location) : base(GraphicObject.MultiLine)
        {
            Points = new List<Position>();
            Location = location;
            Points.Add(Location);
            LineColor = Color.Black;
        }

        public override void Calculate()
        {
            foreach(var pt in Points)
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

            //gr.FillPath(Brushes.LightBlue, GetPath());
            gr.DrawLines(LinePen, pts);

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

            _path.AddLines(Points.Select(p => p.Point).ToArray());

            Matrix widenMatrix = new Matrix();
            _path.Widen(GraphicsService.PathPen, widenMatrix, 1.0f);

            Region region = new Region();

            region.MakeEmpty();

            return region;
        }

        public override BaseGraphic Clone()
        {
            var clone = new GrMultiline(Location.Clone());
            clone.ParentLayer = ParentLayer;
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
                var startingPoint = ((GrMultiline)original).Points[i];
                Points[i].X = startingPoint.X + shift.X;
                Points[i].Y = startingPoint.Y + shift.Y;
            }
            Recalculate();
        }

    }
}
