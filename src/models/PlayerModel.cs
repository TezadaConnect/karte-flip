using System;

class PlayerModel : IEquatable<PlayerModel> {
    private PlayerTypeEnum mPlayerType;
    private TokenColorEnum mTokenColorType;

    public PlayerModel(PlayerTypeEnum playerTypeEnum, TokenColorEnum tokenColorEnum){
        mPlayerType = playerTypeEnum;
        mTokenColorType = tokenColorEnum;
    }

    public TokenColorEnum GetTokenColorType(){
        return mTokenColorType;
    }

    public PlayerTypeEnum GetPlayerType(){
        return mPlayerType;
    }

    public bool Equals(PlayerModel other){
        if(
            mPlayerType == other.GetPlayerType() &&
            mTokenColorType == other.GetTokenColorType()
        ){
            return true;
        }
        return false;
    }

    public bool IsLightToken(){
        if(TokenColorEnum.LIGHT_TOKEN == mTokenColorType){
            return true;
        }
        return false;
    }
}