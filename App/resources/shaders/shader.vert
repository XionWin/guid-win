#version 300 es

layout(location = 0) in vec2 vertex;
layout(location = 1) in vec2 tcoord;
uniform vec2 viewSize;
out vec2 ftcoord;
out vec2 fpos;

void main(void) {
    ftcoord = tcoord;
    fpos = vertex;
    gl_Position = vec4(2.0 * vertex.x / viewSize.x - 1.0, 1.0 - 2.0 * vertex.y / viewSize.y, 0, 1);
    // gl_Position = vec4(vertex.x, vertex.y, 0, 1);
}