float4x4 gWorldXf;
float4x4 gViewXf;
float4x4 gProjectionXf;
float4x4 gWorldITXf;

struct VS_IN
{
    float4 Position : POSITION;
    //float4 TexCoords: TEXCOORD0;
    float3 Normal	: NORMAL;
};

struct VS_OUT
{
    float4 Position : POSITION0;
	float4 Normal	: TEXCOORD0;
	//float4 View		: TEXCOORD1;
	//float2 TexCoords: TEXCOORD2;
};

// ---------------------------------------------------------
VS_OUT VertexShaderFunction(VS_IN IN)
{
    VS_OUT OUT = ( VS_OUT ) 0;

    float4 worldPosition = mul( IN.Position, gWorldXf );
    float4 viewPosition = mul( worldPosition, gViewXf );
    OUT.Position = mul( viewPosition, gProjectionXf );
	OUT.Normal = mul( IN.Normal, gWorldITXf );
	//OUT.View = ViewXf[3] - worldPosition;
	//OUT.TexCoords = IN.TexCoords.xy;

    return OUT;
}
// ---------------------------------------------------------
float4 PixelShaderFunction( VS_OUT IN ) : COLOR0
{
	float4 N = normalize( IN.Normal );
	float4 ToLight = normalize( float4( 2, 1, -4, 0 ) );
	float lamb = dot( N, ToLight );
	return float4( lamb.xxx*0.5, 1.0 );
	//return float4( 1, 1, 1, 1.0 );
	//return float4( 0, 0, 0, 1.0 );
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
