using EffectConfigModule.SpectrumHelp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CircleVisualizerPlugin
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public partial class CircleVisualizer : Decorator
    {
        private ViewModel.EffectViewModel vm = ViewModel.EffectViewModel.Instance;

        float circleOffsetPerFrame = 0.1f;
        float circleCurOffset = 0;
        double circleFrequencyEnd = 4500d;
        float maxOffset = 10;
        float curOffsetX = 0, curOffsetY = 0;
        public float offsetSpeed = 0.1f;
        public bool Shake = true;
        Func<float, float> dftDataFilter;
        public CircleVisualizer ()
        {

        }
        public void Init()
        {

            SpectrumBase.Instance.InitializeCapture();
            SpectrumBase.Instance.UpdateEvent += UpdateAudioSpectrum;
        }
        private void UpdateAudioSpectrum()
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                InvalidateVisual();
            });
        }


        public void Start()
        {
            SpectrumBase.Instance.Start();
        }
        public void Stop()
        {
            SpectrumBase.Instance.Stop();
        }
        protected override void OnRender(DrawingContext dc)
        {
            try
            {
                if (!SpectrumBase.Instance.Recording())
                    return;

                base.OnRender(dc);

                int fqEndIndex = (int)(circleFrequencyEnd / SpectrumBase.Instance.frequencyPerIndex);
                double[] resultPaint = SpectrumBase.Instance.GetDftData(fqEndIndex);
                if (resultPaint == null)
                    return;

                double
                    dataRight = resultPaint.Length, panelRight = this.ActualWidth, panelHeight = this.ActualHeight;
                dftDataFilter ??= (v) => v;

                int centerX = (int)panelRight / 2,
                    centerY = (int)panelHeight / 2;

                float offsetXPix = curOffsetX, offsetYPix = curOffsetY;

               // Point swing = new Point(centerX, centerY);

                if(Shake)
                {
                    Point mouse = new Point(Math.Sin(resultPaint[resultPaint.Count() - 1]) * panelRight, Math.Sin(resultPaint[resultPaint.Count() - 1]) * panelHeight);
                    if (mouse.X > 0 && mouse.Y > 0 && mouse.X < panelRight && mouse.Y < panelHeight || true)
                    {
                        double
                            halfX = panelRight / 2,
                            halfY = panelHeight / 2;
                        double
                            offsetX = (halfX - mouse.X) / halfX * maxOffset,
                            offsetY = (halfY - mouse.Y) / halfY * maxOffset;

                        offsetXPix = (int)(curOffsetX + (offsetX - curOffsetX) * offsetSpeed);
                        offsetYPix = (int)(curOffsetY + (offsetY - curOffsetY) * offsetSpeed);

                        curOffsetX = offsetXPix;
                        curOffsetY = offsetYPix;
                    }
                }

                if (circleCurOffset > 360)
                    circleCurOffset = 0;
                double piOffset = circleCurOffset / 360f * Math.PI * 2;
                double radius = Math.Min(panelRight, panelHeight) / 2d;
                double halfradius = radius / 2;
                double scale = resultPaint.Average() / halfradius / 1 + 1;
                int resultPaintLength = resultPaint.Length;
                double swing = vm.iSwing / 100.0;
                Point[] points = Enumerable.Range(0, resultPaintLength).Select((v) =>
                {
                    double deg = (double)v / resultPaintLength * Math.PI * 2 + piOffset,
                        value = ((float)resultPaint[v] * swing),
                        sin = Math.Sin(deg),
                        cos = Math.Cos(deg);
                    Point p = new Point(
                        (centerX + halfradius * cos * scale + value * cos) + offsetXPix,
                        (centerY + halfradius * sin * scale + value * sin) + offsetYPix);
                    return p;
                }).ToArray();

                circleCurOffset += circleOffsetPerFrame;

                try
                {
                    
                    Color brushColor = (Color)ColorConverter.ConvertFromString(vm.BackColor);
                    SolidColorBrush colorBrush = new SolidColorBrush(brushColor);

                    Color PenColor = (Color)ColorConverter.ConvertFromString(vm.PenColor);
                    SolidColorBrush penBrush = new SolidColorBrush(PenColor);
                    Pen pen = new Pen(penBrush, vm.iPenSize);

                    //List<ArcSegment> segments = new List<ArcSegment>();
                    //foreach (var item in points)
                    //{
                    //    ArcSegment arcSeg = new ArcSegment();
                    //    arcSeg.Point = item;
                    //    arcSeg.Size = new Size(100, 50);
                    //
                    //    segments.Add(arcSeg);
                    //}
                    //{
                    //
                    //    ArcSegment arcSeg = new ArcSegment();
                    //    arcSeg.Point = points.First();
                    //    arcSeg.Size = new Size(50, 50);
                    //    segments.Add(arcSeg);
                    //}


                    List<LineSegment> segments = new List<LineSegment>();
                    foreach (var item in points)
                    {
                        var lineSegment = new LineSegment(item, true);
                        lineSegment.IsSmoothJoin = true;
                        lineSegment.IsStroked = true;
                        
                        segments.Add(lineSegment);
                        
                    }
                    //segments.Add(new LineSegment(points.First(), true));

                    PathFigure figure = new PathFigure(points.First(), segments, true);

                    PathGeometry myPathGeometry = new PathGeometry();
                    myPathGeometry.Figures.Add(figure);
                    dc.DrawGeometry(colorBrush, pen, myPathGeometry);
                    //g.DrawClosedCurve(globalDrawPen, points);
                }
                catch (Exception) { }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("OnRender " + e1.Message);
            }
        }
    }
}
