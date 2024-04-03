using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace KarteFlipClient{
	public partial class TurnRpcService : Node{
		// Player Information
		private PlayerManager _playerManager;
		private RouteManager _routeManager;
		private ScoringManager _scoringManager;
		private GridGroundTilemap _mainTilemap;
		private TextureRect _turnDisplayTextureRect;
		private DisplayDialog _resultDialog;

        public override void _Ready(){
            _routeManager = GetNode<RouteManager>(
				RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
			);
			_scoringManager = GetNode<ScoringManager>(
				RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.SCORING_MANAGER)
			);
			_playerManager = GetNode<PlayerManager>(
				RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.PLAYER_MANAGER)
			);
			_resultDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
        }

        /*
		* ********************************************************
		*	SETTERS AND GETTERS
		* ********************************************************
		*/

		// CODE HERE...
		
		/*
		* ********************************************************
		*	RPC EXECUTER FUNCTIONS
		* ********************************************************
		*/
		public void ExecuteQuitMatch(){
			Multiplayer.MultiplayerPeer.Close();
			Multiplayer.MultiplayerPeer = null;
		}

		public void ExecuteAddTokenToTilemap(Vector2I position, CardModel card){
			Dictionary serializeValue = card.Serialize();
			RpcId(1, nameof(ProcessTurnDataOnServer), position, serializeValue);
		}

		/*
		* ********************************************************
		*	RPC FUNCTIONS
		* ********************************************************
		*/
		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void StartMatch(Array<Dictionary> players){ // client function
			foreach(Dictionary element in players.ToList()){
				PlayerModel playerInfoHolder = PlayerModel.Deserialize(element);
				if(playerInfoHolder.TokenColor == TokenColorEnum.LIGHT_TOKEN){
					_playerManager.CurrentPlayerTurn = playerInfoHolder;
				}
				if(playerInfoHolder.PlayerID == Multiplayer.GetUniqueId()){
					_playerManager.PlayerOne = playerInfoHolder;
					continue;
				}
				_playerManager.PlayerTwo = playerInfoHolder;
			}
			_routeManager.MoveToScene(SceneFilenameEnum.MAIN_VS_PLAYER_SCENE, "Starting match, please wait.");
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void RecieveDataToSpecificClient(Vector2I position, Dictionary card){ // client function
			AddTokenToTilemap(position, card);	
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void DisconnectClient(){ // client function
			ExecuteQuitMatch();
			_routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE, "Your opponent leave the match.");
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void ProcessTurnDataOnServer(Vector2I position, Dictionary card){} // server function

		/*
		* ********************************************************
		*	LOGICS
		* ********************************************************
		*/
		private void AddTokenToTilemap(Vector2I position, Dictionary card){
			_mainTilemap ??= GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
			_turnDisplayTextureRect ??= GetNode<TextureRect>("/root/MainScene/HUDTextureRect/TurnTextureRect");
			
			CardModel cardModel = CardModel.Deserialize(card);
			
			TileHelper.AddAtlasToken(
				position, 
				_mainTilemap, 
				_playerManager.CurrentPlayerTurn.TokenColor
			);
			TokenFlipService.FlipTokens(
				position, 
				_mainTilemap, 
				cardModel.CardListFlipDirections , 
				_playerManager.CurrentPlayerTurn.TokenColor
			);
			_playerManager.CurrentPlayerTurn = _playerManager.CurrentPlayerTurn.Equals(
				_playerManager.PlayerOne
			) ? _playerManager.PlayerTwo : _playerManager.PlayerOne;

			if(_playerManager.CurrentPlayerTurn.TokenColor == TokenColorEnum.DARK_TOKEN){
				_turnDisplayTextureRect.Texture = RouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
			} else {
				_turnDisplayTextureRect.Texture = RouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
			}

			_mainTilemap.PlayTileDropAudio();
			_scoringManager.DisplayScore(_mainTilemap);
			EndGameResult();
		}

		public bool IsMyTurn(){
			if(_playerManager.CurrentPlayerTurn.PlayerID == Multiplayer.GetUniqueId()){
				return true;
			}
			return false;
		}

		private void EndGameResult(){
        // GridGroundTilemap _mainTilemap = GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap"); ensurr this is not empty
			if(!_mainTilemap.IsMaxTiles()){
				return;
			}
			
			bool isblackWin = _scoringManager.BlackScore > _scoringManager.WhiteScore;

			if(isblackWin){
				ShowResultModal(TokenColorEnum.DARK_TOKEN);
				return;
			}
			
			ShowResultModal(TokenColorEnum.LIGHT_TOKEN); 
		}

		private void ShowResultModal(TokenColorEnum winningColor){ // Todo make this more faster
			MainVsPlayerSceneController mainScene = GetNode<MainVsPlayerSceneController>("/root/MainScene");
			mainScene.AddChild(_resultDialog);
			_resultDialog.SetDialogType(DialogType.ONE_BUTTON);
			_resultDialog.GetConfirmButton().Text = "Ok";
			_resultDialog.GetConfirmButton().Pressed += async () => {
				_resultDialog.CloseDialog();
				await Task.Delay(2000);
				_resultDialog.Free();
				_resultDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
			};

			PlayerModel playerWinner = null;
			
			if(_playerManager.PlayerOne.TokenColor == winningColor){
				playerWinner = _playerManager.PlayerOne;
			}

			if(_playerManager.PlayerTwo.TokenColor == winningColor){
				playerWinner = _playerManager.PlayerTwo;
			}

			bool isMeTheWinner = playerWinner.PlayerID == Multiplayer.MultiplayerPeer.GetUniqueId();

			if(isMeTheWinner){
				_resultDialog.ShowDialog("You Win!");
				_resultDialog.SetDialogType(DialogType.ONE_BUTTON);
				_resultDialog.PlayWinSoundEffect();
				return;
			}
					
			_resultDialog.ShowDialog("You Lose!"); 
			_resultDialog.PlayLoseSoundEffect();
		}
	}
}