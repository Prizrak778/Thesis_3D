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

void main(void)
{
	vec3 positionPlane = (modelViewPlane * vec4(0, 0, 0, 1)).xyz;
	float min_x=plane[0].x + positionPlane.x, max_x=plane[0].x + positionPlane.x; 
	float min_z=plane[0].z + positionPlane.y, max_z=plane[0].z + positionPlane.y;
	float min_y=plane[0].y + positionPlane.z, max_y=plane[0].y + positionPlane.z;
	for(int i = 0; i<plane.length(); i++)
	{
		min_x = min(min_x, plane[i].x + positionPlane.x);
		min_y = min(min_y, plane[i].y + positionPlane.y);
		min_z = min(min_z, plane[i].z + positionPlane.z);
		max_x = max(max_x, plane[i].x + positionPlane.x);
		max_y = max(max_y, plane[i].y + positionPlane.y);
		max_z = max(max_z, plane[i].z + positionPlane.z);
	}
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	vec3 s =  vec_LightPosition.xyz * vec3(1.0, 1.0, 1.0);
	vec3 V_pos = (modelView * vec_position).xyz - s;
	float dotV = (dot(normal_floor.xyz, (plane[0].xyz + positionPlane - s))/dot(normal_floor.xyz, (V_pos)));
	vec3 W_pos = V_pos * dotV;
    vec3 B_pos = s + W_pos;
	dotVt = dot(normal_floor.xyz, (plane[0].xyz + positionPlane - s))/dot(normal_floor.xyz, (V_pos));
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
	if(s.y > B_pos.y) B_pos.y += 0.02;
	else B_pos.y -= 0.02;
	vec4 pos_gl = vec4(0.0);
	if(dot(normal_floor.xyz, (V_pos))<-0.01 && dotV>=1)
	{
		pos_gl = vec4(B_pos, 1.0);	
	}
	gl_Position = view * modelView * pos_gl;
}