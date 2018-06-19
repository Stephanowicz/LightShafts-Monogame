float gFactor;

texture tex;
sampler2D gTextureSampler : register(s0);


float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 UV : TEXCOORD0) : SV_TARGET0
{
    float3 c0 = tex2D(gTextureSampler, UV).rgb;
	
    return float4(c0 / gFactor, 1.0);
}

technique Technique1
{
    pass p0
    {
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}
