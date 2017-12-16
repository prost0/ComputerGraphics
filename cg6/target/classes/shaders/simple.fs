#version 120

//Yakomovich Alexander 8O-308B

uniform float red = 172;
uniform float green = 64;
uniform float blue = 0;


uniform vec3 col;
uniform vec3 light = vec3(0,0,1);
uniform vec3 Normal;


uniform float ambK = 1;
uniform float diffK = 1;
uniform float specK = 1;

vec4 calc(){
    vec3 c = vec3(red/255.f,blue/255.f,green/255.f);
    vec3 amb = ambK * c;

    vec3 n = normalize(Normal);
    float diff = max(dot(n,light),0);
    vec3 diffuse = diffK * diff * col;

    float spec = pow(dot(n,light),32);
    vec3 specular = specK * spec * c;

    return vec4(amb + diffuse + specular,1);
}

void main() {
    gl_FragColor = calc();
}