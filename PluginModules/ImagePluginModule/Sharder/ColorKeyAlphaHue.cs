

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace ImagePluginModule.Shaders {

    public class ColorKeyAlphaHue : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorKeyAlphaHue), 0);
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty ColorKeyProperty = DependencyProperty.Register("ColorKey", typeof(Color), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(Color.FromArgb(255, 0, 128, 0), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty SatProperty = DependencyProperty.Register("Sat", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty LumProperty = DependencyProperty.Register("Lum", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(6)));
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(7)));
        public static readonly DependencyProperty UpDownReverseProperty = DependencyProperty.Register("UpDownReverse", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
        public static readonly DependencyProperty ImgshakeProperty = DependencyProperty.Register("Imgshake", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(9)));
        public static readonly DependencyProperty ImgshakeSpeedProperty = DependencyProperty.Register("ImgshakeSpeed", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(10)));
        public static readonly DependencyProperty ImgshakeSizeProperty = DependencyProperty.Register("ImgshakeSize", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(10D)), PixelShaderConstantCallback(11)));
        public static readonly DependencyProperty ImgshakeRangeProperty = DependencyProperty.Register("ImgshakeRange", typeof(double), typeof(ColorKeyAlphaHue), new UIPropertyMetadata(((double)(8D)), PixelShaderConstantCallback(12)));
        public ColorKeyAlphaHue()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/ImagePluginModule;component/Resources/ColorKeyAlphaHue.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(TimeProperty);
            this.UpdateShaderValue(ColorKeyProperty);
            this.UpdateShaderValue(ToleranceProperty);
            this.UpdateShaderValue(HueProperty);
            this.UpdateShaderValue(SatProperty);
            this.UpdateShaderValue(LumProperty);
            this.UpdateShaderValue(AlphaProperty);
            this.UpdateShaderValue(DirectionProperty);
            this.UpdateShaderValue(UpDownReverseProperty);
            this.UpdateShaderValue(ImgshakeProperty);
            this.UpdateShaderValue(ImgshakeSpeedProperty);
            this.UpdateShaderValue(ImgshakeSizeProperty);
            this.UpdateShaderValue(ImgshakeRangeProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        /// <summary>Time.</summary>
        public double Time
        {
            get
            {
                return ((double)(this.GetValue(TimeProperty)));
            }
            set
            {
                this.SetValue(TimeProperty, value);
            }
        }
        /// <summary>The color that becomes transparent.</summary>
        public Color ColorKey
        {
            get
            {
                return ((Color)(this.GetValue(ColorKeyProperty)));
            }
            set
            {
                this.SetValue(ColorKeyProperty, value);
            }
        }
        /// <summary>The tolerance in color differences.</summary>
        public double Tolerance
        {
            get
            {
                return ((double)(this.GetValue(ToleranceProperty)));
            }
            set
            {
                this.SetValue(ToleranceProperty, value);
            }
        }
        /// <summary>ImgHue.</summary>
        public double Hue
        {
            get
            {
                return ((double)(this.GetValue(HueProperty)));
            }
            set
            {
                this.SetValue(HueProperty, value);
            }
        }
        /// <summary>ImgSat.</summary>
        public double Sat
        {
            get
            {
                return ((double)(this.GetValue(SatProperty)));
            }
            set
            {
                this.SetValue(SatProperty, value);
            }
        }
        /// <summary>ImgLum.</summary>
        public double Lum
        {
            get
            {
                return ((double)(this.GetValue(LumProperty)));
            }
            set
            {
                this.SetValue(LumProperty, value);
            }
        }
        /// <summary>Alpha.</summary>
        public double Alpha
        {
            get
            {
                return ((double)(this.GetValue(AlphaProperty)));
            }
            set
            {
                this.SetValue(AlphaProperty, value);
            }
        }
        /// <summary>direction.</summary>
        public double Direction
        {
            get
            {
                return ((double)(this.GetValue(DirectionProperty)));
            }
            set
            {
                this.SetValue(DirectionProperty, value);
            }
        }
        /// <summary>UpDownReverse.</summary>
        public double UpDownReverse
        {
            get
            {
                return ((double)(this.GetValue(UpDownReverseProperty)));
            }
            set
            {
                this.SetValue(UpDownReverseProperty, value);
            }
        }
        /// <summary>Imgshake.</summary>
        public double Imgshake
        {
            get
            {
                return ((double)(this.GetValue(ImgshakeProperty)));
            }
            set
            {
                this.SetValue(ImgshakeProperty, value);
            }
        }
        /// <summary>ImgshakeSpeed.</summary>
        public double ImgshakeSpeed
        {
            get
            {
                return ((double)(this.GetValue(ImgshakeSpeedProperty)));
            }
            set
            {
                this.SetValue(ImgshakeSpeedProperty, value);
            }
        }
        /// <summary>ImgshakeSize.</summary>
        public double ImgshakeSize
        {
            get
            {
                return ((double)(this.GetValue(ImgshakeSizeProperty)));
            }
            set
            {
                this.SetValue(ImgshakeSizeProperty, value);
            }
        }
        /// <summary>ImgshakeRang.</summary>
        public double ImgshakeRange
        {
            get
            {
                return ((double)(this.GetValue(ImgshakeRangeProperty)));
            }
            set
            {
                this.SetValue(ImgshakeRangeProperty, value);
            }
        }
    }
}
