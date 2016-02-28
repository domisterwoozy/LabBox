#version 330
#define MAX_LIGHTS 10

uniform mat4 model;
uniform vec3 cameraPos;

// light properties
uniform int numLights;
uniform vec4 lightPositions[MAX_LIGHTS]; // world coords
uniform vec3 lightColors[MAX_LIGHTS];
uniform float lightPowers[MAX_LIGHTS];
uniform float lightAttenuations[MAX_LIGHTS];
uniform float ambientIntensities[MAX_LIGHTS]; 
uniform float coneAngles[MAX_LIGHTS];
uniform vec3 coneDirections[MAX_LIGHTS];

// material properties
uniform float materialShininess;
uniform vec3 materialSpecularColor;

in vec4 fragColor;
in vec3 fragNormal;
in vec3 fragPos;

out vec4 finalColor;

vec3 ApplyLight(vec4 lightPos, vec3 lightColor, float lightPower, float attenuationCoef, vec3 coneDir, float coneAngle,
				float ambientCoef, vec3 surfaceColor, vec3 normal, vec3 surfacePos, vec3 surfaceToCamera)
{
    vec3 surfaceToLight;
    float attenuation = 1.0;
    if(lightPos.w == 0.0)
	{
        //directional light
        surfaceToLight = normalize(lightPos.xyz);
        attenuation = 1.0; //no attenuation for directional lights
    }
	else
	{
        //point light
        surfaceToLight = normalize(lightPos.xyz - surfacePos);
        float distanceToLight = length(lightPos.xyz - surfacePos);
        attenuation = 1.0 / (1.0 + attenuationCoef * pow(distanceToLight, 2));

        //cone restrictions (affects attenuation)
        float lightToSurfaceAngle = acos(dot(-surfaceToLight, normalize(coneDir)));
        if(lightToSurfaceAngle > coneAngle) // we are outside of cone
		{
            attenuation = 0.0;
        }
    }

    //ambient
    vec3 ambient = ambientCoef * surfaceColor.rgb * lightColor;

    //diffuse
    float diffuseCoefficient = lightPower * max(0.0, dot(normal, surfaceToLight));
    vec3 diffuse = diffuseCoefficient * surfaceColor.rgb * lightColor;
    
    //specular
    float specularCoefficient = 0.0;
    if(diffuseCoefficient > 0.0)
        specularCoefficient = pow(max(0.0, dot(surfaceToCamera, reflect(-surfaceToLight, normal))), materialShininess);
    vec3 specular = specularCoefficient * materialSpecularColor * lightColor;

    //linear color (color before gamma correction)
    return ambient + attenuation*(diffuse + specular);
}

void main() {
    vec3 normal = normalize(transpose(inverse(mat3(model))) * fragNormal);
    vec3 surfacePos = vec3(model * vec4(fragPos, 1));
    vec3 surfaceToCamera = normalize(cameraPos - surfacePos);   
    
	vec3 linearColor = vec3(0);
	for(int i = 0; i < numLights; ++i)
	{
		linearColor += ApplyLight(lightPositions[i], lightColors[i], lightPowers[i], lightAttenuations[i], coneDirections[i], coneAngles[i],
								  ambientIntensities[i], fragColor.rgb, normal, surfacePos, surfaceToCamera);
	}

	//final color (after gamma correction)
    vec3 gamma = vec3(1.0/2.2);
    finalColor = vec4(pow(linearColor, gamma), fragColor.a);
}

