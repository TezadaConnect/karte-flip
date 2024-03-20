using System;
using Godot;

class ScoringManager {

    private int mBlackScore;
    private int mWhiteScore;

    private static ScoringManager mScoringManager;

    public ScoringManager(){
        mBlackScore = 0;
        mWhiteScore = 0;
    }

    public static ScoringManager GetInstance(){
        mScoringManager ??= new ScoringManager();
        return mScoringManager;
    }

    public void CalculateScore(GridGroundTilemap tilemap){
        Godot.Collections.Array<Vector2I> arrayOfTilePositionAtTokenLayer = tilemap.GetUsedCells(
            tilemap.TOKEN_PLACEMENT_LAYER
        );

        int whiteCount = 0;
        int blackCount = 0;
        
        foreach (Vector2I element in arrayOfTilePositionAtTokenLayer){
            Vector2I tileAtlas = tilemap.GetCellAtlasCoords(
                tilemap.TOKEN_PLACEMENT_LAYER, element
            );

            if(GameTurnManager.GetInstance().GetBlackAtlas() == tileAtlas){
                blackCount++;
            } else {
                whiteCount++;
            }
        }

        mWhiteScore = whiteCount;
        mBlackScore = blackCount;
    }

    public int GetWhiteScore(){
        return mWhiteScore;
    }
    public int GetBlackScore(){
        return mBlackScore;
    }

    public void ResetScore(){
        mBlackScore = 0;
        mWhiteScore = 0;
    }
}