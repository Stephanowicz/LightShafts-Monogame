#define SHADOW_BIAS 0.00001
#define NUM_SAMPLES 128
#define PI 3.14159265358979323846

#include "SpriteBatchVertexShader.fx"

// *********************************************************
// Required Render Targets
// 1. World Coordinates
// 2. Shadow Map
// 3. Surface Color
// *********************************************************

texture2D gTextureWorld;
texture2D gTextureShadow;
texture2D gTextureColor;

sampler2D gSamplerWorldMap = sampler_state
{
	Texture = <gTextureWorld>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

sampler2D gSamplerShadowMap = sampler_state
{
	Texture = <gTextureShadow>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

sampler2D gSamplerColorMap = sampler_state
{
	Texture = <gTextureColor>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

// *********************************************************
// Effect Parameters
// *********************************************************

float4x4	gLightViewProjectionXf;
float3		gCameraPosition;
float3		gLightPosition;
float		gDensity = 1.0;			// probability of collision
float		gAlbedo = 1.0;			// probability of scattering
float		gLightIntensity = 1.0;

// *********************************************************
// Shader Code
// *********************************************************

struct VS_OUT
{
    float4 Position : POSITION0;
	float2 TexCoord	: TEXCOORD0;
};

// ---------------------------------------------------------
float ComputeScatteringTerm( 
	float3 Point, 
	float3 Direction )
{
	float DistanceSqr = pow( length( gLightPosition - Point ), 2 );
	float Result = 0;
	Result = gDensity * gAlbedo 
		* ( gLightIntensity / 4 * PI * DistanceSqr );
	return Result;
}
// ---------------------------------------------------------
float ShadowMC( sampler2D ShadowMap, float3 Point )
{
	float2 ProjTexCoords;
	ProjTexCoords.x = Point.x / 2.0 + 0.5;
	ProjTexCoords.y = -Point.y / 2.0 + 0.5;
	
	float Result = 1;
	if( ProjTexCoords.x >= 0 && ProjTexCoords.x < 1 &&
		ProjTexCoords.y >= 0 && ProjTexCoords.y < 1 )
	{
		float4 PosSM = tex2D( ShadowMap, ProjTexCoords );							
		if( PosSM.z < Point.z - SHADOW_BIAS )
		{
			Result = 0;
		}
	}
	return Result;
}
// ---------------------------------------------------------
float4 PixelShaderFunction( VS_OUT IN) : COLOR0
{	
	float4 CurPos = mul(
		tex2D(gSamplerWorldMap, IN.TexCoord),
		gLightViewProjectionXf);
	CurPos /= CurPos.w;

	float4 EndPos = mul(
		float4( 0, 0, 30, 1 ),
		gLightViewProjectionXf);
	EndPos /= EndPos.w;
		
	float4 Direction = ( EndPos - CurPos ) / NUM_SAMPLES;
	float Accu = 0;
	float L0 = 1;
	float s = CurPos.z;
	float L = L0 * exp( -s * gDensity );
	
	for( int i = 0; i < NUM_SAMPLES; ++i )
	{
		//float v = ShadowMC( ShadowMap, CurPos );
		//float d = length( CurPos );
		//float Lin = exp( -d * gDensity ) * v * gLightIntensity / 4 / PI / d / d;
		//float Li = Lin * gDensity * gAlbedo * P(x, Direction );
        Accu += ShadowMC(gSamplerShadowMap, CurPos);
		
		CurPos += Direction;
	}
	Accu /= NUM_SAMPLES;
	return float4( Accu.xxx, 1 );
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