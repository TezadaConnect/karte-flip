using Godot;
using System;

public partial class GridGroundCustomTilemap : TileMap{
	// LAYERS
	public readonly int GROUND_LAYER = 0;
	public readonly int TOKEN_PLACEMENT_LAYER = 1;
	public readonly int CARD_DISPLAY_LAYER = 2;
	// TILE ACTION
	public readonly int REMOVE_TILE_ACTION = -1;
	public readonly int ADD_TILE_ACTION = 0;
	// CUSTOM DATA
	
	public override void _Ready(){}

	public override void _Process(double delta){}
}
