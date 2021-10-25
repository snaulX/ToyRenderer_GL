#version 330 core
out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D texture1;
uniform vec4 color;

void main()
{
    /*vec4 colortex = texture(texture1, TexCoord);
    if (colortex == vec4(1))
	    FragColor = color;
    else
        FragColor = colortex;*/
    FragColor = texture(texture1, TexCoord);// * color;
}