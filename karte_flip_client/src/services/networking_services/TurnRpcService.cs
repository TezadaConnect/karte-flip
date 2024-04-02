using System.Collections.Generic;
using System.Linq;
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

			_scoringManager.DisplayScore(_mainTilemap);
		}

		public bool IsMyTurn(){
			if(_playerManager.CurrentPlayerTurn.PlayerID == Multiplayer.GetUniqueId()){
				return true;
			}
			return false;
		}
	}
}