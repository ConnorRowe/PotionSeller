shader_type canvas_item;

uniform float speed = .1f;
uniform float scale = .2f;

void vertex() {
	float adjustedTime = (fract(TIME * speed) * 6f) - 3f;
	vec2 offset = cos(adjustedTime) * vec2(VERTEX.y, 0) * scale;
	
	VERTEX = VERTEX - offset;
}