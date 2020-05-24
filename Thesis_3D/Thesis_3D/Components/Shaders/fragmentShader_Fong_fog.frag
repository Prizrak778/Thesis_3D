#version 450 core

in vec4 eyePosition;
in vec3 tnorm;
in vec4 frag_LightPosition;
in vec4 obj_position;
layout(location = 23) uniform vec4 CamerPosition;
layout(location = 24) uniform vec3 LightIntensity;
layout(location = 25) uniform vec3 Kd;
layout(location = 27) uniform vec3 Ka;
layout(location = 29) uniform vec3 Ks;
layout(location = 30) uniform float maxDist;
layout(location = 31) uniform float minDist;
layout(location = 32) uniform vec3 fogcolor;

out vec4 color;

void main(void)
{
	float Shininess = 32.0;
	
	vec3 n = normalize(tnorm);
	vec3 s = normalize(vec3(frag_LightPosition - eyePosition));
	vec3 v_b = normalize(vec3(CamerPosition.xyz));
	vec3 r = reflect(-s,n);
	vec3 color_temp = LightIntensity * (Ka + Kd * max(dot(s, n), 0.0) + Ks * pow(max(dot(r,v_b), 0.0), Shininess ));
	float dist = length(CamerPosition.xyz - obj_position.xyz);
	float fogFactor = (maxDist - dist) / (maxDist - minDist);
	fogFactor = clamp(fogFactor, 0.0, 1.0);
	vec3 shadecolor = mix(fogcolor, color_temp, fogFactor);
	color = vec4(shadecolor, 1.0);
}