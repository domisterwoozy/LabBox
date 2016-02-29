#version 330 core
#define MAX_LIGHTS 10

uniform mat4 mvp;

uniform int numLights;
uniform int castsShadows[MAX_LIGHTS];
uniform mat4 depthBiasMVPs[MAX_LIGHTS];

in vec3 vertexPos;
in vec4 vertexColor;
in vec3 vertexNormal;

out vec3 fragPos;
out vec4 fragColor;
out vec3 fragNormal;
out vec4 shadowPositions[MAX_LIGHTS];

void main() {
    // Pass some interpolated variables to the fragment shader
    fragColor = vertexColor;
    fragNormal = vertexNormal;
    fragPos = vertexPos;
    
    // Apply all matrix transformations to vert
    gl_Position = mvp * vec4(vertexPos, 1);

	for(int i = 0; i < numLights; ++i)
	{
		if (castsShadows[i] == 1)
		{
			shadowPositions[i] = depthBiasMVPs[i] * vec4(vertexPos, 1); // the position of the vertex as seen by light casting shadows
		}		
	}
	
}