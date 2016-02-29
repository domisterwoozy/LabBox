#version 330 core

layout(location = 0) out float fragmentDepth; // will write to location 0 in the depthTexture

void main()
{
	fragmentDepth = gl_FragCoord.z;
}
