   M S B A M L   `   `   `  ����N  JDefaultPluginModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:DefaultPluginModuleDefaultPluginModule  5      I)clr-namespace:DefaultPluginModule.ConvertDefaultPluginModule.Convert  5      $    DefaultPluginModule.EffectView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:DefaultPluginModule     5      8vt)clr-namespace:DefaultPluginModule.Convert     5      -   5          � 	Resources  5      6   0DefaultPluginModule.Convert.BoolToVisibleConvert   vtBoolVisable0   *DefaultPluginModule.Convert.OpacityConvert  	vtOpacity3   -DefaultPluginModule.Convert.TimeStringConvert 
 vtTime3   -DefaultPluginModule.Convert.DateStringConvert 
 vtDate5   /DefaultPluginModule.Convert.ColorBrushConverter  vtColorBrush1   +DefaultPluginModule.Convert.FontNameConvert  
vtFontName1   +DefaultPluginModule.Convert.FontSizeConvert  
vtFontSize1   +DefaultPluginModule.Convert.FontBoldConvert  
vtFontBold3	   -DefaultPluginModule.Convert.FontItalicConvert  vtFontItalic��%  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �       5   
   6"     5   
   6     5   
   6     5   
   6     5   
   6!     5   
   6     5   
   6     5   
   6   	  5   
   6   5      .��5      � -   5         �Name$ plugin��5      .+�5   	   �� -   5         ��Name$ diyInfo��5      $��Center=�6%   $��Center=�5      ��60   �� *ShowDiyMargin+.+�5      �� -   5         ��Name$ DiyTxt��5       �� TextAlignment$ Center=�5#      ��5   '   �� *
sDiyText+y�5      �� *bShowDiyText+ �� 	Converter# [   J� Opacity 5      �� *iDiyOpacity+# [ ��5      �� *
txtColor+# [ ��5      �� *showDiyfont+# [ ��5       �� *showDiyfont+# [ ��5!      �� *showDiyfont+# [ ��5"      �� *fDiyFontSize+ J� Effect 5$      3
  -System.Windows.Media.Effects.DropShadowEffect
  5%       
  Color 6#   �� *ShadowColor+	 
  	Direction	 5&   #   �� *iShadowDirection+
 
  ShadowDepth
 5'   #   �� *iShadowDepth+ 
  
BlurRadius 5(   #   �� *iShadowBlurRadius+ 
  Opacity 5)   #   �� *iShadowOpacity+# [ 51      52      �� 53      -   5        $ plugInfo��53       �� Orientation$ Vertical=�6#   $��Center=�6:   $��Center=�54      ��60   �� *
ShowMargin+.+�55   
   �� -   5        $ TimeTxt��55      $ Center=�5<      ��55   &   �� *sShowDateTime1+ 56      �� *
iOpacity+# [ ��57      �� *	TimeColor+# [ ��58      �� *
showfont+# [ ��59      �� *
showfont+# [ ��5:      �� *
showfont+# [ ��5;      �� *fTimeFontSize+ 5C      
  5D       6'   �� *ShadowColor+	 5E   '   �� *iShadowDirection+
 5F   '   �� *iShadowDepth+ 5G   '   �� *iShadowOpacity+# [  5H   '   �� *iShadowBlurRadius+5J      �� 5K      -   5        $ DateTxt��5K      $ Center=�5R      ��5K   )   �� *sShowDateTime2+ 5L      �� *
iOpacity+# [ ��5M      �� *	DateColor+# [ ��5N      �� *
showfont+# [ ��5O      �� *
showfont+# [ ��5P      �� *
showfont+# [ ��5Q      �� *fDateFontSize+ 5[      
  5\       6'   �� *ShadowColor+	 5]   '   �� *iShadowDirection+
 5^   '   �� *iShadowDepth+ 5_   '   �� *iShadowBlurRadius+ 5`   '   �� *iShadowOpacity+# [ 5b      5d      5f      5g      