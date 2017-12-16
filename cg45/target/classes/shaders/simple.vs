#version 120
//Yakomovich Alexander 8O-308B

attribute vec3 vert;

uniform mat4 project;
uniform vec3 oz = vec3(0,0,1);
uniform vec3 Normal;
uniform float css = 1;

void main() {
    vec3 n = normalize(Normal);
    float c = dot(n,oz);
    vec4 pos;
    //vec3 v = vec3(0.2,0.2,0.2);
    pos = vec4((1+css/3) * vert,1);
    gl_Position = project * pos;
}