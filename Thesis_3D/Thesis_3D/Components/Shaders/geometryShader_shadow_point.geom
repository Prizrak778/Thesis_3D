layout(triangles) in;
layout(triangle_strip, max_vertices=120) out;

layout(binding = 6) buffer buffer1
{
	vec4 dad[];
};
in mat4 view_geom[];
in vec4 LightPos_geom[];
void main()
{
	int flag = 0;
	vec3 N;
	float D;
	float mu;
	float mu_znam;
	float total;
	int i;
	for(i = 0; i < dad.length() - 3; i+=3)
	{
		flag = 0;
		for(int j = 0; j < 3; j++)
		{
			
			if(dad[i+j].xyz == gl_in[j].gl_Position.xyz)
			{
				break;
			}
			N = cross((dad[i+1].xyz - dad[i].xyz), (dad[i+2].xyz - dad[i].xyz));
			if(N == vec3(0.0))
			{
				break;
			}
			D = -(N.x * dad[i].x + N.y * dad[i].y + N.z * dad[i].z);
			mu  = D + N.x * gl_in[j].gl_Position.x + N.y * gl_in[j].gl_Position.y + N.z * gl_in[j].gl_Position.z;
			mu_znam = N.x * (gl_in[j].gl_Position.x - LightPos_geom[j].x) + N.y * (gl_in[j].gl_Position.y - LightPos_geom[j].y) + N.z * (gl_in[j].gl_Position.z - LightPos_geom[j].z);
			if(mu_znam!=0)
			{
				mu = mu / mu_znam;
				if(1 > mu && mu > 0)
				{
					vec3 p =  gl_in[j].gl_Position.xyz + mu*(LightPos_geom[j].xyz-gl_in[j].gl_Position.xyz);
					vec3 p1 = normalize(dad[i].xyz - p);
					vec3 p2 = normalize(dad[i + 1].xyz - p);
					vec3 p3 = normalize(dad[i + 2].xyz - p);
					float a1 = p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
					float a2 = p2.x * p3.x + p2.y * p3.y + p2.z * p3.z;
					float a3 = p3.x * p1.x + p3.y * p1.y + p3.z * p1.z;
					total = (acos(a1)+acos(a2)+acos(a3))*10;
					if(abs(total-360) > 0)
					{
						flag++;
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			else
			{
				break;
			}
		}
		if(flag == 3)
		{
			for(int j = 0; j < 3; j++)
			{
				gl_Position = view_geom[j] * gl_in[j].gl_Position;
				EmitVertex();
			}
			EndPrimitive();
			break;
		}
		else
		{
			//for(int j = 0; j < 3; j++)
			//{
			//	gl_Position = vec4(0.0);
			//	EmitVertex();
			//}
			//EndPrimitive();
			gl_Position = view_geom[0] * vec4(N, 1.0);
			EmitVertex();
			gl_Position = view_geom[1] * vec4(0.0, 0.0, 0.0, 1.0);
			EmitVertex();
			gl_Position = view_geom[2] * vec4(0.0, 0.0, 0 , 1.0);
			EmitVertex();
			EndPrimitive();		
		}

	}
	//if(flag == 3)
	//{
	//	for(int j = 0; j < 3; j++)
	//	{
	//		gl_Position = view_geom[j] * gl_in[j].gl_Position;
	//		EmitVertex();
	//	}
	//	EndPrimitive();
	//}
	//else
	//{
	//	gl_Position = view_geom[0] * vec4(N, 1.0);
	//	EmitVertex();
	//	gl_Position = view_geom[1] * vec4(1.0, 1, 0.0, 1.0);
	//	EmitVertex();
	//	gl_Position = view_geom[2] * vec4(1.0, 0.0, 1, 1.0);
	//	EmitVertex();
	//	EndPrimitive();		
	//}
}