shader_type canvas_item;

uniform float speed = 2f;
uniform float scale = .1f;

void vertex() {
	vec2 offset = cos(TIME/speed) * vec2(VERTEX.y, 0) * scale;
	
	VERTEX = VERTEX + offset;
}