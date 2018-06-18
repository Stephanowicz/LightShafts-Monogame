sampler gInputSampler : register( s0 );

float gThreshold;
float gScaleFactor;

float Luminance( float3 Color )
{
	return 
		0.3 * Color.r +
		0.6 * Color.g +
		0.1 * Color.b;
}

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 UV : TEXCOORD0) : SV_TARGET0
{	
    float4 Color = tex2D(gInputSampler, UV);
    float luminance = Luminance(Color.rgb);
	if( luminance >= gThreshold )
	{
		return float4( Color.rgb * gScaleFactor, 1.0 );
	}
	else
	{
		return float4( 0.0, 0.0, 0.0, 1.0 );
	}
} 

technique Technique0
{
    pass p0
	{		
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}