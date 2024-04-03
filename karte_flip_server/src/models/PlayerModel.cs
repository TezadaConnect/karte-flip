using System;
using Godot.Collections;
using KarteFlipServer;

namespace KarteFlipServer{
    public class PlayerModel : IEquatable<PlayerModel>{ // IEquatable<PlayerModel> 
        private long _playerID;
        private PlayerTypeEnum _playerType;
        private TokenColorEnum _tokenColorType;

        // Getters and Setters
        public long PlayerID{
            get { return _playerID; }
            set { _playerID = value; }
        }

        public TokenColorEnum TokenColor{
            get { return _tokenColorType; }
            set { _tokenColorType = value; }
        }

        public PlayerTypeEnum PlayerType {
            get { return _playerType; }
            set { _playerType = value; }
        }

        public Dictionary Serialize(){
            Dictionary serializeValue = new(){
                ["player_id"] = _playerID,
                ["player_type"] = (int)_playerType,
                ["token_color"] = (int)_tokenColorType
            };
            return serializeValue;
        }

        public static PlayerModel Deserialize(Dictionary serializeValue){
            PlayerModel item = new(){
                PlayerID = (long)serializeValue["player_id"],
                PlayerType = (PlayerTypeEnum)(int)serializeValue["player_type"],
                TokenColor =(TokenColorEnum)(int)serializeValue["token_color"]
            };
            return item;
        }

        public bool Equals(PlayerModel other){
            if(other.PlayerID != _playerID){
                return false;
            }
            if(other.PlayerType != _playerType){
                return false;
            }
            if(other.TokenColor != _tokenColorType){
                return false;
            }
            return true;
        }
    }
}