using System;
using Godot.Collections;

public class MatchRecordModel {
    private string _matchID;
    private Array<long> _playersID;

    public MatchRecordModel(){
        _matchID = UniqueIDGeneratorHelper.GenerateUniqueID();
        _playersID = new Array<long>();
    }

    public string MatchID {
        get { return _matchID; }
        set { _matchID = value; }
    }

    public Array<long> PlayersID {
        get { return _playersID; }
        set { _playersID = value; }
    }

    public bool IsNeedingAPlayer (){
        if(_playersID.Count >= 0 && _playersID.Count <= 1){
            return true;
        }
        return false;
    }

    public void AddPlayer(long playerID){
        if(IsNeedingAPlayer()){
            _playersID.Add(playerID);
        }
    }

    public void RemovePlayer(long playerID){
        _playersID.Remove(playerID);
    }

    public Dictionary Serialize(){
        Dictionary serializeValue = new();
        serializeValue["players_id"] = _playersID;
        serializeValue["match_id"] = _matchID;
        return serializeValue;
    }

    public static MatchRecordModel Deserialize(Dictionary serializeValue){
        MatchRecordModel item = new(){
            MatchID = (string)serializeValue["match_id"],
            PlayersID = (Array<long>)serializeValue["players_id"]
        };
        return item;
    }

}