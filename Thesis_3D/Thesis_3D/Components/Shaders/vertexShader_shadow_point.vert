#version 450 core

layout (location = 1) in vec4 vec_position;
layout (location = 2) in vec4 vec_normal;
layout(location = 18) uniform vec4 LightPos;
layout(location = 20) uniform mat4 view;
layout(location = 21) uniform mat4 projection;
layout(location = 22) uniform mat4 modelView;
layout(location = 23) uniform vec4 CamPosition;
layout(location = 24) uniform vec3 Ld;
layout(location = 25) uniform vec3 Kd;
layout(location = 26) uniform vec3 La;
layout(location = 27) uniform vec3 Ka;
layout(location = 28) uniform vec3 Ls;
layout(location = 29) uniform vec3 Ks;
layout(location = 30) uniform int flagLight;
layout(binding = 6) buffer bufferObject
{
	vec4 vertexArray[];
}; 
layout(binding = 7) buffer bufferPosition
{
	mat4 modelViewPosition[];
};
out vec4 vs_color;

void getEyeSpace(out vec3 norm, out vec4 position)
{
	mat3 NormalMatrix = mat3(transpose(inverse(modelView)));
	norm = normalize (NormalMatrix * vec3(vec_normal));
	position = modelView * vec_position;
} 
void main(void)
{
	int flag = 0;
	vec3 N = vec3(0.0);
	float D;
	float mu;
	float mu_znam;
	float eps = 0.0000001;
	float total;
	float testFlag = 0;
	const float PI = 3.1415926535897932384626433832795;
	int j = 0;
	int offset = int(positionArray[0].w);
	for(int i = 0; i < 12 && flag == 0 && flagLight != 1; i+=3)
	{
		
		if(length((vertexArray[i].xyz + positionArray[j].xyz) - (modelView * vec_position).xyz) < eps)
		{
			continue;
		}
		
		N = cross((vertexArray[i+1].xyz - vertexArray[i].xyz + positionArray[j].xyz  + positionArray[j].xyz), (vertexArray[i+2].xyz - vertexArray[i].xyz + positionArray[j].xyz  + positionArray[j].xyz));
		normalize(N);
		D = - N.x * vertexArray[i].x - N.y * vertexArray[i].y - N.z * vertexArray[i].z;
		mu  = -(D + N.x * LightPos.x + N.y * LightPos.y + N.z * LightPos.z);
		mu_znam = N.x * ((modelView * vec_position).x - LightPos.x) + N.y * ((modelView * vec_position).y - LightPos.y) + N.z * ((modelView * vec_position).z - LightPos.z);
		if(abs(mu_znam)< eps)
		{
			continue;
		}
		mu = mu / mu_znam;
		if(1 < mu || mu < 0)
		{
			continue;
		}
		
		vec3 p =  LightPos.xyz + mu * ((modelView * vec_position).xyz - LightPos.xyz);
		vec3 p1 = normalize(vertexArray[i].xyz + positionArray[j].xyz - p);
		vec3 p2 = normalize(vertexArray[i + 1].xyz + positionArray[j].xyz - p);
		vec3 p3 = normalize(vertexArray[i + 2].xyz + positionArray[j].xyz - p);
		float a1 = p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
		float a2 = p2.x * p3.x + p2.y * p3.y + p2.z * p3.z;
		float a3 = p3.x * p1.x + p3.y * p1.y + p3.z * p1.z;
		total = (acos(a1)+acos(a2)+acos(a3))*180/PI;
		testFlag = 1;
		if(abs(total-360) < eps)
		{
			flag = 1;
		}
		if(offset < i)
		{
			j++;
			offset += int(positionArray[j].w);
		}
	}
	float Shininess = 16.0;
	
	vec3 tnorm;
	vec4 eyeCoords;
	getEyeSpace(tnorm, eyeCoords);
	
	vec3 s = normalize(vec3(LightPos - eyeCoords));
	vec3 v = normalize(CamPosition.xyz);
	vec3 r = reflect( -s , tnorm );
	
	vec3 ambient = La * Ka;
	float sDotN = max(dot (s, tnorm), 0.0);
	vec3 diffuse = Ld * Kd * sDotN;
	
	vec3 spec = vec3(0.0);
	if(sDotN > 0.0)
		spec = Ls*Ks*pow(max(dot(r, v), 0.0), Shininess);
	
	
	gl_Position = view * modelView * vec_position;
	vs_color = vec4(vec3(ambient + diffuse + spec), 1.0);
	//if(testFlag == 1) vs_color = vec4(1, 0 , 0 , 1);
	if(flag == 1) vs_color = vec4(vec3(0), 1);
}