#version 330 core
// IN
// interpolated values from the vertex shaders
in vec4 diffuseColor;
in vec3 posWorld;
in vec3 normalCamera;
in vec3 eyeDirCamera;
in vec3 lightDirCamera;

// OUT
// opengl assumes all output from a fragment shader represents colors
out vec4 color;

// UNIFORMS
// material properties
uniform vec4 specularColor;
// single light properties, need to make these array for multiple lights
uniform vec3 lightPosWorld;
uniform vec4 lightColor;
uniform float lightPower;

void main()
{

	vec4 ambientColor = vec4(0.1,0.1,0.1,1.0) * diffuseColor;
	float distance = length(lightPosWorld - posWorld); // distance from fragment to light source
	float cosTheta = clamp(dot(normalCamera, eyeDirCamera), 0, 1);

	// direction in which the fragment reflects light
	vec3 reflectionDir = reflect(-lightDirCamera, normalCamera);

	float cosAlpha = clamp(dot(eyeDirCamera, reflectionDir), 0, 1);

	color = 
		ambientColor + // ambient color in all directions
		diffuseColor * lightColor * lightPower * cosTheta / (distance*distance) + // standard color
		specularColor * lightColor * lightPower * pow(cosAlpha, 5) / (distance*distance); // reflective color
}