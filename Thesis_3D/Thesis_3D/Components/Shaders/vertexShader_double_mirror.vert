#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 18) uniform vec4 vec_LightPosition;
layout(location = 20) uniform mat4 view; //modelView * projection
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout(location = 23) uniform vec4 CamPosition;

out vec4 FrontColor;
out vec4 BackColor;

void getEyeSpace(out vec3 norm, out vec4 position)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	norm = normalize (NormalMatrix * vec3(vec_normal));
	position = modelView * vec_position;
} 
void phongModel(vec4 position, vec3 norm, out vec4 color_temp)
{

	vec3 Kd = vec3(0.5, 0.5, 0.5);
	vec3 Ka = vec3(1.0, 1.0, 1.0);
	vec3 Ks = vec3(0.5, 0.5, 0.5);
	vec3 Ld = vec3(1.0, 1.0, 1.0);
	vec3 La = vec3(0.0, 0.15, 0.0);
	vec3 Ls = vec3(1.0, 1.0, 1.0);
	float Shininess = 16.0;
	
	vec3 s = normalize(vec3(vec_LightPosition - position));
	vec3 v = normalize(CamPosition.xyz);
	vec3 r = reflect( -s , norm );
	
	vec3 ambient = La * Ka;
	float sDotN = max(dot (s, norm), 0.0);
	vec3 diffuse = Ld * Kd * sDotN;
	
	vec3 spec = vec3(0.0);
	if(sDotN > 0.0)
		spec = Ls*Ks*pow(max(dot(r, v), 0.0), Shininess);
		
	color_temp = vec4(vec3(ambient + diffuse + spec), 0.0);
}

void main(void)
{
	vec3 tnorm;
	vec4 eyeCoords;
	getEyeSpace(tnorm, eyeCoords);
	gl_Position = view * modelView * vec_position;
	phongModel(eyeCoords, tnorm, FrontColor);
	phongModel(eyeCoords, -tnorm, BackColor);
}