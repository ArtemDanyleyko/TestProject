using BuildApps.Core.Mobile.FormsKit.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;
using BindExtensions = BuildApps.Core.Mobile.FormsKit.Extensions.BindableObjectExtensions;

namespace TestProject.UI.Controls
{
    public partial class RatingControl
    {
        private const float Cos35 = 0.82f;
        private const float LowestPointY = 100f;
        private const float HighestPointY = -253f;
        private const float LeftmostPointY = -126.7f;
        private const float LeftmostPointX = -191f;
        private const float OnePercentWight = 3.82f;
        private const float StarWight = 382f;
        private const float StarsWight = 1910f;
        private const float StarHight = 353f;
        private const int StarsCount = 5;
        private const int MaxFillValue = 100;

        public static readonly BindableProperty FillColorProperty = BindExtensions.Create<Color, RatingControl>(nameof(FillColor), p => p.OnFillColorChanged, Color.Gray);
        public static readonly BindableProperty StrokeColorProperty = BindExtensions.Create<Color, RatingControl>(nameof(StrokeColor), p => p.OnStrokeColorChanged, Color.Gold);
        public static readonly BindableProperty ValueProperty = BindExtensions.Create<int, RatingControl>(nameof(Value), p => p.OnValueChanged);

        private readonly Point[] starVertexCordinaties;
        private readonly SKRect bounds;

        private SKPaint fillPaint;
        private SKPaint strokePaint;

        public RatingControl()
        {
            InitializeComponent();

            starVertexCordinaties = new[]
            {
                new Point(0 , -253),
                new Point(46, -126.7f ),
                new Point(191, -126.7f),
                new Point(76, -38.7f),
                new Point(132, 100),
                new Point(0 , 7.3f),
                new Point(0 , -253),
                new Point(-46, -126.7f ),
                new Point(-191, -126.7f),
                new Point(-76, -38.7f),
                new Point(-132, 100),
                new Point(0 , 7.3f),
            };

            fillPaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = StrokeColor.ToSKColor(),
                StrokeWidth = 5,
                IsAntialias = true
            };

            strokePaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = FillColor.ToSKColor(),
                StrokeWidth = 5,
                IsAntialias = true
            };

            bounds = new SKRect(LeftmostPointX, HighestPointY, StarsWight, StarHight);
        }

        public int Value
        {
            get => this.GetProperty<int>();
            set => this.SetProperty(value);
        }

        public Color FillColor
        {
            get => this.GetProperty<Color>();
            set => this.SetProperty(value);
        }

        public Color StrokeColor
        {
            get => this.GetProperty<Color>();
            set => this.SetProperty(value);
        }

        private void Draw(SKCanvas canvas, SKImageInfo info)
        {
            var emptyStars = new SKPath();

            canvas.Clear();

            for (var i = 0; i < StarsCount; i++)
            {
                DrawStar(
                    emptyStars,
                    starVertexCordinaties,
                    Math.Abs(LeftmostPointX) * i * 2);
            }

            emptyStars.GetBounds(out var ssss);

            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(1 * Math.Min(info.Width / ssss.Width, info.Height / ssss.Height));
            canvas.Translate(-ssss.MidX, -ssss.MidY);

            canvas.DrawPath(emptyStars, strokePaint);
            canvas.DrawPath(GetFillStars(), fillPaint);
        }

        private void OnCanvasViewPaintSurface(object _, SKPaintSurfaceEventArgs args) =>
            Draw(args.Surface.Canvas, args.Info);

        private void DrawStar(
            SKPath starPath,
            Point[] starCoordinates,
            float offsetX)
        {
            starPath.MoveTo((float)starCoordinates[0].X + offsetX, (float)starCoordinates[0].Y);

            for (var i = 1; i < starCoordinates.Length; i++)
            {
                starPath.LineTo((float)starCoordinates[i].X + offsetX, (float)starCoordinates[i].Y);
            }
        }

        private SKPath GetFillStars()
        {
            var fillStars = new SKPath();
            var maxStarsCount = Math.Ceiling(Value / (double)(100 / StarsCount));

            for (var j = 0; j < maxStarsCount; j++)
            {
                var offsetX = j * StarWight;
                var currentLeftmostPositionX = LeftmostPointX + offsetX;
                var starFillValue = MaxFillValue;
                if (j == maxStarsCount - 1)
                {
                    starFillValue = (Value - MaxFillValue / StarsCount * j) * StarsCount;
                }

                for (var i = 0; i < starFillValue; i++)
                {
                    var lineToX = i * OnePercentWight + currentLeftmostPositionX;
                    var lineToXDefault = i * OnePercentWight + LeftmostPointX;

                    PaintMiddlePartStar(
                        fillStars,
                        lineToX,
                        lineToXDefault,
                        offsetX);
                    PaintTopAndBottomStar(
                        fillStars,
                        lineToX,
                        lineToXDefault,
                        offsetX);
                }
            }

            return fillStars;
        }

        private void PaintMiddlePartStar(
            SKPath fillStars,
            float lineToX,
            float lineToXDefault,
            float offsetX)
        {
            if (lineToX <= -46 + offsetX || lineToX >= 46 + offsetX)
            {
                var x = Math.Abs(LeftmostPointX) - Math.Abs(lineToXDefault);
                var y = LeftmostPointY + (x * Cos35);

                fillStars.MoveTo(lineToX, LeftmostPointY);
                fillStars.LineTo(lineToX, y);
            }
        }

        private void PaintTopAndBottomStar(
            SKPath fillStars,
            float lineToX,
            float lineToXDefault,
            float offsetX)
        {
            if (lineToX >= -132 + offsetX && lineToX <= 132 + offsetX)
            {
                var heightToWeightUpperBound = 2.7f;
                var heightToWeightBottomBound = 0.7f;

                var fromYLength = (132 - Math.Abs(lineToXDefault)) * heightToWeightUpperBound;
                var toYLength = (132 - Math.Abs(lineToXDefault)) * heightToWeightBottomBound;

                var fromY = Math.Abs(LowestPointY) - fromYLength;
                var toY = Math.Abs(LowestPointY) - toYLength;

                fillStars.MoveTo(lineToX, fromY);
                fillStars.LineTo(lineToX, toY);
            }
        }

        private void OnStrokeColorChanged(Color _, Color newValue)
        {
            fillPaint.Color = newValue.ToSKColor();
        }

        private void OnFillColorChanged(Color _, Color newValue)
        {
            strokePaint.Color = newValue.ToSKColor();
        }

        private void OnValueChanged(int _, int __)
        {
            InvalidateSurface();
        }
    }
}
