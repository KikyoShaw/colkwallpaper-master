/// <class>BrightExtractEffect</class>
/// <description>An effect that dims all but the brightest pixels.</description>

sampler2D input : register(S0);

/// <summary>Refraction Amount.</summary>
/// <minValue>1</minValue>
/// <maxValue>9999</maxValue>
/// <defaultValue>50</defaultValue>
float iTime : register(C0);

/// <summary>Refraction Amount.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Radio : register(C1);
//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------
float3 rgb2hsv(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float rand(float2 n) {
    return frac(sin(cos(dot(n, float2(12.9898,12.1414)))) * 83758.5453);
}

float noise(float2 n) {
    const float2 d = float2(0.0, 1.0);
    float2 b = floor(n);
    float2 f = smoothstep(float2(0.0,0), float2(1.0,1.0), frac(n));
    return lerp(lerp(rand(b), rand(b + d.yx), f.x), lerp(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(float2 n) {
    float total = 0.0, amplitude = 1.0;
    for (int i = 0; i <5; i++) {
        total += noise(n) * amplitude;
        n += n*1.5;
        amplitude *= 0.47;
    }
    return total;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    // Look up the original image color.
    float4 inputTex = tex2D(input, uv); 
    float2 fragCoord = float2(uv.x,uv.y); 
    
    const float3 c1 = float3(0.5, 0.0, 0.1);
    const float3 c2 = float3(0.9, 0.1, 0.0);
    const float3 c3 = float3(0.2, 0.1, 0.7);
    const float3 c4 = float3(1.0, 0.9, 0.1);
    const float3 c5 = float3(0.1,0,0);
    const float3 c6 = float3(0.9,0.9,0);
    
     float2 speed = float2(1.3, 0.1);
    float shift = 1.77+sin(iTime*2.0)/10.0;
    float alpha = 1.0;
	  float dist = 6.0+sin(iTime*0.4)/.6;
    float2 p = fragCoord.xy * dist /inputTex.w;
    p.x -= iTime/1.1;
    float q = fbm(p - iTime * 0.01+1.0*sin(iTime)/10.0);
    float qb = fbm(p - iTime * 0.002+0.1*cos(iTime)/5.0);
    float q2 = fbm(p - iTime * 0.44 - 5.0*cos(iTime)/7.0) - 6.0;
    float q3 = fbm(p - iTime * 0.9 - 10.0*cos(iTime)/30.0)-4.0;
    float q4 = fbm(p - iTime * 2.0 - 20.0*sin(iTime)/20.0)+2.0;
    q = (q + qb - q2 -q3  + q4)/3.8;
    float2 r = float2(fbm(p + q /2.0 + iTime * speed.x - p.x - p.y), fbm(p + q - iTime * speed.y));
    float3 c = lerp(c1, c2, fbm(p + r)) + lerp(c3, c4, r.x) - lerp(c5, c6, r.y);
    float3 color = float3(c * cos(shift *  fragCoord.y/inputTex.w/2));
    color -= .25;
    color.r *= 1.02;
    float3 hsv = rgb2hsv(color);
    hsv.y *= hsv.z  * 0.8;
    hsv.z *= hsv.y * 1.3;
    color = hsv2rgb(hsv);
    float4 fragColor = float4(color.x, color.y, color.z, alpha);
    
    
    inputTex.r = inputTex.r + fragColor.r * (Radio);
    inputTex.g = inputTex.g + fragColor.g * (Radio);
    inputTex.b = inputTex.b + fragColor.b * (Radio);
    
    //inputTex +=fragColor;
    return inputTex;
    return fragColor;
}