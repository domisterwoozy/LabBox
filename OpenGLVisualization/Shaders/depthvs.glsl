﻿#version 330 core

in vec3 vertexPos;

uniform mat4 MVP;

void main()
{
	gl_Position = MVP * vec4(vertexPos, 1);
}

