#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 18) uniform vec4 vec_LightPosition;
layout(location = 20) uniform mat4 view; //modelView * projection
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout(location = 23) uniform vec4 CamPosition;

out vec4 vs_color;

void getEyeSpace(out vec3 norm, out vec4 position)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	norm = normalize (NormalMatrix * vec3(vec_normal));
	position = modelView * vec4(vec_position.xyz, 1.0);
} 

void main(void)
{
	vec3 Kd = vec3(1.0, 0.0, 0.0);
	vec3 Ka = vec3(0.0, 0.15, 0.0);
	vec3 Ks = vec3(0.0, 0.0, 0.0);
	vec3 LightIntensity = vec3(1.0, 1.0, 1.0);
	float Shininess = 16.0;
	
	vec3 tnorm;
	vec4 eyeCoords;
	getEyeSpace(tnorm, eyeCoords);
	vec3 s = normalize(vec3(vec_LightPosition - eyeCoords));
	vec3 v = normalize(CamPosition.xyz);
	vec3 r = reflect( -s , tnorm );
	
	gl_Position = view * modelView * vec_position;
	vec3 color_temp = LightIntensity*(Ka + Kd * max(dot(r, tnorm), 0.0) + Ks * pow(max(dot(r,v), 0.0), Shininess));
	vs_color = vec4(color_temp, 1.0);
}