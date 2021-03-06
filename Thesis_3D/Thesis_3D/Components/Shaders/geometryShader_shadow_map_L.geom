#version 450 core
layout(triangles) in;

uniform mat4 shadowMatrices[6];
layout(triangle_strip, max_vertices = 18) out;
out vec4 FragPos;

void main()
{
	for(int face = 0; face < 6; face++)
	{
		gl_Layer = face;
		for(int i = 0; i < 3; i++)
		{
			FragPos = gl_in[i].gl_Position;
			gl_Position = shadowMatrices[face] * vec4(vec3(FragPos.xyz), 1);
			EmitVertex();
		}
		EndPrimitive();
	}
}