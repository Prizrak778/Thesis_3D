﻿#version 450 core

in VS_OUT
{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}vs_in;

layout(location = 23) uniform vec4 CamerPosition;
layout(location = 18) uniform vec4 LightPos;
layout(location = 7) uniform float far_plane;

layout(location = 31) uniform vec3 OffsetTexSize;
uniform samplerCube  depthTex;
uniform sampler3D OffsetTex;

out vec4 color;

float ShadowCalculation(vec3 FragPos)
{ 

	ivec3 offsetCoord;
	vec3 fragToLight = FragPos - LightPos.xyz;
	offsetCoord.xy = ivec2(mod(fragToLight.xy, OffsetTexSize.xy));
	int samplesDiv2 = int(OffsetTexSize.z);
	for(int i = 0; i < 4; i++)
	{
		offsetCoord.z = i;
		vec4 offset = texelFetch(OffsetTex, offsetCoord, 0);
	}
	float currentDepth = length(fragToLight);
	float sum = 0.0;
	float shadow = 0.0;
	float bias = 0.05;
	float viewDistance = length(CamerPosition.xyz - FragPos);
	float diskRadius = (1.0 + (viewDistance / far_plane)) / 25.0;
	for(int i = 0; i < 4; ++i)
	{
		float closestDepth = texture(depthTex, fragToLight + (texelFetch(OffsetTex,offsetCoord,0) * diskRadius).xyz).r;
		closestDepth *= far_plane;
		if(currentDepth - bias > closestDepth)
			sum += 1.0;
	}
	shadow = sum / 8.0; 
	if( shadow != 1.0 && shadow != 0.0 ) {
		for( int i = 4; i < samplesDiv2; i++ ) {
			offsetCoord.z = i;
			float closestDepth = texture(depthTex, fragToLight + (texelFetch(OffsetTex,offsetCoord,0) * diskRadius).xyz).r;
			closestDepth *= far_plane;
			if(currentDepth - bias > closestDepth)
				sum += 1.0;
		}
		shadow = sum / float(samplesDiv2 * 2.0);
	}
	return shadow;
}

void main(void)
{
	//vec3 color = texture(diffuseTexture, vs_in.TexCoords).rgb;
	vec3 normal = normalize(vs_in.Normal);
	vec3 lightColor = vec3(1.0);
	vec3 ambient = vec3(0.0, 0.15, 0.0);
	vec3 lightDir = normalize(-vs_in.FragPos + LightPos.xyz);
	float diff = max(dot(lightDir, normal), 0.0);
	vec3 diffuse = diff * lightColor;
	vec3 viewDir = normalize(CamerPosition.xyz - vs_in.FragPos);
	float spec = 0.0;
	vec3 halfwayDir = normalize(lightDir + viewDir);
	spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
	vec3 specular = spec * lightColor;
	float shadow = ShadowCalculation(vs_in.FragPos);
	vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular));
	color = vec4(lighting, 1.0);
}