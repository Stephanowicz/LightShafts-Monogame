#include "SpriteBatchVertexShader.fx"

sampler2D gWorldCoordinates	: register( s0 );
sampler2D gSource			: register( s1 );
sampler gBlurLevel0			: register( s2 );

float gAperture;// = 1;
float gZNear;// = 1;
float gZFar;// = 500;
float gPlaneInFocus;// = 0.5;
float gFocalLength;// = 0.1;

struct VS_IN
{
    float4 Position : POSITION;
    float2 UV		: TEXCOORD0;
};

struct VS_OUT
{
    float4 Position : POSITION0;
	float2 UV		: TEXCOORD0;
};
// ---------------------------------------------------------
float4 PixelShaderFunction( VS_OUT IN ) : COLOR0
{
	float4 WorldPos = tex2D( gWorldCoordinates, IN.UV );
	float4 Color = tex2D( gSource, IN.UV );
	float d = WorldPos.z;
	
	float CoCScale = ( gAperture * gFocalLength * gPlaneInFocus 
		* (gZFar - gZNear ) )
		/ ( ( gPlaneInFocus - gFocalLength ) * gZNear * gZFar );
	float CoCBias = ( gAperture * gFocalLength 
		* ( gZNear - gPlaneInFocus ) ) 
		/ ( ( gPlaneInFocus * gFocalLength ) * gZNear );
		
	float CoC = abs( d * CoCScale + CoCBias );
	
	float4 tex0 = tex2D( gSource, IN.UV );	
	float4 tex1 = tex2D( gBlurLevel0, IN.UV );
		
	//float focalValue = smoothstep(0.0f, gFocalLength, CoC);
	float4 returnColor = lerp(tex0, tex1, CoC);
	//return float4( focalValue.xxx,1);
	//return float4( CoC.xxx, 1 );
	return float4( tex1.rgb, 1 );
}
// ---------------------------------------------------------
technique Technique1
{
    pass p0
    {
		VertexShader = compile vs_4_0 SpriteVertexShader( );
        PixelShader = compile ps_4_0 PixelShaderFunction( );
    }
}
