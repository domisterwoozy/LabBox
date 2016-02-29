#version 330 core

out vec4 color;

uniform sampler2D texture;

in vec2 UV;

void main()
{
	color = texture2D(texture, UV);
}
