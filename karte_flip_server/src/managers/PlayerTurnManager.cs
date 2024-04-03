using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace KarteFlipServer{
    public partial class PlayerTurnManager: Node{
        public List<MatchRecordModel> _matchList;
        public List<long> _clientIDRecord;

        public override void _Ready(){
            _matchList = new List<MatchRecordModel>();
            _clientIDRecord = new List<long>();
        }

        public void AddClientRecord(long clientID){
            if(!_clientIDRecord.Contains(clientID)){
                _clientIDRecord.Add(clientID);
            }
        }

        public void FindMatchForClient(long clientID){
            // When no match record
            if(_matchList.Count <= 0){
                MatchRecordModel matchRecord = new();
                matchRecord.AddPlayer(clientID);
                _matchList.Add(matchRecord);
                return;
            }
            
            GD.Print("Already have matches, finding queued players");
            foreach (MatchRecordModel item in _matchList.ToList()){
                if(!item.IsNeedingAPlayer()){
                    continue;
                }
                item.AddPlayer(clientID);
                MatchFound(item);
                return;
            }

            GD.Print("All match is fully book, creating new match instance");
            MatchRecordModel newMatch = new();
            newMatch.AddPlayer(clientID);
            _matchList.Add(newMatch);
        }

        public void RemoveClientRecordWhenDisconnected(long clientID){
            _clientIDRecord.Remove(clientID);
            foreach (MatchRecordModel item in _matchList.ToList()){
                List<long> playersIDList = item.PlayersID.ToList();
                foreach (long playerID in playersIDList){
                    if(playerID.Equals(clientID)){
                        _matchList.Remove(item);
                        continue;
                    }
                    RpcId(playerID, nameof(DisconnectClient));
                }
            }
        }

        public void MatchFound(MatchRecordModel match){
            List<TokenColorEnum> allTokens = new List<TokenColorEnum>{ 
            	TokenColorEnum.LIGHT_TOKEN, 
            	TokenColorEnum.DARK_TOKEN 
            };

            Array<Dictionary> listOfPlayers = new();

            TokenColorEnum randomToken = allTokens[new Random().Next(allTokens.Count)];

            TokenColorEnum reverseOfRandomToken = 
                randomToken == TokenColorEnum.DARK_TOKEN ? 
                TokenColorEnum.LIGHT_TOKEN : 
                TokenColorEnum.DARK_TOKEN;

            for (int i = 0; i < match.PlayersID.Count; i++){
                listOfPlayers.Add(new PlayerModel(){
                    PlayerID = match.PlayersID[i], 
                    PlayerType = PlayerTypeEnum.PERSON, 
                    TokenColor = i < 1 ? randomToken : reverseOfRandomToken
                }.Serialize());
            }

            //Code here;
            foreach (long playerID in match.PlayersID.ToList()){
                RpcId(playerID, "StartMatch", listOfPlayers);
            }
        }

        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void ProcessTurnDataOnServer(Vector2I position, Dictionary card){
            long senderID = Multiplayer.GetRemoteSenderId();
    
            foreach (MatchRecordModel item in _matchList.ToList()){
                if(!item.PlayersID.Contains(senderID)){
                    continue;
                }
                foreach (long matchedPlayerId in item.PlayersID.ToList()){
                    RpcId(matchedPlayerId , nameof(RecieveDataToSpecificClient), position, card);
                } 
                return;
            }
        }
        
        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void RecieveDataToSpecificClient(Vector2I position, Dictionary card){}

        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void DisconnectClient(){}

        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        public void StartMatch(Array<Dictionary> players){}  
    }
}