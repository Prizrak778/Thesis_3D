#version 450 core

in vec4 eyePosition;
in vec3 tnorm;
in vec4 frag_LightPosition;
in vec4 obj_position;
layout(location = 23) uniform vec4 CamerPosition;
layout(location = 24) uniform float maxDist;
layout(location = 25) uniform float minDist;
layout(location = 26) uniform vec3 fogcolor;

out vec4 color;

void main(void)
{
	vec3 Kd = vec3(0.5, 0.5, 0.5);
	vec3 Ka = vec3(0.0, 0.3, 0.0);
	vec3 Ks = vec3(0.5, 0.5, 0.5);
	vec3 LightIntensity = vec3(1.0, 1.0, 1.0);
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