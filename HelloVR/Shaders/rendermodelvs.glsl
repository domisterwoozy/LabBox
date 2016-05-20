﻿#version 410
uniform mat4 matrix;
layout(location = 0) in vec4 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoordIn;
out vec2 texCoord;
void main()
{
	texCoord = texCoordIn;
	gl_Position = matrix * vec4(position.xyz, 1);
}