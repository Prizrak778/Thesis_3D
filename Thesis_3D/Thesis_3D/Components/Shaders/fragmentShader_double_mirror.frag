#version 450 core

in vec4 FrontColor;
in vec4 BackColor;
out vec4 color;

void main(void)
{
	if( gl_FrontFacing )
	{
		color = FrontColor;
	}
	else
	{
		color = BackColor;
	}
}