using Godot;

public partial class MainSceneController : Node {
	private RouteManager mRouteManager;
	private GameTurnManager mGameTurnManager;
	private TokenFlipManager mTokenFlipManager;

	// UI NODE
	private GridGroundCustomTilemap mTileMap;
	private Button mQuitButton;
	private TextureRect mHUDTextureRect;
	private Label mRandomCardName;
	private Label mRandomCardDescription;
	private TextureRect mTurnTextureRect;
	private TextureRect mDialogTextureRect;
	private Button mNewGameButton;
	private Button mRestartButton;

	public override void _Ready(){
		// Init managers
		mRouteManager = RouteManager.GetIntance();
		mGameTurnManager = GameTurnManager.GetInstance();
		mTokenFlipManager = TokenFlipManager.GetInsntance();

		// UI BINDINGS
		mTileMap = GetNode<GridGroundCustomTilemap>("GridGroundTilemap");
		mHUDTextureRect = GetNode<TextureRect>("HUDTextureRect");
		mRandomCardDescription = mHUDTextureRect.GetNode<Label>("RandomCardDescription");
		mRandomCardName = mHUDTextureRect.GetNode<Label>("RandomCardName");
		mTurnTextureRect = mHUDTextureRect.GetNode<TextureRect>("TurnTextureRect");
		mQuitButton = GetNode<Button>("QuitButton");
		mDialogTextureRect = GetNode<TextureRect>("DialogueBackgroundTextureRect");
		mNewGameButton = mDialogTextureRect.GetNode<Button>("NewGameButton");
		mRestartButton = GetNode<Button>("RestartButton");
		

		// Set onpressed listener
		mQuitButton.Connect("pressed", new Callable(this, "OnPressedQuiteButton"));
		mNewGameButton.Connect("pressed", new Callable(this, "OnPressedNewGameButton"));
		mRestartButton.Connect("pressed", new Callable(this, "OnPressedRestartButton"));

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
			mTurnTextureRect.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.BLACK_TOKEN));
		} else {
			mGameTurnManager.SetTurnType(GameTurnEnum.LIGHT_TURN);
			mTurnTextureRect.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.WHITE_TOKEN));
		}
		DisplayCardColorForTheTurn();
	}

	private void DisplayCardColorForTheTurn(){
		mGameTurnManager.SetCurrentCardWithRandomCard();
		TextureRect randomCardTexture = GetNode<TextureRect>("HUDTextureRect").GetNode<TextureRect>("RandomCardTextureRect");
		
		if(mGameTurnManager.GetCurrentCard().GetCardFileNameEnum() == LocalAssetFileNameEnum.GREEN_CARD){
			randomCardTexture.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.GREEN_CARD));
		}
		if(mGameTurnManager.GetCurrentCard().GetCardFileNameEnum() == LocalAssetFileNameEnum.BLUE_CARD){
			randomCardTexture.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.BLUE_CARD));
		}
		if(mGameTurnManager.GetCurrentCard().GetCardFileNameEnum() == LocalAssetFileNameEnum.RED_CARD){
			randomCardTexture.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.RED_CARD));
		}
		if(mGameTurnManager.GetCurrentCard().GetCardFileNameEnum() == LocalAssetFileNameEnum.ORANGE_CARD){
			randomCardTexture.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.ORANGE_CARD));
		}
		if(mGameTurnManager.GetCurrentCard().GetCardFileNameEnum() == LocalAssetFileNameEnum.YELLOW_CARD){
			randomCardTexture.Texture = GD.Load<Texture2D>(mRouteManager.GetLocalAssetFilePath(LocalAssetFileNameEnum.YELLOW_CARD));
		}
		
		mRandomCardName.Text = mGameTurnManager.GetCurrentCard().GetCardName();
		mRandomCardDescription.Text = mGameTurnManager.GetCurrentCard().GetCardDiscription();
	}

	private void OnPressedQuiteButton(){
		GetTree().ChangeSceneToFile(mRouteManager.GetSceneFilePath(SceneFileNameEnum.LOBBY_SCENE));
	}

	private void OnPressedNewGameButton(){
		GetTree().ChangeSceneToFile(mRouteManager.GetSceneFilePath(SceneFileNameEnum.MAIN_SCENE));
	}

	private void OnPressedRestartButton(){
		mDialogTextureRect.Show();
	}
}
