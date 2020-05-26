#version 450 core

in VS_OUT
{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}vs_in;

layout(location = 23) uniform vec4 CamerPosition;
layout(location = 18) uniform vec4 LightPos;
layout(location = 7) uniform float far_plane;

uniform samplerCube  depthTex;

out vec4 color;

float ShadowCalculation(vec3 FragPos)
{ 
	vec3 fragToLight = FragPos - LightPos.xyz;
	float closestDepth = texture(depthTex, fragToLight).r;  
	closestDepth *= far_plane;
	float currentDepth = length(fragToLight);
	float bias = 0.05;
	float shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;
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