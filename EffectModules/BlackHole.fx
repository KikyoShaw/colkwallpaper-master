/// <class>BlackHole</class>
/// <description>An effect that makes pixels of a particular color transparent.</description>

sampler2D input0 : register(S0);

/// <summary>Time.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>1</defaultValue>
float Time : register(C0);
/// <summary>ResolutionX.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>1280</defaultValue>
float resolutionX : register(C1);
/// <summary>ResolutionY.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>1024</defaultValue>
float resolutionY : register(C2);

/// <summary>alpha.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float alpha : register(C3);

/// <summary>imgDirection.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float imgDirection : register(C4);

/// <summary>imgUpDownReverse.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float imgUpDownReverse : register(C5);

/// <summary>ImgHue.</summary>
/// <minValue>0</minValue>
/// <maxValue>360</maxValue>
/// <defaultValue>0</defaultValue>
float ImgHue : register(C6); // 0..360, default 0
/// <summary>ImgSat.</summary>
/// <minValue>0</minValue>
/// <maxValue>2</maxValue>
/// <defaultValue>1</defaultValue>
float ImgSat : register(C7); // 0..2, default 1
/// <summary>ImgLum.</summary>
/// <minValue>-1</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float ImgLum : register(C8); // -1..1, default 0

/// <summary>HoleSize.</summary>
/// <minValue>0</minValue>
/// <maxValue>1000</maxValue>
/// <defaultValue>10</defaultValue>
float HoleSize : register(C9); // -1..1, default 0

/// <summary>HoleDeep/summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>6.5</defaultValue>
float HoleDeep : register(C10); // -1..1, default 0

/// <summary>HolePostion.</summary>
/// <type>point</type>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 HolePostion : register(C11); // -0.5..1, default 0


/// <summary>Movement.</summary>
/// <type>point</type>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.1,0.1</defaultValue>
float2 Movement : register(C12); // -0.5..1, default 0


/// <summary>ChangeSize</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float ChangeSize : register(C13); // -0.5..1, default 0

float4 ColorProcessing(float4 c, float h, float s, float l)
{
    float3x3 float3rixH =
    {
        0.8164966f, 0, 0.5352037f,
	 -0.4082483f, 0.70710677f, 1.0548190f,
	 -0.4082483f, -0.70710677f, 0.1420281f
    };

    float3x3 float3rixH2 =
    {
        0.84630f, -0.37844f, -0.37844f,
	 -0.37265f, 0.33446f, -1.07975f,
	  0.57735f, 0.57735f, 0.57735f
    };


    float3x3 rotateZ =
    {
        cos(radians(h)), sin(radians(h)), 0,
		-sin(radians(h)), cos(radians(h)), 0,
		0, 0, 1
    };
    float3rixH = mul(float3rixH, rotateZ);
    float3rixH = mul(float3rixH, float3rixH2);

    float mi = 1 - s;
    float3x3 float3rixS =
    {
        mi * 0.3086f + s, mi * 0.3086f, mi * 0.3086f,
		mi * 0.6094f, mi * 0.6094f + s, mi * 0.6094f,
		mi * 0.0820f, mi * 0.0820f, mi * 0.0820f + s
    };
    float3rixH = mul(float3rixH, float3rixS);

    float3 c1 = mul(c, float3rixH);
    c1 += l;
    return float4(c1, c.a);
}

float4 main(float2 uvin : TEXCOORD) : COLOR
{

    float2 uvbg = uvin;
    if (imgUpDownReverse != 0)
        uvbg.y = 1.0 - uvbg.y;

    if (imgDirection != 0)
        uvbg.x = 1.0 - uvbg.x;

    
    float2 radio = float2(1000, 1000);
    float2 fc = float2(radio.x * uvbg.x, radio.y * (uvbg.y));

    float chs =  ChangeSize*sin( Time );
    float2 uv = fc.xy / radio.xy;
    float2 pos =  float2(radio.x*HolePostion.x, radio.y*HolePostion.y);
    float2 move = float2(0,0);
    move.x = (Movement.x * radio.x * 0.5);
    move.y = (Movement.y * radio.y * 0.5);
    float2 lpos = float2(pos.x+ sin(Time) * move.x, pos.y + cos(Time) * move.y) ;
    float2 warp = normalize(lpos.xy - fc.xy) * pow(distance(lpos.xy, fc.xy), -2.0) * ((HoleSize*100) + chs*100);
    uv = uv + warp;
    float light = clamp(0.1*distance(lpos.xy, fc.xy) - (HoleDeep+chs*0.1), 0.0, 1.0);
    
    float4 color = tex2D(input0, uv); 
    color = ColorProcessing(color, ImgHue, ImgSat, ImgLum) * light;
    color.a = 1.0;
    return color;
}
