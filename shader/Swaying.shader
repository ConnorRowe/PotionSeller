shader_type canvas_item;

void vertex() {
	vec2 offset = cos(TIME/2f) * vec2(VERTEX.y, 0) * .1f;
	
	VERTEX = VERTEX + offset;
}