float4x4 gWorldXf;
float4x4 gViewXf;
float4x4 gProjectionXf;
float4x4 gWorldITXf;
float4x4 gViewIXf;
float gExposure;

struct VS_IN
{
    float4 Position : POSITION;
};

struct VS_OUT
{
    float4 Position : POSITION0;
	float4 WorldPos	: TEXCOORD0;
};

// ---------------------------------------------------------
VS_OUT VertexShaderFunction(VS_IN IN)
{
    VS_OUT OUT = ( VS_OUT ) 0;

    float4 worldPosition = mul( IN.Position, gWorldXf );
    float4 viewPosition = mul( worldPosition, gViewXf );
    OUT.Position = mul( viewPosition, gProjectionXf );
    OUT.WorldPos = OUT.Position;

    return OUT;
}
// ---------------------------------------------------------
float4 PixelShaderFunction( VS_OUT IN ) : COLOR0
{
	//return IN.WorldPos / IN.WorldPos.w;
	IN.WorldPos.xyz /= IN.WorldPos.w;
	float Depth = IN.WorldPos.z;
	return float4( Depth.xxx, 1 );
	//return float4( IN.WorldPos.xyz / IN.WorldPos.w, 1 );
}
// ---------------------------------------------------------
technique Technique1
{
    pass p0
    {
        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction( );
    }
}
