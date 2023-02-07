using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BatEffect.Sharder
{

    /// <summary>An effect that makes pixels of a particular color transparent.</summary>
    public class LightraysChromeKey : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(LightraysChromeKey), 0);
        public static readonly DependencyProperty Input1Property = ShaderEffect.RegisterPixelShaderSamplerProperty("Input1", typeof(LightraysChromeKey), 1);
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty ResolutionXProperty = DependencyProperty.Register("ResolutionX", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1280D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty ResolutionYProperty = DependencyProperty.Register("ResolutionY", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1024D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty ColorKeyProperty = DependencyProperty.Register("ColorKey", typeof(Color), typeof(LightraysChromeKey), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty Alpha1Property = DependencyProperty.Register("Alpha1", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(5)));
        public static readonly DependencyProperty Alpha2Property = DependencyProperty.Register("Alpha2", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(6)));
        public static readonly DependencyProperty MaskColorChannelProperty = DependencyProperty.Register("MaskColorChannel", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(7)));
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
        public static readonly DependencyProperty UpDownReverseProperty = DependencyProperty.Register("UpDownReverse", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(9)));
        public static readonly DependencyProperty ImgDirectionProperty = DependencyProperty.Register("ImgDirection", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(10)));
        public static readonly DependencyProperty ImgUpDownReverseProperty = DependencyProperty.Register("ImgUpDownReverse", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(11)));
        public static readonly DependencyProperty ImgHueProperty = DependencyProperty.Register("ImgHue", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(12)));
        public static readonly DependencyProperty ImgSatProperty = DependencyProperty.Register("ImgSat", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(13)));
        public static readonly DependencyProperty ImgLumProperty = DependencyProperty.Register("ImgLum", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(14)));
        public static readonly DependencyProperty EffHueProperty = DependencyProperty.Register("EffHue", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(15)));
        public static readonly DependencyProperty EffSatProperty = DependencyProperty.Register("EffSat", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(16)));
        public static readonly DependencyProperty EffLumProperty = DependencyProperty.Register("EffLum", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(17)));
        public static readonly DependencyProperty EffColorProperty = DependencyProperty.Register("EffColor", typeof(Color), typeof(LightraysChromeKey), new UIPropertyMetadata(Color.FromArgb(255, 255, 255, 255), PixelShaderConstantCallback(18)));
        public static readonly DependencyProperty ColoursProperty = DependencyProperty.Register("Colours", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(19)));
        public static readonly DependencyProperty ToleranceBackProperty = DependencyProperty.Register("ToleranceBack", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(33)));
        public static readonly DependencyProperty ToleranceEdge1Property = DependencyProperty.Register("ToleranceEdge1", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(34)));
        public static readonly DependencyProperty ToleranceEdge2Property = DependencyProperty.Register("ToleranceEdge2", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(35)));
        public static readonly DependencyProperty MaximumColorProperty = DependencyProperty.Register("MaximumColor", typeof(double), typeof(LightraysChromeKey), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(36)));

        public LightraysChromeKey()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/BatEffect;component/Resources/Effect/LightraysChromeKey.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(Input1Property);
            this.UpdateShaderValue(TimeProperty);
            this.UpdateShaderValue(ResolutionXProperty);
            this.UpdateShaderValue(ResolutionYProperty);
            this.UpdateShaderValue(ColorKeyProperty);
            this.UpdateShaderValue(ToleranceProperty);
            this.UpdateShaderValue(Alpha1Property);
            this.UpdateShaderValue(Alpha2Property);
            this.UpdateShaderValue(MaskColorChannelProperty);
            this.UpdateShaderValue(DirectionProperty);
            this.UpdateShaderValue(UpDownReverseProperty);
            this.UpdateShaderValue(ImgDirectionProperty);
            this.UpdateShaderValue(ImgUpDownReverseProperty);
            this.UpdateShaderValue(ImgHueProperty);
            this.UpdateShaderValue(ImgSatProperty);
            this.UpdateShaderValue(ImgLumProperty);
            this.UpdateShaderValue(EffHueProperty);
            this.UpdateShaderValue(EffSatProperty);
            this.UpdateShaderValue(EffLumProperty);
            this.UpdateShaderValue(EffColorProperty);
            this.UpdateShaderValue(ColoursProperty);
            this.UpdateShaderValue(ToleranceBackProperty);
            this.UpdateShaderValue(ToleranceEdge1Property);
            this.UpdateShaderValue(ToleranceEdge2Property);
            this.UpdateShaderValue(MaximumColorProperty);
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
        /// <summary>ResolutionX.</summary>
        public double ResolutionX
        {
            get
            {
                return ((double)(this.GetValue(ResolutionXProperty)));
            }
            set
            {
                this.SetValue(ResolutionXProperty, value);
            }
        }
        /// <summary>ResolutionY.</summary>
        public double ResolutionY
        {
            get
            {
                return ((double)(this.GetValue(ResolutionYProperty)));
            }
            set
            {
                this.SetValue(ResolutionYProperty, value);
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
        /// <summary>The tolerance in color differences.</summary>
        public double Alpha1
        {
            get
            {
                return ((double)(this.GetValue(Alpha1Property)));
            }
            set
            {
                this.SetValue(Alpha1Property, value);
            }
        }
        /// <summary>The tolerance in color differences.</summary>
        public double Alpha2
        {
            get
            {
                return ((double)(this.GetValue(Alpha2Property)));
            }
            set
            {
                this.SetValue(Alpha2Property, value);
            }
        }
        /// <summary>The tolerance in color differences.</summary>
        public double MaskColorChannel
        {
            get
            {
                return ((double)(this.GetValue(MaskColorChannelProperty)));
            }
            set
            {
                this.SetValue(MaskColorChannelProperty, value);
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
        /// <summary>imgDirection.</summary>
        public double ImgDirection
        {
            get
            {
                return ((double)(this.GetValue(ImgDirectionProperty)));
            }
            set
            {
                this.SetValue(ImgDirectionProperty, value);
            }
        }
        /// <summary>imgUpDownReverse.</summary>
        public double ImgUpDownReverse
        {
            get
            {
                return ((double)(this.GetValue(ImgUpDownReverseProperty)));
            }
            set
            {
                this.SetValue(ImgUpDownReverseProperty, value);
            }
        }
        /// <summary>ImgHue.</summary>
        public double ImgHue
        {
            get
            {
                return ((double)(this.GetValue(ImgHueProperty)));
            }
            set
            {
                this.SetValue(ImgHueProperty, value);
            }
        }
        /// <summary>ImgSat.</summary>
        public double ImgSat
        {
            get
            {
                return ((double)(this.GetValue(ImgSatProperty)));
            }
            set
            {
                this.SetValue(ImgSatProperty, value);
            }
        }
        /// <summary>ImgLum.</summary>
        public double ImgLum
        {
            get
            {
                return ((double)(this.GetValue(ImgLumProperty)));
            }
            set
            {
                this.SetValue(ImgLumProperty, value);
            }
        }
        /// <summary>EffHue.</summary>
        public double EffHue
        {
            get
            {
                return ((double)(this.GetValue(EffHueProperty)));
            }
            set
            {
                this.SetValue(EffHueProperty, value);
            }
        }
        /// <summary>EffSat.</summary>
        public double EffSat
        {
            get
            {
                return ((double)(this.GetValue(EffSatProperty)));
            }
            set
            {
                this.SetValue(EffSatProperty, value);
            }
        }
        /// <summary>EffLum.</summary>
        public double EffLum
        {
            get
            {
                return ((double)(this.GetValue(EffLumProperty)));
            }
            set
            {
                this.SetValue(EffLumProperty, value);
            }
        }
        public Color EffColor
        {
            get
            {
                return ((Color)(this.GetValue(EffColorProperty)));
            }
            set
            {
                this.SetValue(EffColorProperty, value);
            }
        }
        /// <summary>colours.</summary>
        public double Colours
        {
            get
            {
                return ((double)(this.GetValue(ColoursProperty)));
            }
            set
            {
                this.SetValue(ColoursProperty, value);
            }
        }
        public double ToleranceBack
        {
            get
            {
                return ((double)(this.GetValue(ToleranceBackProperty)));
            }
            set
            {
                this.SetValue(ToleranceBackProperty, value);
            }
        }
        public double ToleranceEdge1
        {
            get
            {
                return ((double)(this.GetValue(ToleranceEdge1Property)));
            }
            set
            {
                this.SetValue(ToleranceEdge1Property, value);
            }
        }
        public double ToleranceEdge2
        {
            get
            {
                return ((double)(this.GetValue(ToleranceEdge2Property)));
            }
            set
            {
                this.SetValue(ToleranceEdge2Property, value);
            }
        }

        public double MaximumColor
        {
            get
            {
                return ((double)(this.GetValue(MaximumColorProperty)));
            }
            set
            {
                this.SetValue(MaximumColorProperty, value);
            }
        }
    }
}
