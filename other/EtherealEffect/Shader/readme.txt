:: use /Gec option for compatibility mode (required by some .fx files)
echo ******* Compiling Pixel Shaders *******
"%DXSDK_DIR%\Utilities\bin\x86\fxc" /T ps_3_0 /E main /Fo "%1\MyEffect1\MyEffect1.ps" "%1\MyEffect1\MyEffect1.fx"
echo ******* Compiling Pixel Shaders Completed *******

buil ps

.fx 是DX9

.hlsl是dx11

& << >> 操作符只有dx11才支持

https://wenku.baidu.com/view/10245c44b307e87101f69602.html