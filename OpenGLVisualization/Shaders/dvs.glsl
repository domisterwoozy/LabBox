#version 330 core
// INPUT, recieved from buffers
// all in model space
in vec3 vertexPos; // in model space
in vec4 vertexColor;
in vec3 vertexNormal; // in model space

// OUTPUT, automatically interpolated and sent to the fragment shader
out vec4 diffuseColor;
out vec3 posWorld;
out vec3 eyeDirCamera;
out vec3 lightDirCamera;
out vec3 normalCamera;

//UNIFORMS, set in code, remmbered in program state machine
// all in world space
uniform mat4 MVP;
uniform mat4 V;
uniform mat4 M;
uniform vec3 lightPosWorld;

void main()
{
	// positions in all 3 coordinate spaces
	gl_Position = MVP * vec4(vertexPos, 1);

	posWorld = (M * vec4(vertexPos, 1)).xyz;
	vec3 posCamera = (V * M * vec4(vertexPos, 1)).xyz;

	// vector from the vertex to the camera in camera space
	eyeDirCamera = normalize(vec3(0,0,0) - posCamera);

	// vector that goes from the vertex to the light, in camera space
	vec3 lightPosCamera = (V * vec4(lightPosWorld, 1)).xyz;
	lightDirCamera = normalize(lightPosCamera + eyeDirCamera);

	normalCamera = normalize((V * M * vec4(vertexNormal, 0)).xyz); // Only correct if ModelMatrix does not scale the model ! Use its inverse transpose if not.

	diffuseColor = vertexColor;
}