#version 330

out vec4 outputColor;

uniform vec4 aColor;

in vec2 texCoord;
uniform usampler2D texture0;
uniform vec4 palette[256];
uniform uint maskIndex;

void main()
{
    uvec4 vec_tex = texture(texture0, texCoord);

    uint index = vec_tex.r;

    if(index == maskIndex)
        discard;

    float r = palette[index].r;
    float g = palette[index].g;
    float b = palette[index].b;
    float a = palette[index].a;

    outputColor = vec4(r, g, b, a) * aColor;
}