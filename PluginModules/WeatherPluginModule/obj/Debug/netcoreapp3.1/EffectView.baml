   M S B A M L   `   `   `  ����N  JWeatherPluginModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WeatherPluginModuleWeatherPluginModule  5      I)clr-namespace:WeatherPluginModule.ConvertWeatherPluginModule.Convert  5      V RCefSharp.Wpf, Version=89.0.170.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138A0clr-namespace:CefSharp.Wpf;assembly=CefSharp.WpfCefSharp.Wpf 5      $    WeatherPluginModule.EffectView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:WeatherPluginModule     5      8vt)clr-namespace:WeatherPluginModule.Convert     5      HcefSharpWPF0clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf     5      -   5          � 	Resources  5      6   0WeatherPluginModule.Convert.BoolToVisibleConvert   vtBoolVisable0   *WeatherPluginModule.Convert.OpacityConvert  	vtOpacity3   -WeatherPluginModule.Convert.TimeStringConvert 
 vtTime3   -WeatherPluginModule.Convert.DateStringConvert 
 vtDate5   /WeatherPluginModule.Convert.ColorBrushConverter  vtColorBrush1   +WeatherPluginModule.Convert.FontNameConvert  
vtFontName1   +WeatherPluginModule.Convert.FontSizeConvert  
vtFontSize1   +WeatherPluginModule.Convert.FontBoldConvert  
vtFontBold3	   -WeatherPluginModule.Convert.FontItalicConvert  vtFontItalic��%  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �       5   
   6"     5   
   6     5   
   6     5   
   6     5   
   6!     5   
   6     5   
   6     5   
   6   	  5   
   6   5      .��5      � -   5         �Name$ plugin��5      .+�5   
   � ��6   �� *	BackColor+ �� 	Converter# [ ��6Y   �� *
ShowMargin+��5      �� *
iShowWidth+��6,   �� *iShowHeight+ J� Opacity 5      �� *iBackOpacity+# [ 5      � 5   
   ��6   �� *
ShowMargin+��6-   �� *
iShowWidth+��6J   �� *iShowHeight+.+�5       � -   5        $ Loading��5       .+�5!      �� $"��加载天气中( Loading...)��6   $��20(�5"      $
��Bold"�6&   ���#ffffff$��Center=�5#   -   $��Center=�6H   5$      � 5%      -   5        $ Error��5%      $y�	Collapsed=�6"   .+�5&      �� $%��加载天气超时( Loading...)��6   $��20(�5'      $
��Bold"�6&   ���#ffffff$��Center=�5(   -   $��Center=�6H   5)      5*      � 5+   
   -   5        $ BrowersGrid��5+      ��6$   �� *
ShowMargin+��6B   �� *
iShowWidth+��6_   �� *iShowHeight+.+�5,      %
  CefSharp.Wpf.ChromiumWebBrowser
  -   5         
 Name$ Browers��5,   -    5-   )   �� *
iOpacity+# [  J� Effect 5.      3  -System.Windows.Media.Effects.DropShadowEffect  5/         Color 6'   �� *ShadowColor+   	Direction 50   '   �� *iShadowDirection+   ShadowDepth 51   '   �� *iShadowDepth+	   Opacity	 52   '   �� *iShadowOpacity+# [ 
   
BlurRadius
 53   '   �� *iShadowBlurRadius+55      57      58      59      