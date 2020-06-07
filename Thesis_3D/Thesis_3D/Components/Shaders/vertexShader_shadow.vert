#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 3) in vec4 vec_color;
layout (location = 18) uniform vec4 vec_LightPosition;
layout (location = 20) uniform mat4 view;
layout (location = 21) uniform mat4 projection;
layout (location = 22) uniform mat4 modelView;
layout (location = 23) uniform mat4 modelViewPlane;
layout (binding = 5) buffer bufferPlane
{
	vec4 normal_floor;
	vec4 plane[];
};
out float dotVt;
out vec3 LgthPos;

void main(void)
{
	float min_x = (modelViewPlane * plane[0]).x, max_x = (modelViewPlane * plane[0]).x; 
	float min_z = (modelViewPlane * plane[0]).z, max_z = (modelViewPlane * plane[0]).z;
	float min_y = (modelViewPlane * plane[0]).y, max_y = (modelViewPlane * plane[0]).y;
	for(int i = 0; i < plane.length(); i++)
	{
		min_x = min(min_x, (modelViewPlane * plane[i]).x);
		min_y = min(min_y, (modelViewPlane * plane[i]).y);
		min_z = min(min_z, (modelViewPlane * plane[i]).z);
		max_x = max(max_x, (modelViewPlane * plane[i]).x);
		max_y = max(max_y, (modelViewPlane * plane[i]).y);
		max_z = max(max_z, (modelViewPlane * plane[i]).z);
	}
	mat3 NormalMatrixPlane = mat3(transpose(inverse(modelViewPlane)));
	vec3 tnorm = normalize(NormalMatrixPlane * vec3(normal_floor));
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	vec3 s =  vec_LightPosition.xyz;
	vec3 V_pos = (modelView * vec_position).xyz - s;
	float dotV = (dot(tnorm, ((modelViewPlane * plane[0]).xyz - s))/dot(tnorm, (V_pos)));
	LgthPos = (modelViewPlane * plane[6]).xyz;
	dotVt = dotV;
	vec3 W_pos = V_pos * dotV;
    vec3 B_pos = W_pos + s;
	if(B_pos.x > max_x)
	{
		B_pos.x = max_x;
	}
	if(B_pos.x < min_x)
	{
		B_pos.x = min_x;
	}
	if(B_pos.y > max_y)
	{
		B_pos.y = max_y;
	}
	if(B_pos.y < min_y)
	{
		B_pos.y = min_y;
	}
	if(B_pos.z > max_z)
	{
		B_pos.z = max_z;
	}
	if(B_pos.z < min_z)
	{
		B_pos.z = min_z;
	}
	if(s.y > B_pos.y) B_pos.y += 0.02;
	else B_pos.y -= 0.02;
	vec4 pos_gl = vec4(0.0);
	if(dot(tnorm, (V_pos)) < -0.01 && dotV>=1)
	{
		pos_gl = vec4(B_pos, 1.0);	
	}
	gl_Position = view * pos_gl;
}