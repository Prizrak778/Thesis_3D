#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 3) in vec2 tex_coord;
layout(location = 20) uniform mat4 view;
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;

void main(void)
{
	gl_Position = modelView * vec_position;
}