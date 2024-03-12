using Godot;

public partial class MainSceneController : Node {
	private readonly string MAIN_SCENE_ROUTE = PathHelper.GetSceneFilePath("main_scene.tscn");

	private GridGroundCustomTilemap mTileMap;
	
	public override void _Ready(){
		mTileMap = GetNode<GridGroundCustomTilemap>("GridGround");
	}

	// Input Event Listener
    public override void _Input(InputEvent @event){
		OnTappedGroundTile(@event);
    }

	private void OnTappedGroundTile(InputEvent @event){
		if(
			@event is InputEventMouseButton mouseEvent &&
			mouseEvent.ButtonIndex == MouseButton.Left &&
			@event.IsPressed()
		){
			Vector2 mousePosition = mouseEvent.Position;
			Vector2I tilePostion = mTileMap.LocalToMap(mousePosition);
			TileData groundTileData = mTileMap.GetCellTileData(mTileMap.GROUND_LAYER, tilePostion);
			Vector2I tileToAdd = new Vector2I(1,0); // Location of the white token in the tile asset
		
			if(groundTileData != null){
				bool isPlaceable = (bool)groundTileData.GetCustomData(mTileMap.IS_PLACEABLE_CUSTOM_DATA);
				if(isPlaceable){
					TileData tokenPlacementTilemap = mTileMap.GetCellTileData(
						mTileMap.TOKEN_PLACEMENT_LAYER, 
						tilePostion
					);
					if(tokenPlacementTilemap == null){
						mTileMap.SetCell(
							mTileMap.TOKEN_PLACEMENT_LAYER, 
							tilePostion, 
							0, 
							tileToAdd
						);
					} 
				}	
			}
		}
	}
}
