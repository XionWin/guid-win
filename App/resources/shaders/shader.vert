#version 300 es

layout(location = 0) in vec2 vertex;
layout(location = 1) in vec2 tcoord;
uniform vec2 viewSize;
out vec2 ftcoord;
out vec2 fpos;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 viewZoom;

void main(void) {
    ftcoord = tcoord;
    fpos = vertex;
    vec4 v = vec4(vertex, 0, 1) * model;
    gl_Position = vec4(2.0 * v.x / viewSize.x - 1.0, 1.0 - 2.0 * v.y / viewSize.y, v.z / viewSize.x, 1) * view * projection * viewZoom;
    // gl_Position = vec4(vertex.x, vertex.y, 0, 1);
}