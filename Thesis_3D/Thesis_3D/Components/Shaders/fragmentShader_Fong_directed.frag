#version 450 core

in vec4 eyePosition;
in vec3 tnorm;
in vec4 frag_LightPosition;

layout(location = 23) uniform vec4 CamerPosition;
layout(binding = 24) uniform SpotLightInfo{
	vec4 position_lgh;
	vec3 intensity_lgh;
	vec3 direction_lgh;
	float exponent_lgh;
	float cutoff_lgh;
} SLI;

out vec4 color;

void main(void)
{
	
	vec3 Kd = vec3(0.5, 0.5, 0.5);
	vec3 Ka = vec3(0.0, 0.15, 0.0);
	vec3 Ks = vec3(0.5, 0.5, 0.5);
	float Shininess = 16;
	vec3 color_temp = vec3(0.0, 0.15, 0.0);
	vec3 n = normalize(tnorm);
	vec3 s = normalize(vec3(SLI.position_lgh - eyePosition));
	float angle = acos(dot(-s, SLI.direction_lgh));
	float cutoff = radians(clamp(SLI.cutoff_lgh, 0.0, 90.0));
	vec3 ambient = SLI.intensity_lgh * Ka;
	
	if(angle < cutoff)
	{
		float SLIFactor = pow(dot(-s, SLI.direction_lgh), SLI.exponent_lgh);
		vec3 v = normalize(vec3(CamerPosition.xyz));
		vec3 r = reflect(-s,n);
		color_temp = ambient + SLIFactor * SLI.intensity_lgh * (Kd * max(dot(s, tnorm), 0.0) + Ks * pow(max(dot(r, v), 0.0), Shininess));
	}
	else
	{
		color_temp = ambient;
	}
	
	color = vec4(color_temp, 1.0);
}