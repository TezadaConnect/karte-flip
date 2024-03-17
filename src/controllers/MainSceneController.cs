using System.Threading;
using System.Threading.Tasks;
using Godot;

public partial class MainSceneController : Node {
	// Managers
	private RouteManager mRouteManager;
	private GameTurnManager mGameTurnManager;
	private TokenFlipManager mTokenFlipManager;
	private ScoringManager mScoringManager;
	private AIManager mIAManager;

	// UI NODE
	private GridGroundTilemap mTileMap;
	private TextureRect mHUDTextureRect;
	
	// Dialogues
	private TextureRect mDialogTextureRect;

	public override void _Ready(){
		InitSceneManagers();
		InitUiBindings();
		InitializeListeners();
		DisplayCardColorForTheTurn();
	}

    public override void _Input(InputEvent @event){
		if(!mGameTurnManager.GetIsPlayerTurn()){
			return;
		}

		if(@event is InputEventMouseButton){
			OnTappedGroundTile(@event);
		}
    }

	private void InitSceneManagers(){
		mRouteManager = RouteManager.GetIntance();
		mGameTurnManager = GameTurnManager.GetInstance();
		mTokenFlipManager = TokenFlipManager.GetInstance();
		mScoringManager = ScoringManager.GetInstance();
		mIAManager = AIManager.GetInstance();
	}

	private void InitUiBindings(){
		// Nodes
		mTileMap = GetNode<GridGroundTilemap>("GridGroundTilemap");
		mHUDTextureRect = GetNode<TextureRect>("HUDTextureRect");
		// Dialogues
		mDialogTextureRect = GetNode<TextureRect>("DialogueBackgroundTextureRect");
	}

	private void InitializeListeners(){
		//Button Listeners
		GetNode<Button>("QuitButton").Connect("pressed", new Callable(this, "OnPressedQuitButton"));
		GetNode<Button>("RestartButton").Connect("pressed", new Callable(this, "OnPressedRestartButton"));
		mDialogTextureRect.GetNode<Button>("NewGameButton").Connect("pressed", new Callable(this, "OnPressedNewGameButton"));
	}

	private async void OnTappedGroundTile(InputEvent @event){
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
		DisplayScore();
		DisplayCardColorForTheTurn();

		// Start of AI
		mGameTurnManager.SetIsPlayerTurn(false);
		await Task.Delay(3000);

		mIAManager.GetAITileMove(mTileMap, mGameTurnManager.GetCurrentCard());

		SetNextTurnPLayer();
		DisplayScore();
		DisplayCardColorForTheTurn();
		mGameTurnManager.SetIsPlayerTurn(true);
	}

	private void SetNextTurnPLayer(){
		TextureRect turnTextureRect = mHUDTextureRect.GetNode<TextureRect>("TurnTextureRect");
		if(mGameTurnManager.GetTurnType() == GameTurnEnum.LIGHT_TURN){
			mGameTurnManager.SetTurnType(GameTurnEnum.DARK_TURN);
			turnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
		} else {
			mGameTurnManager.SetTurnType(GameTurnEnum.LIGHT_TURN);
			turnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
		}
		DisplayCardColorForTheTurn();
	}

	private void DisplayCardColorForTheTurn(){
		Label randomCardDescription = mHUDTextureRect.GetNode<Label>("RandomCardDescription");
		Label randomCardName = mHUDTextureRect.GetNode<Label>("RandomCardName");
		mGameTurnManager.SetCurrentCardWithRandomCard();
		TextureRect randomCardTexture = GetNode<TextureRect>("HUDTextureRect").GetNode<TextureRect>("RandomCardTextureRect");
		randomCardTexture.Texture = mRouteManager.GetLocalAssetInTexture2D(
			mGameTurnManager.GetCurrentCard().GetCardFileNameEnum()
		);
		randomCardName.Text = mGameTurnManager.GetCurrentCard().GetCardName();
		randomCardDescription.Text = mGameTurnManager.GetCurrentCard().GetCardDiscription();
	}

	private void DisplayScore(){
		mScoringManager.CalculateScore(mTileMap);
		Label blackScoreNodeHolder = mHUDTextureRect.GetNode<Label>("BlackScoreLabel"); 
		Label whiteScoreNodeHolder = mHUDTextureRect.GetNode<Label>("WhiteScoreLabel"); 
		blackScoreNodeHolder.Text = mScoringManager.GetBlackScore() + "x";
		whiteScoreNodeHolder.Text = mScoringManager.GetWhiteScore() + "X";
	}

	private void OnPressedQuitButton(){
		mScoringManager.ResetScore();
		mRouteManager.MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}

	private void OnPressedNewGameButton(){
		mScoringManager.ResetScore();
		DisplayScore();
		mRouteManager.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

	private void OnPressedRestartButton(){
		mDialogTextureRect.Show();
	}
}
