#version 330

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

in vec3 vPosition;
in vec3 vColor;

out vec4 color;

void main()
{
	gl_Position = proj * view * model * vec4(vPosition, 1.0);
	color = vec4(vColor, 1.0);
}