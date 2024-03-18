using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class MainSceneController : Node {
	// Managers
	private RouteManager mRouteManager;
	private GameTurnManager mGameTurnManager;
	private TokenFlipManager mTokenFlipManager;
	private ScoringManager mScoringManager;
	private AIManager mIAManager;
	private PlayerManager mPlayerManager;
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
		ComputerTapGroundTile(); // Only execute when computer is first turn 
	}

    public override void _Input(InputEvent @event){
		if(mGameTurnManager.GetPlayerTurn().GetPlayerType() == PlayerTypeEnum.COMPUTER){
			return;
		}

		if(@event is InputEventMouseButton){
			OnTapGroundTile(@event);
			ComputerTapGroundTile();
			EndGameResult();
		}
    }

	private void InitSceneManagers(){
		mRouteManager = RouteManager.GetIntance();
		mGameTurnManager = GameTurnManager.GetInstance();
		mTokenFlipManager = TokenFlipManager.GetInstance();
		mScoringManager = ScoringManager.GetInstance();
		mIAManager = AIManager.GetInstance();
		mPlayerManager = PlayerManager.GetInstance();
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
		mDialogTextureRect.GetNode<Button>("CloseDialogButton").Connect("pressed", new Callable(this, "OnPressedCloseDialogButton"));
	}

	private void OnTapGroundTile(InputEvent @event){

		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

		if(!(mouseEvent.ButtonIndex == MouseButton.Left && @event.IsReleased())){
			return;
		}

		if(mDialogTextureRect.Scale.X == 1 && mDialogTextureRect.Scale.Y == 1){ // Cancel click event
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
		
		mGameTurnManager.SetPlayerTurn(mPlayerManager.GetPlayerTwo());
	}

	private async void ComputerTapGroundTile(){
		if(mGameTurnManager.GetPlayerTurn().GetPlayerType() != PlayerTypeEnum.COMPUTER){
			return;
		}

		List<Vector2I> vectorHolderForTokenLayer = mTileMap.GetUsedCells(mTileMap.TOKEN_PLACEMENT_LAYER).ToList();

		if(vectorHolderForTokenLayer.Count >= GridGroundTilemap.BOARD_TILE_COUNT){
			return;
		}
		
		await Task.Delay(3000);

		mIAManager.GetAITileMove(mTileMap, mGameTurnManager.GetCurrentCard());

		SetNextTurnPLayer();
		DisplayScore();
		mGameTurnManager.SetPlayerTurn(mPlayerManager.GetPlayerOne());
	}

	private void SetNextTurnPLayer(){
		TextureRect turnTextureRect = mHUDTextureRect.GetNode<TextureRect>("TurnTextureRect");
		
		if(mGameTurnManager.GetPlayerTurn().IsLightToken()){
			turnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
		} else {
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
		ResetScoringAndTurnAndPlayerManagers();
		mRouteManager.MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}

	private void OnPressedNewGameButton(){
		mDialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").PlayBackwards("Intro");
		mTileMap.ClearLayer(mTileMap.TOKEN_PLACEMENT_LAYER);
		ResetScoringAndTurnAndPlayerManagers();
		DisplayScore();
		DisplayCardColorForTheTurn();
		ComputerTapGroundTile();
		// mRouteManager.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

	private void OnPressedCloseDialogButton(){
		mDialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").PlayBackwards("Intro");
	}

	private void ResetScoringAndTurnAndPlayerManagers(){
		mScoringManager.ResetScore();
		mGameTurnManager.ResetTurn();
		mPlayerManager.ResetPlayers();
	}

	private void OnPressedRestartButton(){
		ShowDialog("Are you sure you want to restart the game?");
	}

	private void ShowDialog(string message){
		mDialogTextureRect.GetNode<Label>("MessageLabel").Text = message;
		mDialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").Play("Intro");
	}

	private void EndGameResult(){
		List<Vector2I> allTileMapVector = mTileMap.GetUsedCells(mTileMap.TOKEN_PLACEMENT_LAYER).ToList();

		if(allTileMapVector.Count < GridGroundTilemap.BOARD_TILE_COUNT){
			return;
		}

		TokenColorEnum winnerColorToken = TokenColorEnum.NO_TOKEN;

		if(mScoringManager.GetBlackScore() > mScoringManager.GetWhiteScore()){
			winnerColorToken = TokenColorEnum.DARK_TOKEN;
		}

		if(mScoringManager.GetWhiteScore() > mScoringManager.GetBlackScore()){
			winnerColorToken = TokenColorEnum.LIGHT_TOKEN;
		}

		string message = "You Lose";

		if(mPlayerManager.GetPlayerOne().GetTokenColorType() == winnerColorToken){
			message = "You win!";
		}

		if(winnerColorToken != TokenColorEnum.DARK_TOKEN && winnerColorToken != TokenColorEnum.LIGHT_TOKEN){
			message = "Draw";
		}

		ShowDialog(message);
	}
}
