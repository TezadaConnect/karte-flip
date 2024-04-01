using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using KarteFlipClient;

public partial class MainSceneController : Node {
	// Managers
	private RouteManager _routeManager;
	
	//Rpc
	private TurnRpcService _turnRpcService;
	
	// UI NODE
	private GridGroundTilemap mTileMap;
	private TextureRect _hudTextureRect;
	
	// Dialogues
	private enum Dialogs { RESTART_DIALOGUE, QUIT_DIALOGUE }
	private Dialogs _currentDialogue;
	private DisplayDialog _displayDialog;

	// Data Models
	private CardModel _randomCard;

	public override void _Ready(){
		InitAutoLoads();
		InitUiBindings();
		InitializeListeners();
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

		if(!_displayDialog.IsDialogHidden()){
			return;
		}

		if(@event is InputEventMouseButton){

			if(_turnRpcService.IsMyTurn()){
				OnTapGroundTile(@event);
			}
			// ComputerTapGroundTile();
		}
    }

	private void InitAutoLoads(){
		_routeManager = GetNode<RouteManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
		);
		_turnRpcService = GetNode<TurnRpcService>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.TURN_RPC_SERVICE)
		);
		// mGameTurnManager = GameTurnManager.GetInstance();
		// mScoringManager = ScoringManager.GetInstance();
		// mPlayerManager = PlayerManager.GetInstance();
	}

	private void InitUiBindings(){
		// Nodes
		mTileMap = GetNode<GridGroundTilemap>("GridGroundTilemap");
		_hudTextureRect = GetNode<TextureRect>("HUDTextureRect");
		// Dialogues
		_displayDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
		AddChild(_displayDialog);
		_displayDialog.SetDialogType(DialogType.TWO_BUTTON);
		_displayDialog.GetConfirmButton().Pressed += OnPressedDialogConfirmButton;
		_displayDialog.GetCancelButton().Pressed += OnPressedDialogCancelButton;
	}

	private void InitializeListeners(){
		//Button Listeners
		GetNode<Button>("QuitButton").Pressed += OnPressedQuitButton;
		GetNode<Button>("RestartButton").Pressed += OnPressedRestartButton;
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

	// private void DisplayScore(){
	// 	mScoringManager.CalculateScore(mTileMap);
	// 	Label blackScoreNodeHolder = mHUDTextureRect.GetNode<Label>("BlackScoreLabel"); 
	// 	Label whiteScoreNodeHolder = mHUDTextureRect.GetNode<Label>("WhiteScoreLabel"); 
	// 	blackScoreNodeHolder.Text = mScoringManager.GetBlackScore() + "x";
	// 	whiteScoreNodeHolder.Text = mScoringManager.GetWhiteScore() + "X";
	// }

	private void DisplayRandomCard(){
		SetRandomCard();
		Label randomCardDescription = _hudTextureRect.GetNode<Label>("RandomCardDescription");
		Label randomCardName = _hudTextureRect.GetNode<Label>("RandomCardName");
		TextureRect randomCardTexture = GetNode<TextureRect>("HUDTextureRect").GetNode<TextureRect>("RandomCardTextureRect");
		randomCardTexture.Texture = RouteManager.GetLocalAssetInTexture2D(_randomCard.CardFilename);
		randomCardName.Text = _randomCard.CardName;
		randomCardDescription.Text = _randomCard.CardDescription;
	}

	private void OnPressedRestartButton(){
		_displayDialog.ShowDialog("Are you sure about restarting the game?");
		_currentDialogue = Dialogs.RESTART_DIALOGUE;
	}

	private void OnPressedQuitButton(){
		_displayDialog.ShowDialog("Are you sure about quiting game?");
		_currentDialogue = Dialogs.QUIT_DIALOGUE;
	}

	private async void OnPressedDialogConfirmButton(){
		if(Dialogs.RESTART_DIALOGUE == _currentDialogue){
			_displayDialog.CloseDialog();
		}

		if(Dialogs.QUIT_DIALOGUE == _currentDialogue){
			Multiplayer.MultiplayerPeer = null;
			await Task.Delay(500);
			_routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE, "Leaving game, please wait.");
		}
	}

	private void OnPressedDialogCancelButton(){
		_displayDialog.CloseDialog();
	}


	// private void ResetScoringAndTurnAndPlayerManagers(){
	// 	mScoringManager.ResetScore();
	// 	mGameTurnManager.ResetTurn();
	// 	mPlayerManager.ResetPlayers();
	// 	mTurnTextureRect.Texture = mRouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
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
