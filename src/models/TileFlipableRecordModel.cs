using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

class TileFlipableRecordModel{
    private Vector2I mTilePosition;
    private List<Vector2I> mAllFlipableTiles;


    public TileFlipableRecordModel(){
        mAllFlipableTiles = new List<Vector2I>();
    }

    public Vector2I GetTilePosition(){
        return mTilePosition;
    }

    public void SetTilePosition(Vector2I value){
        mTilePosition = value;
    }

    public List<Vector2I> GetAllFlipableTiles(){
        return mAllFlipableTiles;
    }

    public void AddFlipableTile(Vector2I value){
        mAllFlipableTiles.Add(value);
    }
}