using Godot;
using Godot.Collections;

public class TileFlipableRecordModel{
    private Vector2I mTilePosition;
    private Array<Vector2I> mAllFlipableTiles;

    public TileFlipableRecordModel(){
        mAllFlipableTiles = new Array<Vector2I>();
    }

    public Dictionary Serialize(){
        Dictionary serializeValue = new();
        serializeValue["tile_position"] = mTilePosition;
        serializeValue["card_description"] = mAllFlipableTiles;
        return serializeValue;
    }

    public static TileFlipableRecordModel Deserialize(Dictionary serializeValue){
        TileFlipableRecordModel item = new();
        item.SetTilePosition((Vector2I)serializeValue["tile_position"]);
        item.SetFlipableTile((Array<Vector2I>)serializeValue["card_description"]);
        return item;
    }

    public Vector2I GetTilePosition(){
        return mTilePosition;
    }

    public void SetTilePosition(Vector2I value){
        mTilePosition = value;
    }

    public Array<Vector2I> GetAllFlipableTiles(){
        return mAllFlipableTiles;
    }

    public void AddFlipableTile(Vector2I value){
        mAllFlipableTiles.Add(value);
    }

    public void SetFlipableTile(Array<Vector2I> list){
        mAllFlipableTiles = list;
    }
}