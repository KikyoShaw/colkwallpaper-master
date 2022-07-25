
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace RainingSimpleEffect.SharderEffect
{

    /// <summary>An effect that dims all but the brightest pixels.</summary>
    public class HueBlursEffect : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(HueBlursEffect), 0);
		public static readonly DependencyProperty TimerProperty = DependencyProperty.Register("Timer", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty RefractonProperty = DependencyProperty.Register("Refracton", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(50D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty VerticalTroughWidthProperty = DependencyProperty.Register("VerticalTroughWidth", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(23D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty Wobble2Property = DependencyProperty.Register("Wobble2", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(23D)), PixelShaderConstantCallback(4)));
		public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
		public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(6)));
		public static readonly DependencyProperty LuminosityProperty = DependencyProperty.Register("Luminosity", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(7)));
		public static readonly DependencyProperty ShowOrgProperty = DependencyProperty.Register("ShowOrg", typeof(double), typeof(HueBlursEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
		public HueBlursEffect() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/RainingSimpleEffect;component/Resources/Effect/HueBlursEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(TimerProperty);
			this.UpdateShaderValue(RefractonProperty);
			this.UpdateShaderValue(VerticalTroughWidthProperty);
			this.UpdateShaderValue(Wobble2Property);
			this.UpdateShaderValue(HueProperty);
			this.UpdateShaderValue(SaturationProperty);
			this.UpdateShaderValue(LuminosityProperty);
			this.UpdateShaderValue(ShowOrgProperty);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		public double Timer {
			get {
				return ((double)(this.GetValue(TimerProperty)));
			}
			set {
				this.SetValue(TimerProperty, value);
			}
		}
		/// <summary>Refraction Amount.</summary>
		public double Refracton {
			get {
				return ((double)(this.GetValue(RefractonProperty)));
			}
			set {
				this.SetValue(RefractonProperty, value);
			}
		}
		/// <summary>Vertical trough</summary>
		public double VerticalTroughWidth {
			get {
				return ((double)(this.GetValue(VerticalTroughWidthProperty)));
			}
			set {
				this.SetValue(VerticalTroughWidthProperty, value);
			}
		}
		/// <summary>Center X of the Zoom.</summary>
		public double Wobble2 {
			get {
				return ((double)(this.GetValue(Wobble2Property)));
			}
			set {
				this.SetValue(Wobble2Property, value);
			}
		}
		/// <summary>The brightness offset.</summary>
		public double Hue {
			get {
				return ((double)(this.GetValue(HueProperty)));
			}
			set {
				this.SetValue(HueProperty, value);
			}
		}
		/// <summary>The brightness offset.</summary>
		public double Saturation {
			get {
				return ((double)(this.GetValue(SaturationProperty)));
			}
			set {
				this.SetValue(SaturationProperty, value);
			}
		}
		/// <summary>The brightness offset.</summary>
		public double Luminosity {
			get {
				return ((double)(this.GetValue(LuminosityProperty)));
			}
			set {
				this.SetValue(LuminosityProperty, value);
			}
		}
		/// <summary>The brightness offset.</summary>
		public double ShowOrg {
			get {
				return ((double)(this.GetValue(ShowOrgProperty)));
			}
			set {
				this.SetValue(ShowOrgProperty, value);
			}
		}
	}
}
