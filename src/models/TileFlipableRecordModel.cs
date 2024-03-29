using Godot;
using Godot.Collections;

public class TileFlipableRecordModel{
    private Vector2I _tilePosition;
    private Array<Vector2I> _allFlipableTiles;

    public TileFlipableRecordModel(){
        _allFlipableTiles = new Array<Vector2I>();
    }

    //Getters
    public Array<Vector2I> AllFlipableTiles {
        get { return _allFlipableTiles; }
        set { _allFlipableTiles = value; }
    }
    public Vector2I TilePosition {
        get { return _tilePosition; }
        set { _tilePosition = value; }
    }

    // Helper Methods
    public Dictionary Serialize(){
        Dictionary serializeValue = new();
        serializeValue["tile_position"] = _tilePosition;
        serializeValue["card_description"] = _allFlipableTiles;
        return serializeValue;
    }

    public static TileFlipableRecordModel Deserialize(Dictionary serializeValue){
        TileFlipableRecordModel item = new(){
            TilePosition = (Vector2I)serializeValue["tile_position"],
            AllFlipableTiles = (Array<Vector2I>)serializeValue["card_description"]
        };
        return item;
    }

    public void AddFlipableTile(Vector2I value){
        _allFlipableTiles.Add(value);
    }
}