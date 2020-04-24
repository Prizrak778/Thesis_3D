#version 450 core 

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 18) uniform vec4 vec_LightPosition;
layout(location = 20) uniform mat4 view;
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout(location = 24) uniform vec3 Ld;
layout(location = 25) uniform vec4 Kd;


out vec4 vs_color;

void main(void)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	vec3 tnorm = normalize(NormalMatrix * vec3(vec_normal));
	vec4 eyeCoords = modelView * vec_position;
	
	vec3 s = normalize(vec3(vec_LightPosition - eyeCoords));
	vec4 all_color = vec4(Ld, 1.0f) * Kd * vec4(max(dot( s, tnorm ), 0.0 ));
	gl_Position = view * modelView * vec_position;
	vs_color =  vec4(vec4(all_color).xyz + vec4(0.0,0.15,0.0,0.0).xyz, 1.0f);
}