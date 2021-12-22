#version 330 core

layout(location = 0) in mat4 Projection;
layout(location = 1) in mat4 ModelView;
layout(location = 2) in vec2 aTexCoord;

in vec4 vertexPosition; 
out vec2 texCoord;

// Add a uniform for the transformation matrix.
uniform mat4 transform;

void main(void)
{
    texCoord = aTexCoord;

    gl_Position = Projection * ModelView * vertexPosition;
}