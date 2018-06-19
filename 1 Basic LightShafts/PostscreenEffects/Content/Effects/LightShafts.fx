#define NUM_SAMPLES 100

struct PS_IN
{
    float4 position : SV_POSITION;
    float4 color : COLOR0;
    float2 texCoord : TEXCOORD0;
};


sampler TextureSampler : register(s0);

float2		gScreenLightPos;
float		gDensity;
float		gDecay;
float		gWeight;
float		gExposure;


// ---------------------------------------------------------
float4 PixelShaderFunction(PS_IN input) : SV_TARGET0
{
    float2 TexCoord = input.texCoord;
    float2 DeltaTexCoord = (TexCoord.xy - gScreenLightPos.xy);
    float Len = length(DeltaTexCoord);
    DeltaTexCoord *= 1.0 / NUM_SAMPLES * gDensity;
    float4 Color = tex2D(TextureSampler, TexCoord);
    float IlluminationDecay = 1.0;
    for (int i = 0; i < NUM_SAMPLES; ++i)
    {
        TexCoord -= DeltaTexCoord;
        float4 Sample = tex2D(TextureSampler, TexCoord);
        Sample *= IlluminationDecay * gWeight;
        Color += Sample;
        IlluminationDecay *= gDecay;
    }
    return float4(Color.xyz * gExposure, 1.0);
}
// ---------------------------------------------------------
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
    }
}
