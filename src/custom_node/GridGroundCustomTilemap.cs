using Godot;
using System;

public partial class GridGroundCustomTilemap : TileMap{

	public readonly int GROUND_LAYER = 0;
	public readonly int TOKEN_PLACEMENT_LAYER = 1;
	public readonly string IS_PLACEABLE_CUSTOM_DATA = "isPlaceable";

	public override void _Ready(){}

	public override void _Process(double delta){}
}
