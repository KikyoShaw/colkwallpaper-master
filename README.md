# colkwallpaper

#### 介绍
动态壁纸主页：www.colkwp.com 

gitee链接：https://gitee.com/qqrock/colkwallpaper

下载链接：https://colkwp.coding.net/s/e3cf1a2f-4e01-4076-9a7e-17f1467590a7

b站教学：https://www.bilibili.com/video/BV1q44y1H7nh?spm_id_from=333.999.0.0

youtube：https://www.youtube.com/watch?v=g8PBgHHiCl8
youtube：https://www.youtube.com/watch?v=1I3vL-USVbM&t=17s

## 效果图
#### 原生特效
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/1.png)

#### 原生特效设置
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/2.png)

#### 视频特效
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/3.png)

#### 网页特效
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/4.png)

#### 壁纸下载
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/5.png)

#### 壁纸设计
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/6.png)

#### 壁纸插件
![image](https://github.com/KikyoShaw/colkwallpaper-master/blob/master/Image/7.png)

## 软件架构
软件架构说明
 使用 wpf .net core 3.1 

## 开发教程
  1. 先运行bin\cfg.bat

  2. 调试dll,需设置调试属性
    - 运行可执行文件的路径: $(SolutionDir)bin\Debug\netcoreapp3.1\ColkEffectClient.exe

  3. 程序默认运行加载  
     1. 特效: EffectModules\bin\netcoreapp3.1\EffectModule.dll
     2. 插件: PluginModules\bin\netcoreapp3.1\DefaultPlugin.dll

  4. 特效/插件工程需要设置生成后事件
     1. 特效: copy  "$(TargetPath)"  "$(OutDir)EffectModule.dll"
	 2. 插件: copy  "$(TargetPath)"  "$(OutDir)DefaultPlugin.dll"


## 特效开发完成打包说明
 1. 特效放入%appdata%\ColkWallpaper\Effect\{你的特效名}
 2. 需要3个文件: 
    1. json文件 
    2. 缩略图
    3. 特效.dll
   
 
  ```
   json文件格式:
   {
     "sIdentifyKey":"BlackHoleEffect_v1.0",
 	 "sName":"BlackHoleEffect",
	 "sDesc":"黑洞特效",
	 "sThumbPicName":"Thumb.jpg",
	 "iOverheadLevel":0,
	 "sModelName":"BlackHoleEffect.dll"
	}	
    注释:
	sIdentifyKey 特效key 全局唯一
	sName 特效名字
	sDesc 特效描述
	sThumbPicName 缩略图文件名
	iOverheadLevel 耗能等级0~2
	sModelName  特效dll文件
  ```
	
## 插件开发完成打包说明

 1. 插件放入%appdata%\ColkWallpaper\Plugin\{你的特效名}
 2. 需要3个文件: 
    1. json文件 
    2. 缩略图
    3. 插件.dll
  
  ```
  json例子:
  {
      "sIdentifyKey":"CircleVisualizerPlugin_v1.0",
      "sName":"CircleVisualizerPlugin",
      "sDesc":"频谱(圆)插件",
      "sThumbPicName":"Thumb.jpg",
      "iOverheadLevel":1,
      "sModelName":"CircleVisualizerPlugin.dll"
  }
  sIdentifyKey: 插件key 全局唯一
  sName: 插件名字
  sDesc: 插件描述
  sThumbPicName: 插件缩略图文件名
  iOverheadLevel: [没用]
  sModelName:  插件dll文件
  ```

#### 参与贡献



#### 特技

可以使用酷壁加载你自己开发的特效与插件
