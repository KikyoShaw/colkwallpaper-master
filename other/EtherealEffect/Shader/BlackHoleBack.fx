/// <class>BlackHoleBack</class>
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
/// <maxValue>20</maxValue>
/// <defaultValue>15</defaultValue>
float HoleSize : register(C9); // -1..1, default 0

/// <summary>HolePostion.</summary>
/// <type>point</type>
/// <minValue>0,0</minValue>
/// <maxValue>1,1</maxValue>
/// <defaultValue>0.5,0.5</defaultValue>
float2 HolePostion : register(C10); // -0.5..1, default 0

/// <summary>HoleDeep/summary>
/// <minValue>0</minValue>
/// <maxValue>20</maxValue>
/// <defaultValue>8.8</defaultValue>
float HoleDeep : register(C11); // -1..1, default 0

/// <summary>Movement.</summary>
/// <type>point</type>
/// <minValue>0,0</minValue>
/// <maxValue>1000,1000</maxValue>
/// <defaultValue>100,100</defaultValue>
float2 Movement : register(C12); // -0.5..1, default 0


/// <summary>ChangeSize</summary>
/// <minValue>0</minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>3</defaultValue>
float ChangeSize : register(C13); // -0.5..1, default 0
#define mat float3
#define vec2 float2
#define vec3 float3
#define vec4 float4
#define fract frac
#define mix lerp
#define mat3 float3x3
#define mat2 float2x2
#define mod fmod
#define texture tex2D

float4 ColorProcessing(float4 c, float h, float s, float l)
{
    float3x3 matrixH =
    {
        0.8164966f, 0, 0.5352037f,
	 -0.4082483f, 0.70710677f, 1.0548190f,
	 -0.4082483f, -0.70710677f, 0.1420281f
    };

    float3x3 matrixH2 =
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
    matrixH = mul(matrixH, rotateZ);
    matrixH = mul(matrixH, matrixH2);

    float mi = 1 - s;
    float3x3 matrixS =
    {
        mi * 0.3086f + s, mi * 0.3086f, mi * 0.3086f,
		mi * 0.6094f, mi * 0.6094f + s, mi * 0.6094f,
		mi * 0.0820f, mi * 0.0820f, mi * 0.0820f + s
    };
    matrixH = mul(matrixH, matrixS);

    float3 c1 = mul(c, matrixH);
    c1 += l;
    return float4(c1, c.a);
}
float rayStrength(float2 raySource, float2 rayRefDirection, float2 coord, float seedA, float seedB, float speed)
{
    float2 sourceToCoord = coord - raySource;
    float cosAngle = dot(normalize(sourceToCoord), rayRefDirection);
	
    return clamp(
		(0.45 + 0.15 * sin(cosAngle * seedA + Time * speed)) +
		(0.3 + 0.2 * cos(-cosAngle * seedB + Time * speed)),
		0.0, 1.0) *
		clamp((resolutionX - length(sourceToCoord)) / resolutionX, 0.5, 1.0);
}

vec2 ws(vec2 p, float s, float r, float w)
{
	float d = Time * s, x = w * (1.0 + r) * (p.x + d), y = w * (1.0 + r) * (p.y + d);
	return vec2(cos(x - y) * cos(y), sin(x + y) * sin(y));
}

vec2 waterProcessing2(vec2 uvin, float s, float rand,  float width)
{
	float2 uvout = uvin;
	uvout.y = 1.0 - uvout.y;

	float2 iResolution = float2(resolutionX, resolutionY);
	float2 fragCoord = float2(iResolution.x * uvout.x, iResolution.y * uvout.y);

	vec2 r = fragCoord / iResolution, q = r + 2. / iResolution.x * (ws(r, s, rand, width) - ws(r + iResolution, s, rand,width));

	q.y = 1. - q.y;
	q.y = max(0, q.y);
	q.x = max(0, q.x);
	q.y = min(1, q.y);
	q.x = min(1, q.x);
	return q;
}

vec4 getTexure(vec2 coord){
	return texture(input0,coord.xy);    
}

float4 main(float2 uvin : TEXCOORD) : COLOR
{

    float2 uvbg = uvin;
    if (imgUpDownReverse != 0)
        uvbg.y = 1.0 - uvbg.y;

    if (imgDirection != 0)
        uvbg.x = 1.0 - uvbg.x;

    
    float2 iResolution = float2(resolutionX, resolutionY);
    float2 fragCoord = float2(iResolution.x * uvbg.x, iResolution.y * (uvbg.y));

    float chs = ChangeSize*sin(Time);
    vec2 uv = fragCoord.xy / iResolution.xy;
    
    vec2 texC = uv;
    vec2 pos =  vec2(iResolution.x*HolePostion.x, iResolution.y*HolePostion.y);
    vec2 lpos = vec2(pos.x+ sin(Time) * Movement.x, pos.y + cos(Time)*Movement.y) / iResolution.x;
    vec2 texC2 = fragCoord.xy / iResolution.x;
    
    texC = mix(texC2, (texC * 2.0 - lpos * 1.0) * 0.0 * 0.5 + lpos, (1.0 / (distance((texC2 * 2.0 - lpos * 2.0) * (33.0-HoleSize + chs ) * 0.5 + lpos, lpos) - 1.0))); //Black hole shader
    
    vec3 getColor = getTexure(texC);
    
    getColor *= clamp(pow(distance(texC2, vec2(lpos)), HoleDeep + chs/10) * 300000000.0,0.0,1.0);
    
    vec4 color = vec4(getColor,1.0);
    color = ColorProcessing(color, ImgHue, ImgSat, ImgLum);
    return color;
}
