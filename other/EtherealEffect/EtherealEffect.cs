
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace EtherealEffectModule {
	
	public class EtherealEffect : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(EtherealEffect), 0);
		public static readonly DependencyProperty IChannel1Property = ShaderEffect.RegisterPixelShaderSamplerProperty("IChannel1", typeof(EtherealEffect), 1);
		public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty ResolutionXProperty = DependencyProperty.Register("ResolutionX", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(1280D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty ResolutionYProperty = DependencyProperty.Register("ResolutionY", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(1024D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
		public static readonly DependencyProperty UpDownReverseProperty = DependencyProperty.Register("UpDownReverse", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
		public static readonly DependencyProperty ImgHueProperty = DependencyProperty.Register("ImgHue", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
		public static readonly DependencyProperty ImgSatProperty = DependencyProperty.Register("ImgSat", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(6)));
		public static readonly DependencyProperty ImgLumProperty = DependencyProperty.Register("ImgLum", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(7)));
		public static readonly DependencyProperty EffHueProperty = DependencyProperty.Register("EffHue", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
		public static readonly DependencyProperty EffSatProperty = DependencyProperty.Register("EffSat", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(9)));
		public static readonly DependencyProperty EffLumProperty = DependencyProperty.Register("EffLum", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(10)));
		public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(11)));
		public static readonly DependencyProperty AlphaModeProperty = DependencyProperty.Register("AlphaMode", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(12)));
		public static readonly DependencyProperty ImgGrayFilterProperty = DependencyProperty.Register("ImgGrayFilter", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(13)));
		public static readonly DependencyProperty ImgFilterColorProperty = DependencyProperty.Register("ImgFilterColor", typeof(Color), typeof(EtherealEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(14)));
		public static readonly DependencyProperty FiflterHeightProperty = DependencyProperty.Register("FiflterHeight", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(15)));
		public static readonly DependencyProperty ImgDirectionProperty = DependencyProperty.Register("ImgDirection", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(16)));
		public static readonly DependencyProperty ImgUpDownReverseProperty = DependencyProperty.Register("ImgUpDownReverse", typeof(double), typeof(EtherealEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(17)));
		public EtherealEffect() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/EtherealEffectModule;component/EtherealEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(IChannel1Property);
			this.UpdateShaderValue(TimeProperty);
			this.UpdateShaderValue(ResolutionXProperty);
			this.UpdateShaderValue(ResolutionYProperty);
			this.UpdateShaderValue(DirectionProperty);
			this.UpdateShaderValue(UpDownReverseProperty);
			this.UpdateShaderValue(ImgHueProperty);
			this.UpdateShaderValue(ImgSatProperty);
			this.UpdateShaderValue(ImgLumProperty);
			this.UpdateShaderValue(EffHueProperty);
			this.UpdateShaderValue(EffSatProperty);
			this.UpdateShaderValue(EffLumProperty);
			this.UpdateShaderValue(AlphaProperty);
			this.UpdateShaderValue(AlphaModeProperty);
			this.UpdateShaderValue(ImgGrayFilterProperty);
			this.UpdateShaderValue(ImgFilterColorProperty);
			this.UpdateShaderValue(FiflterHeightProperty);
			this.UpdateShaderValue(ImgDirectionProperty);
			this.UpdateShaderValue(ImgUpDownReverseProperty);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		public Brush IChannel1 {
			get {
				return ((Brush)(this.GetValue(IChannel1Property)));
			}
			set {
				this.SetValue(IChannel1Property, value);
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
		/// <summary>resolutionX.</summary>
		public double ResolutionX {
			get {
				return ((double)(this.GetValue(ResolutionXProperty)));
			}
			set {
				this.SetValue(ResolutionXProperty, value);
			}
		}
		/// <summary>resolutionY.</summary>
		public double ResolutionY {
			get {
				return ((double)(this.GetValue(ResolutionYProperty)));
			}
			set {
				this.SetValue(ResolutionYProperty, value);
			}
		}
		/// <summary>direction.</summary>
		public double Direction {
			get {
				return ((double)(this.GetValue(DirectionProperty)));
			}
			set {
				this.SetValue(DirectionProperty, value);
			}
		}
		/// <summary>UpDownReverse.</summary>
		public double UpDownReverse {
			get {
				return ((double)(this.GetValue(UpDownReverseProperty)));
			}
			set {
				this.SetValue(UpDownReverseProperty, value);
			}
		}
		/// <summary>ImgHue.</summary>
		public double ImgHue {
			get {
				return ((double)(this.GetValue(ImgHueProperty)));
			}
			set {
				this.SetValue(ImgHueProperty, value);
			}
		}
		/// <summary>ImgSat.</summary>
		public double ImgSat {
			get {
				return ((double)(this.GetValue(ImgSatProperty)));
			}
			set {
				this.SetValue(ImgSatProperty, value);
			}
		}
		/// <summary>ImgLum.</summary>
		public double ImgLum {
			get {
				return ((double)(this.GetValue(ImgLumProperty)));
			}
			set {
				this.SetValue(ImgLumProperty, value);
			}
		}
		/// <summary>EffHue.</summary>
		public double EffHue {
			get {
				return ((double)(this.GetValue(EffHueProperty)));
			}
			set {
				this.SetValue(EffHueProperty, value);
			}
		}
		/// <summary>EffSat.</summary>
		public double EffSat {
			get {
				return ((double)(this.GetValue(EffSatProperty)));
			}
			set {
				this.SetValue(EffSatProperty, value);
			}
		}
		/// <summary>EffLum.</summary>
		public double EffLum {
			get {
				return ((double)(this.GetValue(EffLumProperty)));
			}
			set {
				this.SetValue(EffLumProperty, value);
			}
		}
		/// <summary>The radius of the Poisson disk (in pixels).</summary>
		public double Alpha {
			get {
				return ((double)(this.GetValue(AlphaProperty)));
			}
			set {
				this.SetValue(AlphaProperty, value);
			}
		}
		/// <summary>alphaMode.</summary>
		public double AlphaMode {
			get {
				return ((double)(this.GetValue(AlphaModeProperty)));
			}
			set {
				this.SetValue(AlphaModeProperty, value);
			}
		}
		/// <summary>ImgGrayFilter.</summary>
		public double ImgGrayFilter {
			get {
				return ((double)(this.GetValue(ImgGrayFilterProperty)));
			}
			set {
				this.SetValue(ImgGrayFilterProperty, value);
			}
		}
		public Color ImgFilterColor {
			get {
				return ((Color)(this.GetValue(ImgFilterColorProperty)));
			}
			set {
				this.SetValue(ImgFilterColorProperty, value);
			}
		}
		/// <summary>FiflterHeight.</summary>
		public double FiflterHeight {
			get {
				return ((double)(this.GetValue(FiflterHeightProperty)));
			}
			set {
				this.SetValue(FiflterHeightProperty, value);
			}
		}
		/// <summary>imgDirection.</summary>
		public double ImgDirection {
			get {
				return ((double)(this.GetValue(ImgDirectionProperty)));
			}
			set {
				this.SetValue(ImgDirectionProperty, value);
			}
		}
		/// <summary>imgUpDownReverse.</summary>
		public double ImgUpDownReverse {
			get {
				return ((double)(this.GetValue(ImgUpDownReverseProperty)));
			}
			set {
				this.SetValue(ImgUpDownReverseProperty, value);
			}
		}
	}
}
