#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 3) in vec2 tex_coord;
layout(location = 20) uniform mat4 view;
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout(location = 30) uniform mat4 ShadowMatrix;

out VS_OUT
{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}vs_out;

void main(void)
{
	vs_out.FragPos = vec3(modelView * vec_position);
	vs_out.Normal = transpose(inverse(mat3(modelView)))*vec3(vec_normal);
	vs_out.TexCoords = tex_coord;
	gl_Position = view * modelView * vec_position;
}