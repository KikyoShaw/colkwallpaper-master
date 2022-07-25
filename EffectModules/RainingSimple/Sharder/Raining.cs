
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace RainingSimpleEffect.SharderEffect {

    public class RainingSimple : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(RainingSimple), 0);
		public static readonly DependencyProperty IChannel1Property = ShaderEffect.RegisterPixelShaderSamplerProperty("IChannel1", typeof(RainingSimple), 1);
		public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(18D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty ResolutionXProperty = DependencyProperty.Register("ResolutionX", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(1280D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty ResolutionYProperty = DependencyProperty.Register("ResolutionY", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(1024D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
		public static readonly DependencyProperty UpDownReverseProperty = DependencyProperty.Register("UpDownReverse", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
		public static readonly DependencyProperty ImgHueProperty = DependencyProperty.Register("ImgHue", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(6)));
		public static readonly DependencyProperty ImgSatProperty = DependencyProperty.Register("ImgSat", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(7)));
		public static readonly DependencyProperty ImgLumProperty = DependencyProperty.Register("ImgLum", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(8)));
		public static readonly DependencyProperty BlueRadiusProperty = DependencyProperty.Register("BlueRadius", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(16D)), PixelShaderConstantCallback(9)));
		public static readonly DependencyProperty LightningTimeProperty = DependencyProperty.Register("LightningTime", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(10)));
		public static readonly DependencyProperty DropSizeProperty = DependencyProperty.Register("DropSize", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(400D)), PixelShaderConstantCallback(16)));
		public static readonly DependencyProperty DropReflectProperty = DependencyProperty.Register("DropReflect", typeof(Point), typeof(RainingSimple), new UIPropertyMetadata(new Point(100D, 0D), PixelShaderConstantCallback(17)));
		public static readonly DependencyProperty RainContrastProperty = DependencyProperty.Register("RainContrast", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(65D)), PixelShaderConstantCallback(18)));
		public static readonly DependencyProperty UsePostProcessingProperty = DependencyProperty.Register("UsePostProcessing", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(19)));
		public static readonly DependencyProperty RainSpeedProperty = DependencyProperty.Register("RainSpeed", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(20)));
		public static readonly DependencyProperty BlurPrecisionProperty = DependencyProperty.Register("BlurPrecision", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(50D)), PixelShaderConstantCallback(21)));
		public static readonly DependencyProperty BlurContrastProperty = DependencyProperty.Register("BlurContrast", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(22)));
		public static readonly DependencyProperty ZoomMoveProperty = DependencyProperty.Register("ZoomMove", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(23)));
		public static readonly DependencyProperty ZoomMovePicProperty = DependencyProperty.Register("ZoomMovePic", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(24)));
		public static readonly DependencyProperty OffxProperty = DependencyProperty.Register("Offx", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(25)));
		public static readonly DependencyProperty OffyProperty = DependencyProperty.Register("Offy", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(26)));
		public static readonly DependencyProperty RainintensityProperty = DependencyProperty.Register("Rainintensity", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(0.75D)), PixelShaderConstantCallback(27)));
		public static readonly DependencyProperty DropRainZoomProperty = DependencyProperty.Register("DropRainZoom", typeof(double), typeof(RainingSimple), new UIPropertyMetadata(((double)(260D)), PixelShaderConstantCallback(28)));
		public RainingSimple() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/RainingSimpleEffect;component/Resources/Effect/RainingSimple.ps", UriKind.Relative);
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
			this.UpdateShaderValue(BlueRadiusProperty);
			this.UpdateShaderValue(LightningTimeProperty);
			this.UpdateShaderValue(DropSizeProperty);
			this.UpdateShaderValue(DropReflectProperty);
			this.UpdateShaderValue(RainContrastProperty);
			this.UpdateShaderValue(UsePostProcessingProperty);
			this.UpdateShaderValue(RainSpeedProperty);
			this.UpdateShaderValue(BlurPrecisionProperty);
			this.UpdateShaderValue(BlurContrastProperty);
			this.UpdateShaderValue(ZoomMoveProperty);
			this.UpdateShaderValue(ZoomMovePicProperty);
			this.UpdateShaderValue(OffxProperty);
			this.UpdateShaderValue(OffyProperty);
			this.UpdateShaderValue(RainintensityProperty);
			this.UpdateShaderValue(DropRainZoomProperty);
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
		/// <summary>BlueRadius.</summary>
		public double BlueRadius {
			get {
				return ((double)(this.GetValue(BlueRadiusProperty)));
			}
			set {
				this.SetValue(BlueRadiusProperty, value);
			}
		}
		/// <summary>lightningTime.</summary>
		public double LightningTime {
			get {
				return ((double)(this.GetValue(LightningTimeProperty)));
			}
			set {
				this.SetValue(LightningTimeProperty, value);
			}
		}
		/// <summary>dropSize.</summary>
		public double DropSize {
			get {
				return ((double)(this.GetValue(DropSizeProperty)));
			}
			set {
				this.SetValue(DropSizeProperty, value);
			}
		}
		/// <summary>dropReflect.</summary>
		public Point DropReflect {
			get {
				return ((Point)(this.GetValue(DropReflectProperty)));
			}
			set {
				this.SetValue(DropReflectProperty, value);
			}
		}
		/// <summary>rainContrast.</summary>
		public double RainContrast {
			get {
				return ((double)(this.GetValue(RainContrastProperty)));
			}
			set {
				this.SetValue(RainContrastProperty, value);
			}
		}
		/// <summary>usePostProcessing.</summary>
		public double UsePostProcessing {
			get {
				return ((double)(this.GetValue(UsePostProcessingProperty)));
			}
			set {
				this.SetValue(UsePostProcessingProperty, value);
			}
		}
		/// <summary>rainSpeed.</summary>
		public double RainSpeed {
			get {
				return ((double)(this.GetValue(RainSpeedProperty)));
			}
			set {
				this.SetValue(RainSpeedProperty, value);
			}
		}
		/// <summary>blurPrecision.</summary>
		public double BlurPrecision {
			get {
				return ((double)(this.GetValue(BlurPrecisionProperty)));
			}
			set {
				this.SetValue(BlurPrecisionProperty, value);
			}
		}
		/// <summary>blurContrast.</summary>
		public double BlurContrast {
			get {
				return ((double)(this.GetValue(BlurContrastProperty)));
			}
			set {
				this.SetValue(BlurContrastProperty, value);
			}
		}
		/// <summary>zoomMove.</summary>
		public double ZoomMove {
			get {
				return ((double)(this.GetValue(ZoomMoveProperty)));
			}
			set {
				this.SetValue(ZoomMoveProperty, value);
			}
		}
		/// <summary>zoomMovePic.</summary>
		public double ZoomMovePic {
			get {
				return ((double)(this.GetValue(ZoomMovePicProperty)));
			}
			set {
				this.SetValue(ZoomMovePicProperty, value);
			}
		}
		/// <summary>offx.</summary>
		public double Offx {
			get {
				return ((double)(this.GetValue(OffxProperty)));
			}
			set {
				this.SetValue(OffxProperty, value);
			}
		}
		/// <summary>offy.</summary>
		public double Offy {
			get {
				return ((double)(this.GetValue(OffyProperty)));
			}
			set {
				this.SetValue(OffyProperty, value);
			}
		}
		/// <summary>rainintensity.</summary>
		public double Rainintensity {
			get {
				return ((double)(this.GetValue(RainintensityProperty)));
			}
			set {
				this.SetValue(RainintensityProperty, value);
			}
		}
		/// <summary>rainintensity.</summary>
		public double DropRainZoom {
			get {
				return ((double)(this.GetValue(DropRainZoomProperty)));
			}
			set {
				this.SetValue(DropRainZoomProperty, value);
			}
		}
	}
}
