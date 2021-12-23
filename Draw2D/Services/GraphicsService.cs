using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Draw2D.Services
{
    public static class GraphicsService
    {
        private static Dictionary<string, Pen> _pens;
        private static Dictionary<string, Brush> _brush;

        static GraphicsService()
        {
            _pens = new Dictionary<string, Pen>();
            _brush = new Dictionary<string, Brush>();
            PathPen = new Pen(Color.Black, 10);
        }
        

        public static Pen GetPen(Color color, int width, DashStyle dashStyle, float[] dashPattern)
        {
            string penId = GetPenDescription(color, width, dashStyle, dashPattern);
            if (!_pens.ContainsKey(penId))
            {
                var pen = new Pen(color, width);
                if (dashStyle != DashStyle.Solid && dashPattern != null && dashPattern.Length > 0)
                {
                    pen.DashStyle = dashStyle;
                    pen.DashPattern = dashPattern;
                }
                _pens.Add(penId, pen);
            }

            return _pens[penId];
        }

        public static Brush GetBrush(Color color)
        {
            string brushId = GetBrushDescription(color);
            if (!_brush.ContainsKey(brushId))
            {
                var brush = new SolidBrush(color);
                _brush.Add(brushId, brush);
            }

            return _brush[brushId];
        }

        private static string GetPenDescription(Color color, int width, DashStyle dashStyle, float[] dashPattern)
        {
            return $"#{color.A}.{color.R}.{color.G}.{color.B}#{width}#{dashStyle}#{(dashPattern != null && dashPattern.Any() ? string.Join(";", dashPattern) : "")}";
        }

        private static string GetBrushDescription(Color color)
        {
            return $"#{color.A}.{color.R}.{color.G}.{color.B}#";
        }

        public static Pen PathPen { get; set; }

    }
}
