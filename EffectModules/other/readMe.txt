/// <summary>Time.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>1</defaultValue>
float Time : register(C0);

/// <summary>resolutionX.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>1280</defaultValue>
float resolutionX : register(C1);

/// <summary>resolutionY.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>1024</defaultValue>
float resolutionY : register(C2);

/// <summary>direction.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float direction : register(C3);

/// <summary>UpDownReverse.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float UpDownReverse : register(C4);

/// <summary>ImgHue.</summary>
/// <minValue>0</minValue>
/// <maxValue>360</maxValue>
/// <defaultValue>0</defaultValue>
float ImgHue : register(C5); // 0..360, default 0
/// <summary>ImgSat.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float ImgSat : register(C6); // 0..2, default 1
/// <summary>ImgLum.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float ImgLum : register(C7); // -1..1, default 0

/// <summary>EffHue.</summary>
/// <minValue>0</minValue>
/// <maxValue>360</maxValue>
/// <defaultValue>0</defaultValue>
float EffHue : register(C8); // 0..360, default 0
/// <summary>EffSat.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float EffSat : register(C9); // 0..2, default 1
/// <summary>EffLum.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float EffLum : register(C10); // -1..1, default 0

/// <summary>The radius of the Poisson disk (in pixels).</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float alpha : register(C11);
/// <summary>alphaMode.</summary>
/// <minValue>-2</minValue>
/// <maxValue>5</maxValue>
/// <defaultValue>0</defaultValue>
float alphaMode : register(C12); // -1..1, default 0
/// <summary>ImgGrayFilter.</summary>
/// <minValue>-2</minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>0</defaultValue>
float ImgGrayFilter : register(C13); // -1..1, default 0

/// <defaultValue>#000000</defaultValue>
float4 ImgFilterColor : register(C14); // -1..1, default 0

/// <summary>FiflterHeight.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float FiflterHeight : register(C15); // -1..1, default 0


/// <summary>imgDirection.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float imgDirection : register(C16);

/// <summary>imgUpDownReverse.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float imgUpDownReverse : register(C17);