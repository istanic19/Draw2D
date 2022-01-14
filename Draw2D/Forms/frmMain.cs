using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Draw2D.Entities;
using Draw2D.GraphicsObjects;
using Draw2D.GraphicsObjects.Base;
using Draw2D.GraphicsObjects.Enums;

namespace Draw2D.Forms
{
    public partial class frmMain : Form
    {
        #region Fields

        private Drawing _drawing;

        #endregion

        #region Event Handlers

        #region Form Load, Closing, Shown

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmMain_Shown(object sender, EventArgs e)
        {

        }

        #endregion

        private void pbDisplay_SizeChanged(object sender, EventArgs e)
        {
            if (_drawing != null)
            {
                _drawing.Recalculate();
                pbDisplay.Invalidate();
            }
        }

        private void pbDisplay_Paint(object sender, PaintEventArgs e)
        {
            if (_drawing != null)
            {
                _drawing.Draw(e.Graphics);
            }
        }

        #endregion

        #region Constructor
        public frmMain()
        {
            InitializeComponent();
        }


        #endregion

        #region Methods
        
        private void LoadData()
        {
            _drawing = new Drawing(pbDisplay.Height);
            _drawing.Layers.Add(new Layer("", _drawing));


            for (int i = 1; i <= 10; ++i)
            {
                BaseGraphic grp = new GrPoint(new Position(i, 1));
                grp.ParentLayer = _drawing.Layers.First();
                grp.Recalculate();
                _drawing.Layers.First().Objects.Add(grp);
            }


            for (int i = 2; i <= 10; ++i)
            {
                BaseGraphic grp = new GrPoint(new Position(1, i));
                grp.ParentLayer = _drawing.Layers.First();
                grp.Recalculate();
                _drawing.Layers.First().Objects.Add(grp);
            }


            //grpLine.Rotate(new Position(6.5m, 1), 30);
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            _drawing.Layers.First().Objects.Clear();
            pbDisplay.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*var multi = new GrPolygon(new Position(5, 6));
            multi.FillColor = Color.Aquamarine;
            multi.ParentLayer = _drawing.Layers.First();
            multi.Points.Add(new Position(6, 8));
            multi.Points.Add(new Position(8, 5));
            multi.Points.Add(new Position(7, 4));
            multi.LineColor = Color.Green;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);
            pbDisplay.Invalidate();*/

            /*var eli = new GrElipse(new Position(5, 6));
            eli.FillColor = Color.Aquamarine;
            //eli.Angle = -30m;
            eli.LineWidth = 3;
            eli.Width = 4;
            eli.Height = 2;
            eli.ParentLayer = _drawing.Layers.First();
            eli.Recalculate();
            eli.Rotate(new Position(1, 6), 30);
            _drawing.Layers.First().Objects.Add(eli);


            var grp = new GrPoint(new Position(5, 6));
            grp.ParentLayer = _drawing.Layers.First();
            grp.Recalculate();
            _drawing.Layers.First().Objects.Add(grp);*/

            var multi = new GrClosedCurve(new Position(12, 9));
            multi.FillColor = Color.Aquamarine;
            multi.Tension = 0.3f;
            multi.ParentLayer = _drawing.Layers.First();
            multi.Points.Add(new Position(14, 6));
            multi.Points.Add(new Position(13, 5));
            multi.Points.Add(new Position(10, 6));
            multi.Points.Add(new Position(10, 7));
            multi.Points.Add(new Position(11, 9));

            multi.LineColor = Color.Green;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);

            /*var multi2 = new GrClosedCurve(new Position(11, 8));
            multi2.Tension = 0.3f;
            multi2.ParentLayer = _drawing.Layers.First();
            multi2.Points.Add(new Position(12, 8));
            multi2.Points.Add(new Position(12, 7));
            multi2.Points.Add(new Position(11, 7));

            multi2.LineColor = Color.Red;
            multi2.LineWidth = 3;
            multi2.Recalculate();
            _drawing.Layers.First().Objects.Add(multi2);

            multi.ExcludedArea = multi2;*/


            pbDisplay.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var multi = new GrCurve(new Position(1, 2));
            multi.FillColor = Color.Aquamarine;
            multi.Tension = 0.3f;
            multi.ParentLayer = _drawing.Layers.First();
            multi.Points.Add(new Position(2, 3));
            multi.Points.Add(new Position(2, 1));

            multi.LineColor = Color.Green;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);

            


            pbDisplay.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var multi = new GrEllipse(new Position(6, 8));
            multi.ParentLayer = _drawing.Layers.First();
            multi.Width = 5;
            multi.Height = 3;
            multi.FillColor = Color.BurlyWood;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);


            /*var multi2 = new GrEllipse(new Position(6, 8));
            multi2.Angle = -30;
            multi2.ParentLayer = _drawing.Layers.First();
            multi2.Width = 2;
            multi2.Height = 1;
            multi2.LineWidth = 3;
            multi2.Recalculate();
            _drawing.Layers.First().Objects.Add(multi2);

            multi.ExcludedArea = multi2;*/

            pbDisplay.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var multi = new GrPie(new Position(3, 4));
            multi.ParentLayer = _drawing.Layers.First();
            multi.Width = 8;
            multi.Height = 5;
            multi.StartAngle = 0;
            multi.EndAngle = -45;
            multi.FillColor = Color.BurlyWood;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);


            /*var multi2 = new GrPie(new Position(4.5m, 4.3m));
            multi2.Angle = -30;
            multi2.ParentLayer = _drawing.Layers.First();
            multi2.Width = 4;
            multi2.Height = 1;
            multi2.StartAngle = 0;
            multi2.EndAngle = -45;
            multi2.LineWidth = 3;
            multi2.Recalculate();
            _drawing.Layers.First().Objects.Add(multi2);

            multi.ExcludedArea = multi2;*/







            pbDisplay.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var multi = new GrArc(new Position(7, 4));
            multi.ParentLayer = _drawing.Layers.First();
            multi.Width = 8;
            multi.Height = 5;
            multi.StartAngle = 0;
            multi.EndAngle = -45;
            multi.FillColor = Color.BurlyWood;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);

            pbDisplay.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var multi = new GrPolygon(new Position(10, 1));
            multi.FillColor = Color.Aquamarine;
            multi.ParentLayer = _drawing.Layers.First();
            multi.Points.Add(new Position(8, 3));
            multi.Points.Add(new Position(14, 3));

            multi.LineColor = Color.Green;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);




            pbDisplay.Invalidate();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var multi = new GrMultiline(new Position(5, 1));
            multi.ParentLayer = _drawing.Layers.First();
            multi.Points.Add(new Position(6, 2));
            multi.Points.Add(new Position(7.5m, 1.5m));
            multi.Points.Add(new Position(8.5m, 3.5m));

            multi.LineColor = Color.Red;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);




            pbDisplay.Invalidate();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var multi = new GrLine(new Position(6, 0.5m));
            multi.ParentLayer = _drawing.Layers.First();
            multi.EndPoint = new Position(8, 0.3m);

            multi.LineColor = Color.Red;
            multi.LineWidth = 3;
            multi.Recalculate();
            _drawing.Layers.First().Objects.Add(multi);

            pbDisplay.Invalidate();
        }

        private Point? _referentPoint = null;

        private void pbDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (_drawing == null)
                return;
            _referentPoint = e.Location;
            _drawing.SelectObject(e.Location, ModifierKeys == Keys.Control);
        }

        private void pbDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (_drawing == null)
                return;

            /*var selection = _drawing.SelectObject(e.Location, Control.ModifierKeys == Keys.Control);
            if (selection == null)
            {
                _drawing.ClearSelection();
            }
            else
            {
                _drawing.ClearCloned();
            }*/

            _drawing.ClearCloned();
            _referentPoint = null;
        }

        private void pbDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drawing == null)
                return;
            if ((e.Button & MouseButtons.Left) != 0 && _drawing.SelectedObjects.Any() && _referentPoint != null)
            {
                _drawing.MoveObjects(_referentPoint.Value, e.Location);
                pbDisplay.Invalidate();
            }
        }

        private void btIncreaseWidth_Click(object sender, EventArgs e)
        {
            if (_drawing == null)
                return;

            _drawing.SizeHorizontal(1.1m,null);
            pbDisplay.Invalidate();
        }

        private void btDecreaseWidth_Click(object sender, EventArgs e)
        {
            if (_drawing == null)
                return;

            _drawing.SizeHorizontal(0.9m, null);
            pbDisplay.Invalidate();
        }

        private void btnIncreaseHeight_Click(object sender, EventArgs e)
        {
            if (_drawing == null)
                return;

            _drawing.SizeVertical(1.1m, null);
            pbDisplay.Invalidate();
        }

        private void btnDecreaseHeight_Click(object sender, EventArgs e)
        {
            if (_drawing == null)
                return;

            _drawing.SizeVertical(0.9m, null);
            pbDisplay.Invalidate();
        }

        private void btnSizePlus_Click(object sender, EventArgs e)
        {
            if (_drawing == null)
                return;

            _drawing.Size(1.1m, null);
            pbDisplay.Invalidate();
        }

        private void btnSizeMinus_Click(object sender, EventArgs e)
        {
            if (_drawing == null)
                return;

            _drawing.Size(0.9m, null);
            pbDisplay.Invalidate();
        }
    }
}
