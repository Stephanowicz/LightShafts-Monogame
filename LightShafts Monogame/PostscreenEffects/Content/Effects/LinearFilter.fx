sampler TextureSampler : register(s0);
float2 gTextureSize;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 UV : TEXCOORD0) : SV_TARGET0
{
    float OffsetX = 1.0 / gTextureSize.x;
    float OffsetY = 1.0 / gTextureSize.y;
    float2 Coords;
    float4 Color = tex2D(TextureSampler, UV);

    Coords.x = UV.x + OffsetX;
    Coords.y = UV.y;
    Color += tex2D(TextureSampler, Coords);
    Coords.x = UV.x - OffsetX;
    Coords.y = UV.y;
    Color += tex2D(TextureSampler, Coords);
    Coords.x = UV.x;
    Coords.y = UV.y + OffsetY;
    Color += tex2D(TextureSampler, Coords);
    Coords.x = UV.x;
    Coords.y = UV.y - OffsetY;
    Color += tex2D(TextureSampler, Coords);
	
    Color *= 0.2;
	
    return float4(Color.rgb, 1.0);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}
