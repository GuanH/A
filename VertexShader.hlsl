struct VsOut
{
	float4 color : Color;
	float4 pos:SV_Position;
};


VsOut main(float2 pos : Position, float4 col : Color)
{ 
	VsOut vso;
	vso.pos = float4(pos.x, pos.y, 0.0f , 1.0f);
	vso.color = col;
	return vso;
}