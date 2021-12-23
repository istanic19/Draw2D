using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Draw2D.GraphicsObjects.Base;
using Draw2D.Services;

namespace Draw2D.Entities
{
    [Serializable]
    public class Layer
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool Locked { get; set; }

        public List<BaseGraphic> Objects { get; set; }

        private Drawing _parentDrawing;

        public Drawing ParentDrawing
        {
            get { return _parentDrawing; }
            set
            {
                _parentDrawing = value;
            }
        }

        public Layer(string name, Drawing parent)
        {
            _parentDrawing = parent;
            Name = name;
            Visible = true;
            Locked = false;
            Objects = new List<BaseGraphic>();
        }
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (Objects == null)
                Objects = new List<BaseGraphic>();
            foreach (var ob in Objects)
                ob.ParentLayer = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Recalculate()
        {
            foreach(var ob in Objects)
                ob.Recalculate();
        }

        public void Draw(Graphics gr)
        {
            if (!Visible)
                return;
            foreach (var ob in Objects)
            {
                ob.Draw(gr);
            }
        }
    }
}
