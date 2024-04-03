using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class GridGroundTilemap : TileMap{
	// LAYERS
	public readonly int GROUND_LAYER = 0;
	public readonly int TOKEN_PLACEMENT_LAYER = 1;
	public readonly int CARD_DISPLAY_LAYER = 2;
	// TILE ACTION
	public readonly int REMOVE_TILE_ACTION = -1;
	public readonly int ADD_TILE_ACTION = 0;
	//Count of board Tile
	public const int BOARD_TILE_COUNT = 49;
	// CUSTOM DATA
	
	public override void _Ready(){}

	public override void _Process(double delta){}

	public void PlayTileDropAudio(){
		GetNode<AudioStreamPlayer>("TileDropAudioStreamPlayer").Play();
	}

	public bool IsMaxTiles(){
		List<Vector2I> allTileMapVector = GetUsedCells(TOKEN_PLACEMENT_LAYER).ToList();
		return allTileMapVector.Count >= BOARD_TILE_COUNT;
	}
}
