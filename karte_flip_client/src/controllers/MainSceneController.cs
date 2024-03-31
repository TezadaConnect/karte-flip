using System.Threading;
using System.Threading.Tasks;
using Godot;
using KarteFlipClient;

public partial class MainSceneController : Node {
	// Managers
	private RouteManager _routeManager;
	// private GameTurnManager mGameTurnManager;
	// private ScoringManager mScoringManager;
	// private PlayerManager mPlayerManager;
	//Rpc
	private TurnRpcService _turnRpcService;
	// UI NODE
	private GridGroundTilemap mTileMap;
	private TextureRect _hudTextureRect;
	// Dialogues
	private TextureRect mDialogTextureRect;
	// Data Models
	private CardModel _randomCard;
	public override void _Ready(){
		InitSceneManagers();
		InitUiBindings();
		InitializeListeners();
		_turnRpcService = GetNode<TurnRpcService>(_routeManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.TURN_RPC_SERVICE));
		DisplayRandomCard();
		// ComputerTapGroundTile(); // Only execute when computer is first turn 
	}

    public override void _Input(InputEvent @event){
		// if(IsPlayerAComputer()){
		// 	return;
		// }

		// if(IsMaxTiles()){
		// 	return;
		// }

		if(@event is InputEventMouseButton){

			if(_turnRpcService.IsMyTurn()){
				OnTapGroundTile(@event);
			}
			// ComputerTapGroundTile();
		}
    }

	private void InitSceneManagers(){
		_routeManager = RouteManager.GetIntance();
		// mGameTurnManager = GameTurnManager.GetInstance();
		// mScoringManager = ScoringManager.GetInstance();
		// mPlayerManager = PlayerManager.GetInstance();
	}

	private void InitUiBindings(){
		// Nodes
		mTileMap = GetNode<GridGroundTilemap>("GridGroundTilemap");
		_hudTextureRect = GetNode<TextureRect>("HUDTextureRect");
		// Dialogues
		mDialogTextureRect = GetNode<TextureRect>("DialogueBackgroundTextureRect");
	}

	private void InitializeListeners(){
		//Button Listeners
		// GetNode<Button>("QuitButton").Pressed += OnPressedQuitButton;
		// GetNode<Button>("RestartButton").Pressed += OnPressedRestartButton;
		// mDialogTextureRect.GetNode<Button>("NewGameButton").Pressed += OnPressedNewGameButton;
		// mDialogTextureRect.GetNode<Button>("CloseDialogButton").Pressed += OnPressedCloseDialogButton;
	}

	private void OnTapGroundTile(InputEvent @event){

		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

		if(!(mouseEvent.ButtonIndex == MouseButton.Left && @event.IsReleased())){
			return;
		}

		// if(IsOpenDialogue()){ // Halt click event
		// 	return;
		// }

		Vector2I tilePostion = mTileMap.LocalToMap(mTileMap.GetLocalMousePosition());
		TileData groundTileData = mTileMap.GetCellTileData(mTileMap.GROUND_LAYER, tilePostion);
		
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

		_turnRpcService.ExecuteAddTokenToTilemap(tilePostion, _randomCard);
		mTileMap.PlayTileDropAudio();;
		DisplayRandomCard();
		// if(IsMaxTiles()){
		// 	EndGameResult();
		// 	return;
		// }
		
		// DisplayScore();	
		
		// mGameTurnManager.SetPlayerTurn(mPlayerManager.GetPlayerTwo());
	}

	// private async void ComputerTapGroundTile(){
		// if(!IsPlayerAComputer()){
		// 	return;
		// }

		// if(IsMaxTiles()){
		// 	return;
		// }
		
		// await Task.Delay(500);

		// ComputerOpponentService.GetComputerTileMove(mTileMap, mGameTurnManager.GetCurrentCard());

		// mTileMap.PlayTileDropAudio();

		// if(IsMaxTiles()){
		// 	EndGameResult();
		// 	return;
		// }

		// SetNextTurnPLayer();
		// DisplayScore();
		// mGameTurnManager.SetPlayerTurn(mPlayerManager.GetPlayerOne());
	// }

	private void DisplayRandomCard(){
		SetRandomCard();
		Label randomCardDescription = _hudTextureRect.GetNode<Label>("RandomCardDescription");
		Label randomCardName = _hudTextureRect.GetNode<Label>("RandomCardName");
		TextureRect randomCardTexture = GetNode<TextureRect>("HUDTextureRect").GetNode<TextureRect>("RandomCardTextureRect");
		randomCardTexture.Texture = _routeManager.GetLocalAssetInTexture2D(
			_randomCard.CardFilename
		);
		randomCardName.Text = _randomCard.CardName;
		randomCardDescription.Text = _randomCard.CardDescription;
	}

	// private void DisplayScore(){
	// 	mScoringManager.CalculateScore(mTileMap);
	// 	Label blackScoreNodeHolder = mHUDTextureRect.GetNode<Label>("BlackScoreLabel"); 
	// 	Label whiteScoreNodeHolder = mHUDTextureRect.GetNode<Label>("WhiteScoreLabel"); 
	// 	blackScoreNodeHolder.Text = mScoringManager.GetBlackScore() + "x";
	// 	whiteScoreNodeHolder.Text = mScoringManager.GetWhiteScore() + "X";
	// }

	// private async void OnPressedQuitButton(){
	// 	ResetScoringAndTurnAndPlayerManagers();
	// 	await Task.Delay(500);
	// 	mRouteManager.MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	// }

	// private void OnPressedNewGameButton(){
	// 	mDialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").PlayBackwards("Intro");
	// 	mTileMap.ClearLayer(mTileMap.TOKEN_PLACEMENT_LAYER);
	// 	ResetScoringAndTurnAndPlayerManagers();
	// 	DisplayScore();
	// 	DisplayCardColorForTheTurn();
	// 	ComputerTapGroundTile();
	// 	// mRouteManager.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	// }

	// private void OnPressedCloseDialogButton(){
	// 	mDialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").PlayBackwards("Intro");
	// }

	// private void ResetScoringAndTurnAndPlayerManagers(){
	// 	mScoringManager.ResetScore();
	// 	mGameTurnManager.ResetTurn();
	// 	mPlayerManager.ResetPlayers();
	// 	mTurnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
	// }

	// private void OnPressedRestartButton(){
	// 	ShowDialog("Are you sure you want to restart the game?");
	// }

	// private void ShowDialog(string message){
	// 	mDialogTextureRect.GetNode<Label>("MessageLabel").Text = message;
	// 	mDialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").Play("Intro");
	// }

	// private void EndGameResult(){
	// 	DisplayScore();
	// 	TokenColorEnum winnerColorToken = TokenColorEnum.NO_TOKEN;
	// 	AudioStreamPlayer loseAudio = mDialogTextureRect.GetNode<AudioStreamPlayer>("LoseAudioStreamPlayer");
	// 	string message = "";

	// 	if(mScoringManager.GetBlackScore() > mScoringManager.GetWhiteScore()){
	// 		winnerColorToken = TokenColorEnum.DARK_TOKEN;
	// 	}

	// 	if(mScoringManager.GetWhiteScore() > mScoringManager.GetBlackScore()){
	// 		winnerColorToken = TokenColorEnum.LIGHT_TOKEN;
	// 	}

	// 	if(mPlayerManager.GetPlayerOne().GetTokenColorType() == winnerColorToken){
	// 		mDialogTextureRect.GetNode<AudioStreamPlayer>("WinAudioStreamPlayer").Play();
	// 		message = "You win!";
	// 	}

	// 	if(mPlayerManager.GetPlayerOne().GetTokenColorType() != winnerColorToken){
	// 		loseAudio.Play();
	// 		message = "You lose!";
	// 	}

	// 	if(winnerColorToken != TokenColorEnum.DARK_TOKEN && winnerColorToken != TokenColorEnum.LIGHT_TOKEN){
	// 		loseAudio.Play();
	// 		message = "Draw";
	// 	}

	// 	ShowDialog(message);
	// }

	// public bool IsMaxTiles(){
	// 	List<Vector2I> allTileMapVector = mTileMap.GetUsedCells(mTileMap.TOKEN_PLACEMENT_LAYER).ToList();
	// 	return allTileMapVector.Count >= GridGroundTilemap.BOARD_TILE_COUNT;
	// }

	// public bool IsOpenDialogue(){
	// 	return mDialogTextureRect.Scale.X == 1 && mDialogTextureRect.Scale.Y == 1;
	// }

	// public bool IsPlayerAComputer(){
	// 	return mGameTurnManager.GetPlayerTurn().GetPlayerType() == PlayerTypeEnum.COMPUTER;
	// }

	private void SetRandomCard(){
		_randomCard = TileHelper.GetRandomCard();
	}
}
