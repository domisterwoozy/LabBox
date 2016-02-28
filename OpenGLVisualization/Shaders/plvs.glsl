#version 330 core

uniform mat4 mvp;

in vec3 vertexPos;
in vec4 vertexColor;
in vec3 vertexNormal;

out vec3 fragVert;
out vec4 fragColor;
out vec3 fragNormal;

void main() {
    // Pass some interpolated variables to the fragment shader
    fragColor = vertexColor;
    fragNormal = vertexNormal;
    fragVert = vertexPos;
    
    // Apply all matrix transformations to vert
    gl_Position = mvp * vec4(vertexPos, 1);
}