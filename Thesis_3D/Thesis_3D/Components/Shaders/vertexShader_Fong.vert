#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 18) uniform vec4 vec_LightPosition;
layout(location = 20) uniform mat4 view;
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;




out vec4 eyePosition;
out vec3 tnorm;
out vec4 frag_LightPosition;

void main(void)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	tnorm = normalize(NormalMatrix * vec3(vec_normal));
	eyePosition = modelView * vec_position;
	frag_LightPosition = vec_LightPosition;
	gl_Position = view * modelView * vec_position;
}