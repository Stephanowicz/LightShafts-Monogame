#define GAMMA 1.0 / 2.2

float4x4 gWorldXf;
float4x4 gViewXf;
float4x4 gProjectionXf;
float4x4 gWorldITXf;
float4x4 gViewIXf;
float gExposure;

/*
samplerCUBE gSamplerSpecular	: register( s0 );
samplerCUBE gSamplerDiffuse		: register( s1 );



samplerCUBE gSampler = sampler_state
{
	texture = < gTexture >;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap; 
	AddressW = Wrap;
};
*/

struct a2v
{
    float4 Position : POSITION0;
    float3 Normal	: NORMAL0;
};

struct v2f
{
    float4 Position : POSITION0;
	float3 Normal	: TEXCOORD0;
	float3 View		: TEXCOORD1;
	float3 WorldPos	: TEXCOORD2;
};

v2f VertexShaderFunction(a2v IN)
{
    v2f OUT;

    float4 worldPosition = mul( IN.Position, gWorldXf );
    float4 viewPosition = mul( worldPosition, gViewXf );
    OUT.Position = mul( viewPosition, gProjectionXf );
	OUT.Normal = mul( IN.Normal, gWorldITXf ).xyz;
	OUT.View = gViewIXf[ 3 ].xyz - worldPosition.xyz;
	OUT.WorldPos = worldPosition;

    return OUT;
}

float4 PixelShaderFunction(v2f IN) : COLOR0
{
	float3 N = normalize( IN.Normal );
	float3 V = normalize( IN.View );
	float3 R = reflect( -V, N );
	float3 L = normalize( float3( 0, 0, 180 ) - IN.WorldPos ); //?
	float Diff = max( 0.0,  dot( L, N ) );
    float4 Diffuse = saturate(dot(N.xyz, R.xyz));
    float4 Specular = 0.2 * max(pow(saturate(dot(R, V)), 5), 0) * length(Diffuse);
	
	Specular = pow( Specular, GAMMA );
	Diffuse = pow( Diffuse, GAMMA );
	
	float3 FinalColor = (Diffuse.rgb * 1.0
		+ Specular.rgb *0.6) * Diff;
	
	return float4( FinalColor * gExposure, 1 );
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}
