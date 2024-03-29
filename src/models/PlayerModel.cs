using System;

class PlayerModel { // IEquatable<PlayerModel> 
    private long _playerID;
    private PlayerTypeEnum _playerType;
    private TokenColorEnum _tokenColorType;

    public PlayerModel(long playerID, PlayerTypeEnum playerTypeEnum, TokenColorEnum tokenColorEnum){
        _playerID = playerID;
        _playerType = playerTypeEnum;
        _tokenColorType = tokenColorEnum;
    }

    public long PlayerID{
        get { return _playerID; }
        set { _playerID = PlayerID; }
    }

    public TokenColorEnum TokenColor{
        get { return _tokenColorType; }
        set { _tokenColorType = TokenColor; }
    }

    public PlayerTypeEnum PlayerType {
        get { return _playerType; }
        set { _playerType = PlayerType; }
    }

    // public bool Equals(PlayerModel other){
    //     if(
    //         mPlayerType == other.GetPlayerType() &&
    //         mTokenColorType == other.GetTokenColorType()
    //     ){
    //         return true;
    //     }
    //     return false;
    // }

    // public bool IsLightToken(){
    //     if(TokenColorEnum.LIGHT_TOKEN == mTokenColorType){
    //         return true;
    //     }
    //     return false;
    // }
}