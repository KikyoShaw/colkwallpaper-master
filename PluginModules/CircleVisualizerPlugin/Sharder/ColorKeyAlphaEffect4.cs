
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace CircleVisualizerPlugin.Shaders {
	
	/// <summary>An effect that makes pixels of a particular color transparent.</summary>
	public class ColorKeyAlphaEffect4 : ShaderEffect {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorKeyAlphaEffect4), 0);
        public static readonly DependencyProperty Input1Property = ShaderEffect.RegisterPixelShaderSamplerProperty("Input1", typeof(ColorKeyAlphaEffect4), 1);
        public static readonly DependencyProperty ColorKeyProperty = DependencyProperty.Register("ColorKey", typeof(Color), typeof(ColorKeyAlphaEffect4), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty ColorizeProperty = DependencyProperty.Register("Colorize", typeof(double), typeof(ColorKeyAlphaEffect4), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(ColorKeyAlphaEffect4), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty SatProperty = DependencyProperty.Register("Sat", typeof(double), typeof(ColorKeyAlphaEffect4), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty LumProperty = DependencyProperty.Register("Lum", typeof(double), typeof(ColorKeyAlphaEffect4), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(ColorKeyAlphaEffect4), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(5)));
        public ColorKeyAlphaEffect4()
        {
            PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/CircleVisualizerPlugin;component/Resources/ColorKeyAlphaEffect4.ps", UriKind.Relative);
			this.PixelShader = pixelShader;


            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(Input1Property);
            this.UpdateShaderValue(ColorKeyProperty);
            this.UpdateShaderValue(ColorizeProperty);
            this.UpdateShaderValue(HueProperty);
            this.UpdateShaderValue(SatProperty);
            this.UpdateShaderValue(LumProperty);
            this.UpdateShaderValue(ToleranceProperty);
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
        public Brush Input1
        {
            get
            {
                return ((Brush)(this.GetValue(Input1Property)));
            }
            set
            {
                this.SetValue(Input1Property, value);
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
        /// <summary>Colorize.</summary>
        public double Colorize
        {
            get
            {
                return ((double)(this.GetValue(ColorizeProperty)));
            }
            set
            {
                this.SetValue(ColorizeProperty, value);
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
    }
}