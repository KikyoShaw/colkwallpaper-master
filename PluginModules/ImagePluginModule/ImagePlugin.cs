using EffectConfigModule.SpectrumHelp;
using ImagePluginModule.Shaders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImagePluginModule
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public class AnimationInfo
    {
        public bool bPower { get; set; } = false;
        public bool bRotateContinuous { get; set; } = false;
        public double speed { get; set; } = 0;
        public double range { get; set; } = 0;
        public double minValue { get; set; } = 0;
        public double begValue { get; set; } = 0;
        public double endValue { get; set; } = 0;
        public double curValue { get; set; } = 0;
        public double angle { get; set; } = 0;
        public double orgValue { get; set; } = 0;

    }
    public partial class ImagePlugin : Image
    {
        private ViewModel.EffectViewModel vm = ViewModel.EffectViewModel.Instance;
        ColorKeyAlphaHue _colorKeyEffect = new ColorKeyAlphaHue();
        //Image imageCtrl = new Image();
        string _sLastLogoPath = "";
        BitmapImage _curImage = null;
        Point _showPoint = new Point(0, 0);
        Size _showSize = new Size(100, 100);
        double _curHue = 1.0;
        public Thickness _thickness = new Thickness(0, 0, 0, 0);
        RotateTransform _rotate = new RotateTransform();
        bool _bimgsake = false;
        AnimationInfo _alphaAnimation = new AnimationInfo();
        AnimationInfo _hueAnimation = new AnimationInfo();
        AnimationInfo _sizeAnimation = new AnimationInfo();
        AnimationInfo _moveXAnimation = new AnimationInfo();
        AnimationInfo _moveyAnimation = new AnimationInfo();
        AnimationInfo _rotateAnimation = new AnimationInfo();
        double _timeTick = 0;

        public ImagePlugin()
        {
            this.MinWidth = 1;
            this.MinHeight = 1;
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            //Child = imageCtrl;
            //this.Children.Add(imageCtrl);
            //Focusable = true;
            Stretch = Stretch.Uniform;
            Source = vm.GetDefaultBitmap();
            Effect = _colorKeyEffect;
            _colorKeyEffect.Tolerance = 0;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            RenderTransformOrigin = new Point(0.5, 0.5);
            this.RenderTransform = _rotate;


            RefreshPostion();
            if(_curImage!=null)
                Source = _curImage;
        }
        public void OnAnimationTime()
        {

            bool updatePostion = false;
            if (_bimgsake)
            {
                _timeTick += 0.01;
                _colorKeyEffect.Time = _timeTick;

            }
            if (_alphaAnimation.bPower)
            {
                _alphaAnimation.angle += _alphaAnimation.speed;
                _alphaAnimation.curValue = (Math.Sin(_alphaAnimation.angle) * (_alphaAnimation.range));
                _colorKeyEffect.Alpha = (_alphaAnimation.minValue + _alphaAnimation.curValue) / 1000;
            }
            if (_hueAnimation.bPower)
            {
                _hueAnimation.angle += _hueAnimation.speed;
                if (_hueAnimation.bRotateContinuous)
                {
                    _hueAnimation.curValue = (_hueAnimation.curValue + _hueAnimation.speed * 100.0) %360;
                    _colorKeyEffect.Hue = _hueAnimation.curValue;
                }
                else
                {
                    _hueAnimation.curValue = (Math.Sin(_hueAnimation.angle) * (_hueAnimation.range));
                    _colorKeyEffect.Hue = (_hueAnimation.minValue + _hueAnimation.curValue) / 10.0;
                }
            }
            if (_sizeAnimation.bPower)
            {
                _sizeAnimation.angle += _sizeAnimation.speed;
                _sizeAnimation.curValue = (Math.Sin(_sizeAnimation.angle) * (_sizeAnimation.range));

                updatePostion = true;
            }
            if (_moveXAnimation.bPower)
            {
                _moveXAnimation.angle += _moveXAnimation.speed;
                _moveXAnimation.curValue = (Math.Sin(_moveXAnimation.angle) * (_moveXAnimation.range));

                updatePostion = true;
            }
            if (_moveyAnimation.bPower)
            {
                _moveyAnimation.angle += _moveyAnimation.speed;
                _moveyAnimation.curValue = (Math.Sin(_moveyAnimation.angle) * (_moveyAnimation.range));

                updatePostion = true;
            }
            if (_rotateAnimation.bPower)
            {
                _rotateAnimation.angle += _rotateAnimation.speed;
                if(_rotateAnimation.bRotateContinuous)
                {
                    _rotateAnimation.curValue = (_rotateAnimation.curValue+ _rotateAnimation.speed*100.0)%360 ;
                    _rotate.Angle = _rotateAnimation.curValue;
                }
                else
                {
                    _rotateAnimation.curValue = (Math.Sin(_rotateAnimation.angle) * (_rotateAnimation.range));
                    _rotate.Angle = (_rotateAnimation.minValue + _rotateAnimation.curValue) % 360;
                }
            }
            if (updatePostion)
                RefreshPostion();
        }
        public void OnValueChange(string key, string value)
        {
            try
            {

                bool updatePostion = false;
                switch (key)
                {
                    case "zIndex":
                        {
                            SetZIndex(int.Parse(value));
                        }
                        break;
                    case "Image":
                        {
                            SetImage(value);
                        }
                        break;
                    case "ImageStrength":
                        {
                            int mode = int.Parse(value);
                            switch (mode)
                            {
                                case 1:
                                    Stretch = Stretch.Fill;
                                    break;
                                case 2:
                                    Stretch = Stretch.Uniform;
                                    break;

                                case 3:
                                    Stretch = Stretch.UniformToFill;
                                    break;
                                default:
                                    Stretch = Stretch.Uniform;
                                    break;
                            }
                        }
                        break;
                    case "HorizontalOffset":
                        {
                            SetPointLeft(double.Parse(value));
                            updatePostion = true;
                        }
                        break;
                    case "VerticalOffset":
                        {
                            SetPointTop(double.Parse(value));
                            updatePostion = true;
                        }
                        break;
                    case "ShowWidthRatio":
                        {
                            SetWidthRatio(double.Parse(value));
                            if(!_sizeAnimation.bPower)
                                updatePostion = true;
                        }
                        break;
                    case "ShowHeightRatio":
                        {
                            SetHeightRatio(double.Parse(value));
                            if (!_sizeAnimation.bPower)
                                updatePostion = true;
                        }
                        break;
                    case "Sat":
                        {
                            _colorKeyEffect.Sat = (double)((int.Parse(value))) / 1000.0;
                        }
                        break;
                    case "Lum":
                        {
                            _colorKeyEffect.Lum = (double)((int.Parse(value))) / 1000.0;
                        }
                        break;
                    case "FilterColor":
                        {
                            _colorKeyEffect.ColorKey = (Color)ColorConverter.ConvertFromString(value);
                        }
                        break;
                    case "FilterTolerance":
                        {
                            _colorKeyEffect.Tolerance = (double)((int.Parse(value))) / 100.0;
                        }
                        break;
                    case "Direction":
                        {
                            _colorKeyEffect.Direction = bool.Parse(value) ? 1: 0;
                        }
                        break;
                    case "UpDownReverse":
                        {
                            _colorKeyEffect.UpDownReverse = bool.Parse(value) ? 1 : 0;
                        }
                        break;

                    case "Imgshake":
                        {
                            _bimgsake = bool.Parse(value);
                            _colorKeyEffect.Imgshake = (double)((_bimgsake ? 1.0 : 0.0));                            
                        }
                        break;
                    case "shakeSpeed":
                        {
                            _colorKeyEffect.ImgshakeSpeed = (double)((int.Parse(value))) / 100.0;
                        }
                        break;
                    case "shakeSize":
                        {
                            _colorKeyEffect.ImgshakeSize = (double)((int.Parse(value)));
                        }
                        break;
                    case "shakeRange":
                        {
                            _colorKeyEffect.ImgshakeRange = (double)((int.Parse(value))) / 10.0;
                        }
                        break;
                    case "Alpha":
                        {
                            _alphaAnimation.orgValue = (double.Parse(value) / 1000);
                            if(!_alphaAnimation.bPower)
                                _colorKeyEffect.Alpha = _alphaAnimation.orgValue;
                        }
                        break;
                    case "AlphaAnimation":
                        {
                            _alphaAnimation.bPower = bool.Parse(value);
                            if (!_alphaAnimation.bPower)
                                _colorKeyEffect.Alpha = _alphaAnimation.orgValue;
                        }
                        break;
                    case "AlphaAnimationRangeBeg":
                        {
                            _alphaAnimation.begValue = (double.Parse(value));
                            _alphaAnimation.range = Math.Abs(_alphaAnimation.endValue - _alphaAnimation.begValue) * 0.5;
                            _alphaAnimation.minValue = Math.Min(_alphaAnimation.begValue, _alphaAnimation.endValue)+_alphaAnimation.range;
                        }
                        break;
                    case "AlphaAnimationRangeEnd":
                        {
                            _alphaAnimation.endValue = (double.Parse(value));
                            _alphaAnimation.range = Math.Abs(_alphaAnimation.endValue - _alphaAnimation.begValue) * 0.5;
                            _alphaAnimation.minValue = Math.Min(_alphaAnimation.begValue, _alphaAnimation.endValue)+_alphaAnimation.range;
                        }
                        break;
                    case "AlphaAnimationSpeed":
                        {
                            _alphaAnimation.speed = (double.Parse(value) / 10000);
                        }
                        break;
                    case "Hue":
                        {
                            _hueAnimation.orgValue = (double)((int.Parse(value))) / 10.0;
                            if (!_hueAnimation.bPower)
                                _colorKeyEffect.Hue = _hueAnimation.orgValue;
                        }
                        break;
                    case "HueAnimation":
                        {
                            _hueAnimation.bPower = bool.Parse(value);
                            if (!_hueAnimation.bPower)
                                _colorKeyEffect.Hue = _hueAnimation.orgValue;
                        }
                        break;
                    case "HueContinuous":
                        {
                            _hueAnimation.bRotateContinuous = bool.Parse(value);
                        }
                        break;
                    case "HueAnimationRangeBeg":
                        {
                            _hueAnimation.begValue = (double.Parse(value));
                            _hueAnimation.range = Math.Abs(_hueAnimation.endValue - _hueAnimation.begValue) * 0.5;
                            _hueAnimation.minValue = Math.Min(_hueAnimation.begValue, _hueAnimation.endValue)+_hueAnimation.range;
                        }
                        break;
                    case "HueAnimationRangeEnd":
                        {
                            _hueAnimation.endValue = (double.Parse(value));
                            _hueAnimation.range = Math.Abs(_hueAnimation.endValue - _hueAnimation.begValue) * 0.5;
                            _hueAnimation.minValue = Math.Min(_hueAnimation.begValue, _hueAnimation.endValue)+_hueAnimation.range;
                        }
                        break;
                    case "HueAnimationSpeed":
                        {
                            _hueAnimation.speed = (double.Parse(value) / 10000);
                        }
                        break;

                    case "SizeAnimation":
                        {
                            _sizeAnimation.bPower = bool.Parse(value);
                        }
                        break;
                    case "SizeAnimationRangeBeg":
                        {
                            _sizeAnimation.begValue = (double.Parse(value));
                            _sizeAnimation.range = Math.Abs(_sizeAnimation.endValue - _sizeAnimation.begValue) * 0.5;
                            _sizeAnimation.minValue = Math.Min(_sizeAnimation.begValue, _sizeAnimation.endValue)+_sizeAnimation.range;
                        }
                        break;
                    case "SizeAnimationRangeEnd":
                        {
                            _sizeAnimation.endValue = (double.Parse(value));
                            _sizeAnimation.range = Math.Abs(_sizeAnimation.endValue - _sizeAnimation.begValue) * 0.5;
                            _sizeAnimation.minValue = Math.Min(_sizeAnimation.begValue, _sizeAnimation.endValue)+_sizeAnimation.range;
                        }
                        break;
                    case "SizeAnimationSpeed":
                        {
                            _sizeAnimation.speed = (double.Parse(value) / 10000);
                        }
                        break;
                    case "MoveXAnimation":
                        {
                            _moveXAnimation.bPower = bool.Parse(value);
                        }
                        break;
                    case "MoveXAnimationRangeBeg":
                        {
                            _moveXAnimation.begValue = (double.Parse(value));
                            _moveXAnimation.range = Math.Abs(_moveXAnimation.endValue - _moveXAnimation.begValue) * 0.5;
                            _moveXAnimation.minValue = Math.Min(_moveXAnimation.begValue, _moveXAnimation.endValue)+_moveXAnimation.range;
                        }
                        break;
                    case "MoveXAnimationRangeEnd":
                        {
                            _moveXAnimation.endValue = (double.Parse(value));
                            _moveXAnimation.range = Math.Abs(_moveXAnimation.endValue - _moveXAnimation.begValue) * 0.5;
                            _moveXAnimation.minValue = Math.Min(_moveXAnimation.begValue, _moveXAnimation.endValue)+_moveXAnimation.range;
                        }
                        break;
                    case "MoveXAnimationSpeed":
                        {
                            _moveXAnimation.speed = (double.Parse(value) / 10000);
                        }
                        break;

                    case "MoveYAnimation":
                        {
                            _moveyAnimation.bPower = bool.Parse(value);
                        }
                        break;
                    case "MoveYAnimationRangeBeg":
                        {
                            _moveyAnimation.begValue = (double.Parse(value));
                            _moveyAnimation.range = Math.Abs(_moveyAnimation.endValue - _moveyAnimation.begValue) * 0.5;
                            _moveyAnimation.minValue = Math.Min(_moveyAnimation.begValue, _moveyAnimation.endValue) + _moveyAnimation.range;
                        }
                        break;
                    case "MoveYAnimationRangeEnd":
                        {
                            _moveyAnimation.endValue = (double.Parse(value));
                            _moveyAnimation.range = Math.Abs(_moveyAnimation.endValue - _moveyAnimation.begValue) * 0.5;
                            _moveyAnimation.minValue = Math.Min(_moveyAnimation.begValue, _moveyAnimation.endValue) + _moveyAnimation.range;
                        }
                        break;
                    case "MoveYAnimationSpeed":
                        {
                            _moveyAnimation.speed = (double.Parse(value) / 10000);
                        }
                        break;

                    case "ShowAngle":
                        {
                            _rotateAnimation.orgValue = double.Parse(value);
                            if (!_rotateAnimation.bPower)
                                _rotate.Angle = _rotateAnimation.orgValue;
                        }
                        break;
                    case "RotateAnimation":
                        {
                            _rotateAnimation.bPower = bool.Parse(value);
                            if (!_rotateAnimation.bPower)
                                _rotate.Angle = _rotateAnimation.orgValue;
                        }
                        break;
                    case "RotateContinuous":
                        {
                            _rotateAnimation.bRotateContinuous = bool.Parse(value);
                        }
                        break;
                    case "RotateAnimationRangeBeg":
                        {
                            _rotateAnimation.begValue = (double.Parse(value));
                            _rotateAnimation.range = Math.Abs(_rotateAnimation.endValue - _rotateAnimation.begValue) * 0.5;
                            _rotateAnimation.minValue = Math.Min(_rotateAnimation.begValue, _rotateAnimation.endValue) + _rotateAnimation.range;
                        }
                        break;
                    case "RotateAnimationRangeEnd":
                        {
                            _rotateAnimation.endValue = (double.Parse(value));
                            _rotateAnimation.range = Math.Abs(_rotateAnimation.endValue - _rotateAnimation.begValue) * 0.5;
                            _rotateAnimation.minValue = Math.Min(_rotateAnimation.begValue, _rotateAnimation.endValue) + _rotateAnimation.range;
                        }
                        break;
                    case "RotateAnimationSpeed":
                        {
                            _rotateAnimation.speed = (double.Parse(value) / 10000);
                        }
                        break;
                    default:
                        break;
                }
                System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (updatePostion)
                        RefreshPostion();
                });
            }
            catch
            {

            }
        }
        public void SetZIndex(int zIndex)
        {
            this.SetValue(Panel.ZIndexProperty, zIndex);
        }
        public void SetImage(string source)
        {
            try
            {
                if (!string.IsNullOrEmpty(source))
                {
                    //MessageBox.Show($"CentenImage {_sLastLogoPath} ?? {value}");
                    if (File.Exists(source))
                    {
                        _curImage = HYWEffectConfig.AssistHelp.LoadLocalImage(source, (int)this.ActualWidth, (int)this.ActualHeight);
                        if (_curImage != null)
                        {

                            GC.Collect();
                            _sLastLogoPath = source;
                            Source = _curImage;
                            return;
                        }

                    }
                }

            }
            catch
            {

            }


            Source = vm.GetDefaultBitmap();
            
        }
        public void SetPointLeft(double left)
        {
            _showPoint.X = left;
            //RefreshPostion();
        }
        public void SetPointTop(double top)
        {
            _showPoint.Y = top;
            //RefreshPostion();
        }
        public void SetWidthRatio(double widthRatio)
        {
            _showSize.Width = widthRatio;
            //RefreshPostion();
        }
        public void SetHeightRatio(double heightRatio)
        {
            _showSize.Height = heightRatio;
            //RefreshPostion();
        }
        public void RefreshPostion()
        {
            var parendGrid = (Grid)this.Parent;
            if(!_sizeAnimation.bPower)
            {
                this.Width = (parendGrid.ActualWidth * (_showSize.Width / 1000.0));
                this.Height = (parendGrid.ActualHeight * (_showSize.Height / 1000.0));

                if(!_moveXAnimation.bPower)
                {
                    double hov = ((double)_showPoint.X / 500 * (parendGrid.ActualWidth - this.Width));
                    _thickness.Left = hov;
                }
                else
                {
                    double hov = ((double)(_moveXAnimation.minValue +_moveXAnimation.curValue) / 500 * (parendGrid.ActualWidth - this.Width));
                    _thickness.Left = hov;

                }
                if(!_moveyAnimation.bPower)
                {
                    double vec = ((double)_showPoint.Y / 500.0 * (parendGrid.ActualHeight - this.Height));
                    _thickness.Top = vec;
                }
                else
                {

                    double vec = ((double)(_moveyAnimation.minValue + _moveyAnimation.curValue) / 500 * (parendGrid.ActualHeight - this.Height));
                    _thickness.Top = vec;
                }

                this.Margin = _thickness;

            }
            else
            {
                this.Width = (parendGrid.ActualWidth * ((_sizeAnimation.minValue+_sizeAnimation.curValue) / 1000.0));
                this.Height = (parendGrid.ActualHeight * ((_sizeAnimation.minValue+_sizeAnimation.curValue) / 1000.0));


                Size maxSize = new Size(0,0);
                double curSize = Math.Max(_sizeAnimation.begValue, _sizeAnimation.endValue);
                maxSize.Width = (parendGrid.ActualWidth * (curSize/ 1000.0));
                maxSize.Height = (parendGrid.ActualHeight * (curSize / 1000.0));


                if (!_moveXAnimation.bPower)
                {
                    double hov = ((double)_showPoint.X / 500.0 * (parendGrid.ActualWidth - maxSize.Width));
                    _thickness.Left = hov;
                }
                else
                {
                    double hov = ((double)(_moveXAnimation.minValue + _moveXAnimation.curValue) / 500 * (parendGrid.ActualWidth - maxSize.Width));
                    _thickness.Left = hov;

                }
                if (!_moveyAnimation.bPower)
                {
                    double vec = ((double)_showPoint.Y / 500.0 * (parendGrid.ActualHeight - maxSize.Height));
                    _thickness.Top = vec;
                }
                else
                {

                    double vec = ((double)(_moveyAnimation.minValue + _moveyAnimation.curValue) / 500 * (parendGrid.ActualHeight - maxSize.Height));
                    _thickness.Top = vec;
                }

                this.Margin = _thickness;

            }

        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                RefreshPostion(); 
            });
        }
    }
}
