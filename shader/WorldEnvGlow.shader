shader_type canvas_item;
render_mode blend_mul;

uniform float glowStrength = 1f;

void vertex(){
	COLOR = COLOR * glowStrength;
}