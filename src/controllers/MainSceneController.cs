using Godot;

public partial class MainSceneController : Node {
	private RouteManager mRouteManager;
	private GameTurnManager mGameTurnManager;
	private TokenFlipManager mTokenFlipManager;

	private GridGroundCustomTilemap mTileMap;

	
	public override void _Ready(){
		mRouteManager = RouteManager.GetIntance();
		mGameTurnManager = GameTurnManager.GetInstance();
		mTokenFlipManager = TokenFlipManager.GetInsntance();
		mTileMap = GetNode<GridGroundCustomTilemap>("GridGround");
		// FlipTokens();
	}

	// Input Event Listener
    public override void _Input(InputEvent @event){
		if(@event is InputEventMouseButton){
			OnTappedGroundTile(@event);
		}
		
    }

	private void OnTappedGroundTile(InputEvent @event){
		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

		if(!(mouseEvent.ButtonIndex == MouseButton.Left && @event.IsPressed())){
			return;
		}
            
		Vector2 mousePosition = mouseEvent.Position;
		Vector2I tilePostion = mTileMap.LocalToMap(mousePosition);
		TileData groundTileData = mTileMap.GetCellTileData(mTileMap.GROUND_LAYER, tilePostion);
		Vector2I tileToAdd = mGameTurnManager.GetTileForDisplay(); // Location of the white token in the tile asset
	
		if(groundTileData == null){
			return;
		} 

		bool isPlaceable = (bool)groundTileData.GetCustomData(mTileMap.IS_PLACEABLE_CUSTOM_DATA);

		if(!isPlaceable){
			return;
		}
			
		TileData tokenPlacementTilemap = mTileMap.GetCellTileData(
			mTileMap.TOKEN_PLACEMENT_LAYER, 
			tilePostion
		);

		// GD.Print(groundTileData.GetPropertyList());

		if(tokenPlacementTilemap != null){
			return;
		}

		mTileMap.SetCell(
			mTileMap.TOKEN_PLACEMENT_LAYER, 
			tilePostion, 
			0, 			
			tileToAdd
		);

		mTokenFlipManager.FlipTokens(
			tilePostion, 
			mTileMap, 
			mGameTurnManager.GetRandomCard().GetCardListFlipDirections()
		);
	
		SetNextTurnPLayer();
	}

	private void SetNextTurnPLayer(){
		if(mGameTurnManager.GetTurnType() == GameTurnEnum.LIGHT_TURN){
			mGameTurnManager.SetTurnType(GameTurnEnum.DARK_TURN);
		} else {
			mGameTurnManager.SetTurnType(GameTurnEnum.LIGHT_TURN);
		}	
	}

	

}
