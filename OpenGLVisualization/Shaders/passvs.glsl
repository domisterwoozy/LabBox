#version 330 core

in vec3 vertexPos;

out vec2 UV;

void main()
{
	gl_Position = vec4(vertexPos, 1);
	UV = (vertexPos.xy+vec2(1,1))/2.0;
}

