#pragma pack_matrix(row_major )

cbuffer cbPerObject : register( b0 )
{
    float4x4 matrMVP;
    float opacity;
    float blur_size;
    float2 _reserverd;
    float2 background_image_size;
    float2 background_size;
    float4 background_grid;
}


Texture2D g_texture0 : register(t0);
Texture2D g_texture1 : register(t1);
Texture2D g_texture2 : register(t2);
Texture2D g_texture3 : register(t3);

Texture2D g_backgrounds : register(t10);

Buffer<uint> barrages_Indices : register(t11);
Buffer<uint> barrages_Colors : register(t12);
Buffer<float4> barrages_Trans : register(t13);
Buffer<float4> barrages_Blocks : register(t14);
SamplerState g_samLinear : register( s0 );

struct VS_OUTPUT
{
    float4 pos : SV_POSITION;
    float2 texcoord : TEXCOORD0;

    uint primitiveIndex : SV_PrimitiveID;
};

float4 color_uint_to_float4(uint color)
{
    if (color == 0xffffffff)
        return float4(1, 1, 1, 1);

    float a = ((color & 0xff000000) >> 24);
    float r = ((color & 0xff0000) >> 16);
    float g = ((color & 0xff00) >> 8);
    float b = ((color & 0xff));
    return float4(r, g, b, a) / 255.0;
}

float blur_sample(const Texture2D tex, float2 texcoord, float blur_size)
{
    blur_size = blur_size / 9.0f;
    float alpha = 0;
    //float factors[3][3] = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
    float factors[3][3] = { 0.0947416f, 0.118618f, 0.0947416f, 0.118318f, 0.147761f, 0.118318f, 0.0947416f, 0.118618f, 0.0947416f };
    //float factors[3][3] = { 0.1111111f, 0.1111111f, 0.1111111f, 0.1111111f, 0.1111111f, 0.1111111f, 0.1111111f, 0.1111111f, 0.1111111f };
    
    for (int row = 0; row < 3; ++row)
    {
        for (int col = 0; col < 3; ++col)
            alpha += factors[row][col] * tex.Sample(g_samLinear,
            float2(texcoord.x + float(row - 1) * (blur_size / background_image_size.x),
            texcoord.y + float(col - 1) * (blur_size / background_image_size.y))).a;
    }
    return alpha;
}

// 内部缩放
float2 background_texcoord_inner(float2 texcoord, float2 background_size_src, float2 background_size_dst)
{
    float scale_y = background_size_dst.y / background_size_src.y;
    float2 ret = texcoord;
    float2 pos = texcoord * background_size_dst;

    float grid_right = background_size_src.x - background_grid.x - background_grid.z;
    float grid_bottom = background_size_src.y - background_grid.y - background_grid.w;

    float rate_x = background_grid.z / background_size_src.x;
    float rate_y = background_grid.w / background_size_src.y;

    if (pos.x < background_grid.x * scale_y)
        ret.x = pos.x / (background_size_src.x * scale_y);
    else if (pos.x > background_size_dst.x - grid_right * scale_y)
        ret.x = 1.0f - (background_size_dst.x - pos.x) / (background_size_src.x * scale_y);
    else
        ret.x = background_grid.x / background_size_src.x + rate_x * (pos.x - background_grid.x * scale_y) / (background_size_dst.x - background_grid.x * scale_y - grid_right * scale_y);
    
    if (pos.y < background_grid.y * scale_y)
        ret.y = pos.y / (background_size_src.y * scale_y);
    else if (pos.y > background_size_dst.y - grid_bottom * scale_y)
        ret.y = 1.0f - (background_size_dst.y - pos.y) / (background_size_src.y * scale_y);
    else
        ret.y = background_grid.y / background_size_src.y + rate_y * (pos.y - background_grid.y * scale_y) / (background_size_dst.y - background_grid.y * scale_y - grid_bottom * scale_y);

    return ret;
}

float4 ps_main(VS_OUTPUT input) : SV_TARGET
{
    //if (input.primitiveIndex % 2)
    //    return float4(1, 0, 0, 1);
    //else
    //    return float4(1, 0, 1, 1);

    uint barrage_index = input.primitiveIndex / 2;
    uint indices = barrages_Indices[barrage_index];
    float4 srcColor = color_uint_to_float4(barrages_Colors[barrage_index]);
    uint texture_index = indices & 0xf;
    uint background_index = (indices >> 8) & 0xf;
    float4 block = barrages_Blocks[barrage_index];

    float4 backcolor = float4(0, 0, 0, 0);
    if (background_index > 0)
    {
        float4 background_rect = float4(0, background_size.y * (background_index - 1) / background_image_size.y, background_size.x / 512.0f, background_size.y / background_image_size.y);
        float4 trans = barrages_Trans[barrage_index];

        float2 texcoord_bg = background_rect.xy + background_rect.zw * background_texcoord_inner(input.texcoord, background_size, trans.zw);
        backcolor = g_backgrounds.Sample(g_samLinear, texcoord_bg);
    }

    float4 textcolor = float4(0, 0, 0, 0);
    float2 texcoord = block.xy + block.zw * input.texcoord;
    float alpha = 0;
    if (texture_index == 0)
        textcolor = g_texture0.Sample(g_samLinear, texcoord);
    else if (texture_index == 1)
        textcolor = g_texture1.Sample(g_samLinear, texcoord);
    else if (texture_index == 2)
        textcolor = g_texture2.Sample(g_samLinear, texcoord);
    else if (texture_index == 3)
        textcolor = g_texture3.Sample(g_samLinear, texcoord);
    else
    {
    }

    float alpha2 = 0;
    if (texture_index == 0)
        alpha2 = blur_sample(g_texture0, texcoord, blur_size + 0.2);
    else if (texture_index == 1)
        alpha2 = blur_sample(g_texture1, texcoord, blur_size + 0.2);
    else if (texture_index == 2)
        alpha2 = blur_sample(g_texture2, texcoord, blur_size + 0.2);
    else if (texture_index == 3)
        alpha2 = blur_sample(g_texture3, texcoord, blur_size + 0.2);
    else
    {
    }

    if (alpha2 < 0.1)
        alpha2 = alpha2 * 0.5f;
    alpha2 = min(alpha2 * 4, 1);
    if (alpha2 < 0.001f)
        return backcolor * opacity * (barrage_index > 0 ? 1.3 : 1);

    float4 sourceColor = textcolor;
    sourceColor.a = 1.0;
    float4 color = lerp(float4(0.35f, 0.35f, 0.35f, alpha2*0.80f), sourceColor * 1.05f, textcolor.a);
    float4 color2 = lerp(float4(0.1f, 0.1f, 0.1f, alpha2*0.70f), sourceColor* 1.1f, textcolor.a);
    color = lerp(color2, color, textcolor.a);

    color = lerp(backcolor, color, color.a); 
    color2 = lerp(backcolor, color2, color2.a);
    color = lerp(color2, color, color2.a);
    return float4(color.rgb, color.a * opacity );

    //float2 texcoord = block.xy + block.zw * input.texcoord;
    //float alpha = 0;
    //if (texture_index == 0)
    //    alpha = g_texture0.Sample(g_samLinear, texcoord).a;
    //else if (texture_index == 1)
    //    alpha = g_texture1.Sample(g_samLinear, texcoord).a;
    //else if (texture_index == 2)
    //    alpha = g_texture2.Sample(g_samLinear, texcoord).a;
    //else if (texture_index == 3)
    //    alpha = g_texture3.Sample(g_samLinear, texcoord).a;
    //else
    //{
    //}
    //float alpha2 = 0;
    //if (texture_index == 0)
    //    alpha2 = blur_sample(g_texture0, texcoord, blur_size);
    //else if (texture_index == 1)
    //    alpha2 = blur_sample(g_texture1, texcoord, blur_size);
    //else if (texture_index == 2)
    //    alpha2 = blur_sample(g_texture2, texcoord, blur_size);
    //else if (texture_index == 3)
    //    alpha2 = blur_sample(g_texture3, texcoord, blur_size);
    //else
    //{
    //}
    //
    //if(alpha2 < 0.1)
    //    alpha2 = alpha2 * 0.5f;
    //alpha2 = min(alpha2 * 4, 1);
    //if(alpha2 < 0.001f)
    //    return backcolor * opacity * (barrage_index > 0 ? 1.3 : 1); //TV上电视边框透明度调整
    //
    //float4 color = lerp(float4(0.3f, 0.3f, 0.3f, alpha2*0.9f), srcColor * 1.0f, alpha);
    //color = lerp(backcolor, color, color.a);
    //return float4(color.rgb, color.a * opacity);

}
