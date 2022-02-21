# colkwallpaper

#### 介绍
动态壁纸 www.colkwp.com 

gitee链接：https://gitee.com/qqrock/colkwallpaper

下载链接：https://colkwp.coding.net/s/e3cf1a2f-4e01-4076-9a7e-17f1467590a7

b站教学：https://www.bilibili.com/video/BV1q44y1H7nh?spm_id_from=333.999.0.0

CSDN：https://blog.csdn.net/qq_36651243/article/details/123015982?spm=1001.2014.3001.5502

#### 软件架构
软件架构说明
 使用 wpf .net core 3.1 

#### 开发教程

1.先运行bin\cfg.bat

2.调试dll 设置调试属性

运行可执行文件的路径:
$(SolutionDir)bin\Debug\netcoreapp3.1\ColkEffectClient.exe

默认运行加载 
特效:
EffectModules\bin\netcoreapp3.1\EffectModule.dll
插件:
PluginModules\bin\netcoreapp3.1\DefaultPlugin.dll


工程需要设置生成后事件
特效:
copy  "$(TargetPath)"  "$(OutDir)EffectModule.dll"
插件
copy  "$(TargetPath)"  "$(OutDir)DefaultPlugin.dll"


#### 开发完成打包说明
1.特效放入%appdata%\Effect\{effName}
需要3个文件: json文件 缩略图 特效.dll
json文件格式:
例子:
{
    "sIdentifyKey":"BlackHoleEffect_v1.0",
    "sName":"BlackHoleEffect",
    "sDesc":"黑洞特效",
    "sThumbPicName":"Thumb.jpg",
    "iOverheadLevel":0,
    "sModelName":"BlackHoleEffect.dll"
}

sIdentifyKey 特效key 全局唯一
sName 特效名字
sDesc 特效描述
sThumbPicName 缩略图文件名
iOverheadLevel 耗能等级0~2
sModelName  特效dll文件

2.插件放入%appdata%\Plugin\{PluginName}
需要3个文件
json文件 缩略图 插件.dll

{
    "sIdentifyKey":"CircleVisualizerPlugin_v1.0",
    "sName":"CircleVisualizerPlugin",
    "sDesc":"频谱(圆)插件",
    "sThumbPicName":"Thumb.jpg",
    "iOverheadLevel":1,
    "sModelName":"CircleVisualizerPlugin.dll"
}

sIdentifyKey 插件key 全局唯一
sName 插件名字
sDesc 插件描述
sThumbPicName 插件缩略图文件名
iOverheadLevel [没用]
sModelName  插件dll文件

#### 参与贡献



#### 特技

可以使用酷壁加载你自己开发的特效与插件
