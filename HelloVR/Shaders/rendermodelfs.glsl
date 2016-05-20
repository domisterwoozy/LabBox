#version 410 core
uniform sampler2D diffuse;
in vec2 texCoord;
out vec4 outputColor;
void main()
{
	outputColor = texture(diffuse, texCoord);
}