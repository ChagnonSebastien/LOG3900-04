using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace PolyPaint.Modeles.Strokes
{

    class BaseElipseStroke : CustomStroke
    {
        

        public BaseElipseStroke(StylusPointCollection pts, CustomStrokeCollection strokes) : base(pts, strokes)
        {
        }

        public override void addDragHandles()
        {
            throw new NotImplementedException();
        }

        public override void deleteDragHandles()
        {
            
        }

        public override StrokeType getType()
        {
            return StrokeType.OBJECT;
        }

        public override bool HitTest(Point point)
        {
            double width = Math.Abs(this.StylusPoints[0].X - this.StylusPoints[1].X);
            double height = Math.Abs(this.StylusPoints[0].Y - this.StylusPoints[1].Y);
            double centerX = (this.StylusPoints[0].X + this.StylusPoints[1].X) / 2;
            double centerY = (this.StylusPoints[0].Y + this.StylusPoints[1].Y) / 2;
            return Math.Pow(point.X - centerX, 2) / Math.Pow(width / 2, 2) + Math.Pow(point.Y - centerY, 2) / Math.Pow(height / 2, 2) <= 1;
        }

        public override bool isSelectable()
        {
            return true;
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            DrawingAttributes originalDa = drawingAttributes.Clone();
            SolidColorBrush fillBrush = new SolidColorBrush(drawingAttributes.Color);
            fillBrush.Freeze();
            Pen outlinePen = new Pen(new SolidColorBrush(Colors.Black), 2);
            outlinePen.Freeze();

            StylusPoint stp = this.StylusPoints[0];
            StylusPoint sp = this.StylusPoints[1];

            if (this.isEditing())
            {
                Pen selectedPen = new Pen(new SolidColorBrush(Colors.Blue), 10);
                selectedPen.Freeze();
                drawingContext.DrawEllipse(null, selectedPen, new Point((sp.X + stp.X) / 2.0, (sp.Y + stp.Y) / 2.0), Math.Abs(sp.X - stp.X) / 2, Math.Abs(sp.Y - stp.Y) / 2);


            }
            else if (this.isSelected())
            {
                Pen selectedPen = new Pen(new SolidColorBrush(Colors.GreenYellow), 10);
                selectedPen.Freeze();
                drawingContext.DrawEllipse(null, selectedPen, new Point((sp.X + stp.X) / 2.0, (sp.Y + stp.Y) / 2.0), Math.Abs(sp.X - stp.X) / 2, Math.Abs(sp.Y - stp.Y) / 2);
            }

            drawingContext.DrawEllipse(fillBrush, outlinePen, new Point((sp.X + stp.X) / 2.0, (sp.Y + stp.Y) / 2.0), Math.Abs(sp.X - stp.X) / 2, Math.Abs(sp.Y - stp.Y) / 2);
        }
    }
}
