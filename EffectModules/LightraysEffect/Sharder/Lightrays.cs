
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace LightraysEffect.SharderEffect
{

    /// <summary>An effect that dims all but the brightest pixels.</summary>
    public class Lightrays : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Lightrays), 0);
		public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty ResolutionXProperty = DependencyProperty.Register("ResolutionX", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(1280D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty ResolutionYProperty = DependencyProperty.Register("ResolutionY", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(1024D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
		public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Lightrays), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(4)));
		public static readonly DependencyProperty OrgXProperty = DependencyProperty.Register("OrgX", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(0.6D)), PixelShaderConstantCallback(5)));
		public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(6)));

        public static readonly DependencyProperty ImgHueProperty = DependencyProperty.Register("ImgHue", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(7)));
        public static readonly DependencyProperty ImgSatProperty = DependencyProperty.Register("ImgSat", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(8)));
        public static readonly DependencyProperty ImgLumProperty = DependencyProperty.Register("ImgLum", typeof(double), typeof(Lightrays), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(9)));
        public Lightrays() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/LightraysEffect;component/Resources/Effect/Lightrays.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(TimeProperty);
			this.UpdateShaderValue(ResolutionXProperty);
			this.UpdateShaderValue(ResolutionYProperty);
			this.UpdateShaderValue(AlphaProperty);
			this.UpdateShaderValue(ColorProperty);
			this.UpdateShaderValue(OrgXProperty);
            this.UpdateShaderValue(DropDownProperty);
            this.UpdateShaderValue(ImgHueProperty);
            this.UpdateShaderValue(ImgSatProperty);
            this.UpdateShaderValue(ImgLumProperty);
        }
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		/// <summary>Time.</summary>
		public double Time {
			get {
				return ((double)(this.GetValue(TimeProperty)));
			}
			set {
				this.SetValue(TimeProperty, value);
			}
		}
		/// <summary>ResolutionX.</summary>
		public double ResolutionX {
			get {
				return ((double)(this.GetValue(ResolutionXProperty)));
			}
			set {
				this.SetValue(ResolutionXProperty, value);
			}
		}
		/// <summary>ResolutionY.</summary>
		public double ResolutionY {
			get {
				return ((double)(this.GetValue(ResolutionYProperty)));
			}
			set {
				this.SetValue(ResolutionYProperty, value);
			}
		}
		/// <summary>alpha.</summary>
		public double Alpha {
			get {
				return ((double)(this.GetValue(AlphaProperty)));
			}
			set {
				this.SetValue(AlphaProperty, value);
			}
		}
		public Color Color {
			get {
				return ((Color)(this.GetValue(ColorProperty)));
			}
			set {
				this.SetValue(ColorProperty, value);
			}
		}
		/// <summary>DropDown.</summary>
		public double OrgX {
			get {
				return ((double)(this.GetValue(OrgXProperty)));
			}
			set {
				this.SetValue(OrgXProperty, value);
			}
		}
		/// <summary>DropDown.</summary>
		public double DropDown {
			get {
				return ((double)(this.GetValue(DropDownProperty)));
			}
			set {
				this.SetValue(DropDownProperty, value);
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
    }
}
