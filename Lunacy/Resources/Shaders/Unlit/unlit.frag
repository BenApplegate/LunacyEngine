#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform vec4 albedo;
uniform sampler2D albedoTexture;

void main()
{
    FragColor = texture(albedoTexture, texCoord) * albedo;
} 