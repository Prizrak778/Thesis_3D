#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout(location = 19) uniform vec4 vec_color;
layout(location = 20) uniform mat4 view; //modelView * projection
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;


out vec4 vs_color;

void main(void)
{
	gl_Position = view * vec_position;
	vs_color = vec_color;
}