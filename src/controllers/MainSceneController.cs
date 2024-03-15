using Godot;

public partial class MainSceneController : Node {
	private RouteManager mRouteManager;
	private GameTurnManager mGameTurnManager;
	private TokenFlipManager mTokenFlipManager;

	// UI NODE
	private GridGroundCustomTilemap mTileMap;
	private Label mCardNameLabel;
	private Label mCardDescriptionLabel;

	public override void _Ready(){
		mRouteManager = RouteManager.GetIntance();
		mGameTurnManager = GameTurnManager.GetInstance();
		mTokenFlipManager = TokenFlipManager.GetInsntance();
		mTileMap = GetNode<GridGroundCustomTilemap>("GridGroundTilemap");
		mCardNameLabel = GetNode<Label>("CardNameLabel");
		mCardDescriptionLabel = GetNode<Label>("CardDescriptionLabel");
		DisplayCardColorForTheTurn();
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
		Vector2I tileImageCoordinate = mGameTurnManager.GetTileForDisplay(); // Location of the white token in the tile asset
	
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

		if(tokenPlacementTilemap != null){
			return;
		}

		CardModel cardDisplay = mGameTurnManager.GetCurrentCard();

		mTileMap.SetCell( // Add tile
			mTileMap.TOKEN_PLACEMENT_LAYER, 
			tilePostion, 
			mTileMap.ADD_TILE_ACTION, 			
			tileImageCoordinate
		);

		mTokenFlipManager.FlipTokens( // Flip nearby tokens
			tilePostion, 
			mTileMap, 
			cardDisplay.GetCardListFlipDirections()
		);
	
		SetNextTurnPLayer();
		DisplayCardColorForTheTurn();
	}

	private void SetNextTurnPLayer(){
		if(mGameTurnManager.GetTurnType() == GameTurnEnum.LIGHT_TURN){
			mGameTurnManager.SetTurnType(GameTurnEnum.DARK_TURN);
		} else {
			mGameTurnManager.SetTurnType(GameTurnEnum.LIGHT_TURN);
		}
		DisplayCardColorForTheTurn();
	}

	private void DisplayCardColorForTheTurn(){
		mGameTurnManager.SetCurrentCardWithRandomCard();
		Godot.Collections.Array<Vector2I> cells =  mTileMap.GetUsedCells(mTileMap.CARD_DISPLAY_LAYER);
		Vector2I vectorHolder = cells[0];
		
		mTileMap.SetCell(
			mTileMap.CARD_DISPLAY_LAYER, 
			vectorHolder,
			mTileMap.REMOVE_TILE_ACTION
		);
		
		mTileMap.SetCell(
			mTileMap.CARD_DISPLAY_LAYER, 
			vectorHolder,
			mTileMap.ADD_TILE_ACTION,
			mGameTurnManager.GetCurrentCard().GetCardTileImageCoordinate()
		);

		mCardNameLabel.Text = mGameTurnManager.GetCurrentCard().GetCardName();
		mCardDescriptionLabel.Text = mGameTurnManager.GetCurrentCard().GetCardDiscription();
	}
}
