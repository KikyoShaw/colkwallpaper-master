using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace VirtualCharacterPlugin.Shader
{

    /// <summary>An effect that makes pixels of a particular color transparent.</summary>
    public class LumAlpha : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(LumAlpha), 0);
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(LumAlpha), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(LumAlpha), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty LuminosityProperty = DependencyProperty.Register("Luminosity", typeof(double), typeof(LumAlpha), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(double), typeof(LumAlpha), new UIPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(3)));
        
        public LumAlpha()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("VirtualCharacterPlugin;component/Resources/LumAlpha.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(HueProperty);
            this.UpdateShaderValue(SaturationProperty);
            this.UpdateShaderValue(LuminosityProperty);
            this.UpdateShaderValue(AlphaProperty);
        }

        public Brush Input
        {
            get => ((Brush)(this.GetValue(InputProperty)));
            set => this.SetValue(InputProperty, value);
        }

        /// <summary>The brightness offset.</summary>
        public double Hue
        {
            get => ((double)(this.GetValue(HueProperty)));
            set => this.SetValue(HueProperty, value);
        }

        /// <summary>The brightness offset.</summary>
        public double Saturation
        {
            get => ((double)(this.GetValue(SaturationProperty)));
            set => this.SetValue(SaturationProperty, value);
        }

        /// <summary>The brightness offset.</summary>
        public double Luminosity
        {
            get => ((double)(this.GetValue(LuminosityProperty)));
            set => this.SetValue(LuminosityProperty, value);
        }

        /// <summary>Alpha.</summary>
        public double Alpha
        {
            get => ((double)(this.GetValue(AlphaProperty)));
            set => this.SetValue(AlphaProperty, value);
        }
    }
}
