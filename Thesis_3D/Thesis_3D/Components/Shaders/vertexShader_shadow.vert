#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout (location = 3) in vec4 vec_color;
layout (location = 4) in vec4 vec_LightPosition;
layout (location = 23) uniform float _z; 
layout(location = 20) uniform mat4 view;
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout (location = 19) uniform vec3 normal_floor;
layout(location = 18) uniform vec4 pos_floor;
layout(binding = 5) buffer buffer1
{
	vec4 dad[];
};



void main(void)
{
	float min_x=dad[0].x, min_z=dad[0].z, max_x=dad[0].x, max_z=dad[0].z, min_y=dad[0].y, max_y=dad[0].y;
	
	for(int i = 0; i<dad.length(); i++)
	{
		min_x = min(min_x, dad[i].x);
		min_y = min(min_y, dad[i].y);
		min_z = min(min_z, dad[i].z);
		max_x = max(max_x, dad[i].x);
		max_y = max(max_y, dad[i].y);
		max_z = max(max_z, dad[i].z);
	}
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	vec4 LghCoords = modelView * vec4(0.0, 0.0, 0.0, 1.0);
	vec3 s = LghCoords.xyz * vec3(-1.0, -1.0, -1.0);
	vec3 V_pos = vec_position.xyz - s.xyz;
	vec3 W_pos = V_pos *(dot(normal_floor, (dad[0].xyz - s.xyz))/dot(normal_floor, (V_pos)));
	vec3 B_pos = s.xyz + W_pos;
	if(B_pos.x>max_x)
	{
		B_pos.x=max_x;
	}
	if(B_pos.x<min_x)
	{
		B_pos.x=min_x;
	}
	if(B_pos.y>max_y)
	{
		B_pos.y=max_y;
	}
	if(B_pos.y<min_y)
	{
		B_pos.y=min_y;
	}
	if(B_pos.z>max_z)
	{
		B_pos.z=max_z;
	}
	if(B_pos.z<min_z)
	{
		B_pos.z=min_z;
	}
	vec4 pos_gl = vec4(0.0);
	if(dot(normal_floor, (V_pos))<-0.1)
	{
		pos_gl = view *vec4(B_pos, 1.0);	
	}
	gl_Position = pos_gl;
}