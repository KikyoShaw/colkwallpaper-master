# colkwallpaper

#### 介绍
动态壁纸主页：www.colkwp.com 

gitee链接：https://gitee.com/qqrock/colkwallpaper

b站视频介绍：https://space.bilibili.com/29085504?spm_id_from=333.788.0.0

youtube：https://www.youtube.com/channel/UCYreiOapB_k66p5pnJAV_UQ

youtube：https://www.youtube.com/watch?v=1I3vL-USVbM&t=17s

抖音：https://www.douyin.com/user/MS4wLjABAAAAqsU3RCbLlZUVyrMnWXr7IWv0SyPgGaelAkXaRLtc7bw

#### 更多特效获取&&问题反馈&&想加入壁纸设计者

QQ群：[982074420](http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=V18eR3alzHXcEse5SpcqsagTH0t5R72Q&authKey=p7QlzhSDxXDUTDO4AhEWlo3uyIdsuPKVMoLe6ZLcIkBeO0FjwnNsNEMcUQhx6za3&noverify=0&group_code=982074420)

#### 特效仓库

https://github.com/KikyoShaw/NewEffectPackage-dll

#### 创意工坊

https://gitee.com/qqrock/ColkDesign

#### 动画插件仓库

https://github.com/KikyoShaw/ImageAnimationTest

#### web特效
https://github.com/KikyoShaw/Cool-photo-album-html

https://github.com/KikyoShaw/html-demo

#### 第三方壁纸下载地址

https://wallhaven.cc/

#### 视频素材解析下载工具

https://github.com/KikyoShaw/videoGetWpf

#### ffmpeg工具-用于处理素材

https://github.com/KikyoShaw/FFmepgTools

#### 虚拟形象动画插件

https://github.com/stevenjoezhang/live2d-widget


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
