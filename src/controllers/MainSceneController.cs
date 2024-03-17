using Godot;

public partial class MainSceneController : Node {
	// Managers
	private RouteManager mRouteManager;
	private GameTurnManager mGameTurnManager;
	private TokenFlipManager mTokenFlipManager;

	// UI NODE
	private GridGroundTilemap mTileMap;
	private TextureRect mHUDTextureRect;
	private Label mRandomCardName;
	private Label mRandomCardDescription;
	private TextureRect mTurnTextureRect;
	
	// Dialogues
	private TextureRect mDialogTextureRect;
	
	// Buttons
	private Button mQuitButton;
	private Button mNewGameButton;
	private Button mRestartButton;

	public override void _Ready(){
		InitSceneManagers();
		InitUiBindings();
		InitializeListeners();
		DisplayCardColorForTheTurn();
	}

    public override void _Input(InputEvent @event){
		if(@event is InputEventMouseButton){
			OnTappedGroundTile(@event);
		}
    }

	private void InitSceneManagers(){
		mRouteManager = RouteManager.GetIntance();
		mGameTurnManager = GameTurnManager.GetInstance();
		mTokenFlipManager = TokenFlipManager.GetInsntance();
	}

	private void InitUiBindings(){
		// Nodes
		mTileMap = GetNode<GridGroundTilemap>("GridGroundTilemap");
		mHUDTextureRect = GetNode<TextureRect>("HUDTextureRect");
		mRandomCardDescription = mHUDTextureRect.GetNode<Label>("RandomCardDescription");
		mRandomCardName = mHUDTextureRect.GetNode<Label>("RandomCardName");
		mTurnTextureRect = mHUDTextureRect.GetNode<TextureRect>("TurnTextureRect");
		
		// Dialogues
		mDialogTextureRect = GetNode<TextureRect>("DialogueBackgroundTextureRect");
		// Buttons
		mNewGameButton = mDialogTextureRect.GetNode<Button>("NewGameButton");
		mQuitButton = GetNode<Button>("QuitButton");
		mRestartButton = GetNode<Button>("RestartButton");
	}

	private void InitializeListeners(){
		mQuitButton.Connect("pressed", new Callable(this, "OnPressedQuiteButton"));
		mNewGameButton.Connect("pressed", new Callable(this, "OnPressedNewGameButton"));
		mRestartButton.Connect("pressed", new Callable(this, "OnPressedRestartButton"));
	}


	private void OnTappedGroundTile(InputEvent @event){
		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

		if(!(mouseEvent.ButtonIndex == MouseButton.Left && @event.IsPressed())){
			return;
		}

		if(mDialogTextureRect.IsVisibleInTree() == true){
			return;
		}

		Vector2I tilePostion = mTileMap.LocalToMap(mTileMap.GetLocalMousePosition());
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

		mTileMap.SetCell( // Add tile
			mTileMap.TOKEN_PLACEMENT_LAYER, 
			tilePostion, 
			mTileMap.ADD_TILE_ACTION, 			
			tileImageCoordinate
		);

		CardModel cardDisplay = mGameTurnManager.GetCurrentCard();

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
			mTurnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
		} else {
			mGameTurnManager.SetTurnType(GameTurnEnum.LIGHT_TURN);
			mTurnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
		}
		DisplayCardColorForTheTurn();
	}

	private void DisplayCardColorForTheTurn(){
		mGameTurnManager.SetCurrentCardWithRandomCard();
		TextureRect randomCardTexture = GetNode<TextureRect>("HUDTextureRect").GetNode<TextureRect>("RandomCardTextureRect");
		randomCardTexture.Texture = mRouteManager.GetLocalAssetInTexture2D(
			mGameTurnManager.GetCurrentCard().GetCardFileNameEnum()
		);
		mRandomCardName.Text = mGameTurnManager.GetCurrentCard().GetCardName();
		mRandomCardDescription.Text = mGameTurnManager.GetCurrentCard().GetCardDiscription();
	}

	private void OnPressedQuiteButton(){
		mRouteManager.MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}

	private void OnPressedNewGameButton(){
		mRouteManager.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

	private void OnPressedRestartButton(){
		mDialogTextureRect.Show();
	}
}
