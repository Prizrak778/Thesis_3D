#version 450 core

layout(location = 1) in vec4 vec_position;
layout(location = 2) in vec4 vec_normal;
layout(location = 20) uniform mat4 view; //modelView * projection
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout(location = 23) uniform vec4 CamPosition;

layout(location = 175) uniform vec4 LightPositiont[3];
layout(location = 27) uniform vec3 LightIntensityt[3];
layout(location = 24) uniform vec3 Kd;
layout(location = 25) uniform vec3 Ka;
layout(location = 26) uniform vec3 Ks;


out vec4 vs_color;

void getEyeSpace(out vec3 norm, out vec4 position)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	norm = normalize (NormalMatrix * vec3(vec_normal));
	position = modelView * vec_position;
} 
vec4 phongModel(int indexLight, vec4 position, vec3 norm)
{
	float Shininess = 16.0;
	vec3 s = normalize(vec3(LightPositiont[indexLight] - position));
	vec3 v = normalize(-position.xyz);
	vec3 r = reflect( -s , norm );
	vec3 ambient = LightIntensityt[indexLight] * Ka;
	float sDotN = max(dot (s, norm), 0.0);
	vec3 diffuse = LightIntensityt[indexLight] * Kd * sDotN;
	
	vec3 spec = vec3(0.0);
	if(sDotN > 0.0)
		spec = LightIntensityt[indexLight]*Ks*pow(max(dot(r, v), 0.0), Shininess);
		
	return vec4(vec3(ambient + diffuse + spec), 1.0);
}

void main(void)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	vec3 tnorm;
	vec4 eyeCoords;
	getEyeSpace(tnorm, eyeCoords);
	vs_color = vec4(0.0);
	for(int i=0;i<3;i++)
		vs_color += phongModel(i, eyeCoords, tnorm);
	gl_Position = view * modelView * vec_position;
}