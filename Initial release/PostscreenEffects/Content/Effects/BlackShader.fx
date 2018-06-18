float4x4 gWorldXf;
float4x4 gViewXf;
float4x4 gProjectionXf;

struct VS_IN
{
    float4 Position : POSITION;
};

struct VS_OUT
{
    float4 Position : POSITION0;
};

// ---------------------------------------------------------
VS_OUT VertexShaderFunction( 
	VS_IN IN)
{
    VS_OUT OUT = ( VS_OUT ) 0;

    float4 worldPosition = mul( IN.Position, gWorldXf );
    float4 viewPosition = mul( worldPosition, gViewXf );
    OUT.Position = mul( viewPosition, gProjectionXf );

    return OUT;
}
// ---------------------------------------------------------
float4 PixelShaderFunction( VS_OUT IN ) : COLOR0
{
	return float4( 0, 0, 0, 1 );
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
