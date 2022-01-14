using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Draw2D.GraphicsObjects.Base;

namespace Draw2D.Entities
{
    [Serializable]
    public class Drawing
    {
        public Point ReferentPoint { get; set; }
        public decimal Dpi { get; set; }
        public decimal Zoom { get; set; }
        public List<Layer> Layers { get; set; }

        [NonSerialized]
        private List<BaseGraphic> _selectedObjects;
        [NonSerialized]
        private List<BaseGraphic> _clonedObjects;

        public List<BaseGraphic> SelectedObjects
        {
            get { return _selectedObjects; }
        }

        public Drawing(int displayHeight)
        {
            Layers = new List<Layer>();
            Zoom = 1m;
            Dpi = 50;
            ReferentPoint = new Point(20, displayHeight - 20);
            _selectedObjects = new List<BaseGraphic>();
            _clonedObjects = new List<BaseGraphic>();
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (Layers == null)
                Layers = new List<Layer>();
            foreach (var layer in Layers)
                layer.ParentDrawing = this;

            _selectedObjects = new List<BaseGraphic>();
            _clonedObjects = new List<BaseGraphic>();
        }

        public Point CalculatePoint(Position position)
        {
            var pt = new Point()
            {
                X = (int)(position.X * Dpi * Zoom) + ReferentPoint.X,
                Y = (ReferentPoint.Y - (int)(position.Y * Dpi * Zoom))
            };

            return pt;
        }

        public Position CalculatePosition(Point point)
        {
            var pt = new Position()
            {
                X = ((decimal) (point.X - ReferentPoint.X)) / (Dpi * Zoom),
                Y = ((decimal) (ReferentPoint.Y - point.Y)) / (Dpi * Zoom)
            };

            return pt;
        }

        public BaseGraphic SelectObject(Point point, bool keepSelection = false)
        {
            var obj = GetObject(point);
            if (obj == null)
            {
                if (!keepSelection)
                {
                    ClearSelection();
                }

            }
            else
            {
                if (!_selectedObjects.Contains(obj))
                {
                    if(!keepSelection)
                        _selectedObjects.Clear();
                    _selectedObjects.Add(obj);
                }
            }

            return obj;
        }

        /// <summary>
        /// TODO: Napraviti da enumerira sve obrnuto od redslijeda kreiranja elemenata
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private BaseGraphic GetObject(Point point)
        {
            foreach (var layer in Layers.Where(l => l.Visible && !l.Locked))
            {
                foreach (var go in layer.Objects)
                {
                    if (go.ContainsPoint(point))
                    {
                        return go;
                    }
                }
            }
            return null;
        }

        public void MoveObjects(Point referentPoint, Point newPoint)
        {
            if (!_selectedObjects.Any())
                return;
            if (!_clonedObjects.Any())
                CloneSelected();
            var refPosition = CalculatePosition(referentPoint);
            var newPosition = CalculatePosition(newPoint);

            var positionShift = new Position(newPosition.X - refPosition.X, newPosition.Y - refPosition.Y);

            for (int i = 0; i < _selectedObjects.Count; ++i)
            {
                var obToMove = _selectedObjects[i];
                var startingObject = _clonedObjects[i];
                obToMove.Move(startingObject, positionShift);
            }
        }

        private void CloneSelected()
        {
            foreach (var ob in _selectedObjects)
            {
                _clonedObjects.Add(ob.Clone());
            }
        }

        public void ClearSelection()
        {
            _selectedObjects.Clear();
            foreach(var clone in _clonedObjects)
                clone.Dispose();
            _clonedObjects.Clear();
        }

        public void ClearCloned()
        {
            foreach (var clone in _clonedObjects)
                clone.Dispose();
            _clonedObjects.Clear();
        }

        public int CalculatePixels(decimal value)
        {
            return (int) (value * Dpi * Zoom);
        }

        public void Recalculate()
        {
            foreach (var layer in Layers)
            {
                layer.Recalculate();    
            }
        }

        public void Draw(Graphics gr)
        {
            foreach (var layer in Layers)
            {
                layer.Draw(gr);
            }
        }

        public void SizeHorizontal(decimal magnify, Position referentPoint)
        {
            if (_selectedObjects == null || !_selectedObjects.Any())
                return;

            foreach (var ob in _selectedObjects)
                ob.SizeHorizontal(magnify, null);
        }

        public void SizeVertical(decimal magnify, Position referentPoint)
        {
            if (_selectedObjects == null || !_selectedObjects.Any())
                return;

            foreach (var ob in _selectedObjects)
                ob.SizeVertical(magnify, null);
        }

        public void Size(decimal magnify, Position referentPoint)
        {
            if (_selectedObjects == null || !_selectedObjects.Any())
                return;

            foreach (var ob in _selectedObjects)
                ob.Size(magnify, null);
        }
    }
}
