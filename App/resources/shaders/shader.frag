#version 300 es

#define UNIFORMARRAY_SIZE 11

#define scissorMat mat3(frag[0].xyz, frag[1].xyz, frag[2].xyz)
#define paintMat mat3(frag[3].xyz, frag[4].xyz, frag[5].xyz)
#define innerCol frag[6]
#define outerCol frag[7]
#define scissorExt frag[8].xy
#define scissorScale frag[8].zw
#define extent frag[9].xy
#define radius frag[9].z
#define feather frag[9].w
#define strokeMult frag[10].x
#define strokeThr frag[10].y
#define texType int(frag[10].z)
#define type int(frag[10].w)

precision highp float;
uniform vec4 frag[UNIFORMARRAY_SIZE];
uniform sampler2D tex;
in vec2 ftcoord;
in vec2 fpos;
out vec4 outColor;


// float sdroundrect(vec2 pt, vec2 ext, float rad) {
// 	vec2 ext2 = ext - vec2(rad, rad);
// 	vec2 d = abs(pt) - ext2;
// 	return min(max(d.x, d.y),0.0) + length(max(d, 0.0)) - rad;
// }

// Scissoring
float scissorMask(vec2 p) {
	vec2 sc = abs((scissorMat * vec3(p, 1.0)).xy) - scissorExt;
	sc = vec2(0.5, 0.5) - sc * scissorScale;
	// if (p.y > 200.0)
	// 	return 1.0;
	// else
	// 	return 0.0;
	return clamp(sc.x, 0.0, 1.0) * clamp(sc.y, 0.0, 1.0);
}

float strokeMask() {
	return min(1.0, (1.0 - abs(ftcoord.x * 2.0-1.0)) * strokeMult) * min(1.0, ftcoord.y);
}

float sdroundrect(vec2 pt, vec2 ext, float rad) {
	vec2 ext2 = ext - vec2(rad, rad);
	vec2 d = abs(pt) - ext2;
	return min(max(d.x, d.y),0.0) + length(max(d, 0.0)) - rad;
}

void main(void) {
   vec4 result;
	float scissor = scissorMask(fpos);
	float strokeAlpha = strokeMask();
	// Gradient
	if (type == 0)
	{
		// Calculate gradient color using box gradient
		vec2 pt = (paintMat * vec3(fpos, 1.0)).xy;
		// vec2 pt = fpos;
		float d = clamp((sdroundrect(pt, extent, radius) + feather * 0.5) / feather, 0.0, 1.0);
		// float d = clamp((sdroundrect(pt, extent) + feather * 0.5) / feather, 0.0, 1.0);
		
		vec4 color = mix(innerCol, outerCol, d);
		// Combine alpha
		color *= strokeAlpha * scissor;
		result = color;
		// if(fpos.x > 510.0)
		// {
		// 	discard;
		// }
		// else if ( d > 0.8 )
		// {
		// 	discard;
		// }
		// if(fpos.y > 200.0)
		// {
		// 	result = vec4(1.0, 0.0, 0.0, 0.1);
		// }
	}
	// Image
	else if (type == 1) 
	{
		// Calculate color fron texture
		vec2 pt = (paintMat * vec3(fpos,1.0)).xy / extent;
		vec4 color = texture(tex, pt);
		if (texType == 1) color = vec4(color.xyz*color.w,color.w);
		if (texType == 2) color = vec4(color.x);
		// Apply color tint and alpha.
		color *= innerCol;
		// Combine alpha
		color *= strokeAlpha * scissor;
		result = color;
	}
	// Stencil fill
	else if (type == 2) 
	{
		result = innerCol; //vec4(1,1,1,1);
	}
	// Textured tris
	else if (type == 3) 
	{
		vec4 color = texture(tex, ftcoord);
		if (texType == 1) color = vec4(color.xyz*color.w,color.w);
		if (texType == 2) color = vec4(color.x);
		color *= scissor;
		result = color * innerCol;
	}
	if (strokeAlpha < strokeThr) discard;
	outColor = result;
}
