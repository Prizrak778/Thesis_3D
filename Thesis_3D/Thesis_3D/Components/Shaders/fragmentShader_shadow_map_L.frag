#version 450 core

in vec4 FragPos;
layout(location = 18) uniform vec4 vec_LightPosition;
layout (location = 7) uniform float far_plane;
void main(void)
{
	float lightDistance = length(FragPos.xyz - vec_LightPosition.xyz);
	lightDistance = lightDistance / far_plane;
	gl_FragDepth =  lightDistance;
}