struct VertexShaderOutput
{
    float4 position : SV_POSITION;
    float4 color : COLOR0;
    float2 texCoord : TEXCOORD0;
};
struct VertexShaderInput
{
    float4 position : SV_POSITION;
    float4 color : COLOR0;
    float2 texCoord : TEXCOORD0;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    
    output.position = float4(input.position.xyz, 1);
    output.texCoord = input.texCoord;
    
    return output;
}

float2 lightScreenPosition;

float2 screenRes = float2(4,3);

float4x4 matVP;

float2 halfPixel;

float SunSize = 1500;

texture scene;
sampler2D Scene
{
	Texture = (scene);
	AddressU = Clamp;
	AddressV = Clamp;
};

//texture flare;
sampler Flare : register(s0);
//{
//    Texture = (flare);
//    AddressU = CLAMP;
//    AddressV = CLAMP;
//};

float4 LightSourceMaskPS(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
	texCoord -= halfPixel;

	// Get the scene
	float4 col = 0;
	
	// Find the suns position in the world and map it to the screen space.
		float2 coord;
		
		float size = SunSize / 1;
					
		float2 center = lightScreenPosition;

		coord = .5 - ((texCoord - center) * screenRes) / size * .5f;
		
		col += (pow(tex2D(Flare,coord),2) * 1) * 2;						
	
	
    return col;// * tex2D(Scene, texCoord);
}

technique LightSourceMask
{
	pass p0
	{
		//VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 LightSourceMaskPS();
	}
}