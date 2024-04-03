using System.Linq;
using Godot;
using Godot.Collections;

namespace KarteFlipClient{
	public partial class PlayerTurnManager : TurnManager {
		private RouteManager _routeManager;

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
			_routeManager.MoveToScene(SceneFilenameEnum.VS_PLAYER_MATCH_SCENE, "Starting match, please wait.");
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
			CardModel cardModel = CardModel.Deserialize(card);
			DrawTokens(position, cardModel.CardListFlipDirections);
			ShiftPlayer();
			_mainTilemap.PlayTileDropAudio();
			_scoringManager.DisplayScore(_mainTilemap);
			EndGameResult();
		}

		public override bool IsMyTurn(){
			if(_playerManager.CurrentPlayerTurn.PlayerID == Multiplayer.GetUniqueId()){
				return true;
			}
			return false;
		}

		protected override void MakeResultDialog(TokenColorEnum winningColor){ // Todo make this more faster
			VsPlayerMatchSceneController mainScene = GetNode<VsPlayerMatchSceneController>("/root/MainScene");
			InstantiateResultDialog(mainScene);
			PlayerModel playerWinner = null;
			if(_playerManager.PlayerOne.TokenColor == winningColor){
				playerWinner = _playerManager.PlayerOne;
			}
			if(_playerManager.PlayerTwo.TokenColor == winningColor){
				playerWinner = _playerManager.PlayerTwo;
			}
			bool isMeTheWinner = playerWinner.PlayerID == Multiplayer.MultiplayerPeer.GetUniqueId();
			ShowDialogBaseOnResult(isMeTheWinner);
		}
	}
}