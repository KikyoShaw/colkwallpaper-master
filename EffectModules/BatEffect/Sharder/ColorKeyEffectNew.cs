using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BatEffect.Sharder
{

    /// <summary>An effect that makes pixels of a particular color transparent.</summary>
    public class ColorKeyEffectNew : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorKeyEffectNew), 0);
		public static readonly DependencyProperty Input1Property = ShaderEffect.RegisterPixelShaderSamplerProperty("Input1", typeof(ColorKeyEffectNew), 1);
		public static readonly DependencyProperty ColorKeyProperty = DependencyProperty.Register("ColorKey", typeof(Color), typeof(ColorKeyEffectNew), new UIPropertyMetadata(Color.FromArgb(255, 0, 254, 25), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty MaskColorChannelProperty = DependencyProperty.Register("MaskColorChannel", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty ChanelTypeProperty = DependencyProperty.Register("ChanelType", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
		public static readonly DependencyProperty Alpha1Property = DependencyProperty.Register("Alpha1", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(4)));
		public static readonly DependencyProperty Alpha2Property = DependencyProperty.Register("Alpha2", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(5)));
		public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(6)));
		public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(7)));
		public static readonly DependencyProperty ImgDirectionProperty = DependencyProperty.Register("ImgDirection", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
		public static readonly DependencyProperty ImgUpDownReverseProperty = DependencyProperty.Register("ImgUpDownReverse", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(9)));
		public static readonly DependencyProperty ImgHueProperty = DependencyProperty.Register("ImgHue", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(10)));
		public static readonly DependencyProperty ImgSatProperty = DependencyProperty.Register("ImgSat", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(11)));
		public static readonly DependencyProperty ImgLumProperty = DependencyProperty.Register("ImgLum", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(12)));
		public static readonly DependencyProperty EffHueProperty = DependencyProperty.Register("EffHue", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(13)));
		public static readonly DependencyProperty EffSatProperty = DependencyProperty.Register("EffSat", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(14)));
		public static readonly DependencyProperty EffLumProperty = DependencyProperty.Register("EffLum", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(15)));
		public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(16)));
		public static readonly DependencyProperty UpDownReverseProperty = DependencyProperty.Register("UpDownReverse", typeof(double), typeof(ColorKeyEffectNew), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(17)));
		public ColorKeyEffectNew() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/BatEffect;component/Resources/Effect/ColorKeyEffectNew.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(Input1Property);
			this.UpdateShaderValue(ColorKeyProperty);
			this.UpdateShaderValue(ToleranceProperty);
			this.UpdateShaderValue(MaskColorChannelProperty);
			this.UpdateShaderValue(ChanelTypeProperty);
			this.UpdateShaderValue(Alpha1Property);
			this.UpdateShaderValue(Alpha2Property);
			this.UpdateShaderValue(KeyProperty);
			this.UpdateShaderValue(AlphaProperty);
			this.UpdateShaderValue(ImgDirectionProperty);
			this.UpdateShaderValue(ImgUpDownReverseProperty);
			this.UpdateShaderValue(ImgHueProperty);
			this.UpdateShaderValue(ImgSatProperty);
			this.UpdateShaderValue(ImgLumProperty);
			this.UpdateShaderValue(EffHueProperty);
			this.UpdateShaderValue(EffSatProperty);
			this.UpdateShaderValue(EffLumProperty);
			this.UpdateShaderValue(DirectionProperty);
			this.UpdateShaderValue(UpDownReverseProperty);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		public Brush Input1 {
			get {
				return ((Brush)(this.GetValue(Input1Property)));
			}
			set {
				this.SetValue(Input1Property, value);
			}
		}
		/// <summary>The color that becomes transparent.</summary>
		public Color ColorKey {
			get {
				return ((Color)(this.GetValue(ColorKeyProperty)));
			}
			set {
				this.SetValue(ColorKeyProperty, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double Tolerance {
			get {
				return ((double)(this.GetValue(ToleranceProperty)));
			}
			set {
				this.SetValue(ToleranceProperty, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double MaskColorChannel {
			get {
				return ((double)(this.GetValue(MaskColorChannelProperty)));
			}
			set {
				this.SetValue(MaskColorChannelProperty, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double ChanelType {
			get {
				return ((double)(this.GetValue(ChanelTypeProperty)));
			}
			set {
				this.SetValue(ChanelTypeProperty, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double Alpha1 {
			get {
				return ((double)(this.GetValue(Alpha1Property)));
			}
			set {
				this.SetValue(Alpha1Property, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double Alpha2 {
			get {
				return ((double)(this.GetValue(Alpha2Property)));
			}
			set {
				this.SetValue(Alpha2Property, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double Key {
			get {
				return ((double)(this.GetValue(KeyProperty)));
			}
			set {
				this.SetValue(KeyProperty, value);
			}
		}
		/// <summary>The tolerance in color differences.</summary>
		public double Alpha {
			get {
				return ((double)(this.GetValue(AlphaProperty)));
			}
			set {
				this.SetValue(AlphaProperty, value);
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
	}
}
