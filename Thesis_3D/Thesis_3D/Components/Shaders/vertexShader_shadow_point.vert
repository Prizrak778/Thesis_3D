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
layout(location = 31) uniform int offset;
layout(location = 32) uniform int size;
layout(binding = 3) buffer bufferObject
{
	vec4 vertexArray[];
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
	float eps = 0.00000001;
	float total;
	float testFlag = 0;
	const float PI = 3.1415926535897932384626433832795;
	for(int i = 0; (i < vertexArray.length()) && (flag == 0) && (flagLight != 1); i+=3)
	{
		vec3 pa = vertexArray[i].xyz;
		vec3 pb = vertexArray[i + 1].xyz;
		vec3 pc = vertexArray[i + 2].xyz;
		vec3 p2 = (modelView * vec_position).xyz;
		if(offset <= i && i < (offset + size))
		{
			continue;
		}
		//testFlag = i;
		N = normalize(cross((pb - pa), (pc - pa)));
		D = - N.x * pa.x - N.y * pa.y - N.z * pa.z;
		mu = N.x * LightPos.x + N.y * LightPos.y + N.z * LightPos.z;
		
		mu  = -(D + mu);
		mu_znam = N.x * (p2.x - LightPos.x) + N.y * (p2.y - LightPos.y) + N.z * (p2.z - LightPos.z);
		if(abs(mu_znam)< eps)
		{
			continue;
		}
		mu = mu / mu_znam;
		if(mu < 0 || mu >= 1)
		{
			continue;
		}
		vec3 p =  LightPos.xyz + mu * (p2 - LightPos.xyz);
		vec3 pa1 = normalize(pa - p);
		vec3 pa2 = normalize(pb - p);
		vec3 pa3 = normalize(pc - p);
		float a1 = pa1.x * pa2.x + pa1.y * pa2.y + pa1.z * pa2.z;
		float a2 = pa2.x * pa3.x + pa2.y * pa3.y + pa2.z * pa3.z;
		float a3 = pa3.x * pa1.x + pa3.y * pa1.y + pa3.z * pa1.z;
		total = (acos(a1)+acos(a2)+acos(a3))*180/PI;
		if((total - 360) < eps)
		{
			flag = 1;
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
	//if(testFlag > 0) vs_color = vec4(vec3(0.0), 1);
	//if(testFlag > 1) vs_color = vec4(1, 0, 0, 1);
	if(flag > 0) vs_color = vec4(vec3(0.0), 1);
}