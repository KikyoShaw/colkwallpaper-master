using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace LightraysEffect.SharderEffect
{
	
	public class BrightBlursEffect : ShaderEffect 
	{
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BrightBlursEffect), 0);
		public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register("Threshold", typeof(double), typeof(BrightBlursEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty TimerProperty = DependencyProperty.Register("Timer", typeof(double), typeof(BrightBlursEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty RefractonProperty = DependencyProperty.Register("Refracton", typeof(double), typeof(BrightBlursEffect), new UIPropertyMetadata(((double)(50D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty VerticalTroughWidthProperty = DependencyProperty.Register("VerticalTroughWidth", typeof(double), typeof(BrightBlursEffect), new UIPropertyMetadata(((double)(23D)), PixelShaderConstantCallback(3)));
		public static readonly DependencyProperty Wobble2Property = DependencyProperty.Register("Wobble2", typeof(double), typeof(BrightBlursEffect), new UIPropertyMetadata(((double)(23D)), PixelShaderConstantCallback(4)));
		public BrightBlursEffect() 
		{
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/LightraysEffect;component/Resources/Effect/BrightBlursEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(ThresholdProperty);
			this.UpdateShaderValue(TimerProperty);
			this.UpdateShaderValue(RefractonProperty);
			this.UpdateShaderValue(VerticalTroughWidthProperty);
			this.UpdateShaderValue(Wobble2Property);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		/// <summary>Threshold below which values are discarded.</summary>
		public double Threshold {
			get {
				return ((double)(this.GetValue(ThresholdProperty)));
			}
			set {
				this.SetValue(ThresholdProperty, value);
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
	}
}
