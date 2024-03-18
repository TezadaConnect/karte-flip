using System;
using System.Collections.Generic;
using Godot;

class AIManager {
    private static AIManager mAIManagerInstance;

    public static AIManager GetInstance(){
        mAIManagerInstance ??= new AIManager();
        return mAIManagerInstance;
    }

    public void GetAITileMove(GridGroundTilemap tilemap, CardModel card){        
        Godot.Collections.Array<Vector2I> vectorHolder = tilemap.GetUsedCells(tilemap.GROUND_LAYER);
        List<TileFlipableRecordModel> record = new List<TileFlipableRecordModel>();
        TileFlipableRecordModel newRecord = new TileFlipableRecordModel();

        List<Vector2I> listOfVacantTilePosition = new List<Vector2I>();
    
        foreach (Vector2I elementVector in vectorHolder){
            newRecord.SetTilePosition(elementVector);
            TileData getTileData = tilemap.GetCellTileData(tilemap.TOKEN_PLACEMENT_LAYER, elementVector);
            if(getTileData != null){
                continue;
            }
            listOfVacantTilePosition.Add(elementVector);
            AddVectorFlipable(elementVector, tilemap, newRecord, card.GetCardListFlipDirections());

            if(newRecord.GetAllFlipableTiles().Count <= 0){
                continue;
            }

            record.Add(newRecord);
            newRecord = new TileFlipableRecordModel();
        }

        TokenFlipManager tokenFlipManagerInstance = TokenFlipManager.GetInstance();
        TileFlipableRecordModel chosenTile = new TileFlipableRecordModel();
        
        if(record.Count <= 0){
            Vector2I randomTilePosition = listOfVacantTilePosition[new Random().Next(listOfVacantTilePosition.Count)];
            tokenFlipManagerInstance.SetTileBaseOnPlayersTurn(randomTilePosition, tilemap);
            return;
        }

        foreach (TileFlipableRecordModel element in record){
            if(element.GetAllFlipableTiles().Count > chosenTile.GetAllFlipableTiles().Count){
                chosenTile = element;
            }
        }

        tokenFlipManagerInstance.SetTileBaseOnPlayersTurn(chosenTile.GetTilePosition(), tilemap);
        tokenFlipManagerInstance.FlipTokens(chosenTile.GetTilePosition(), tilemap, card.GetCardListFlipDirections());
    }

    private void AddVectorFlipable(
        Vector2I position, 
        GridGroundTilemap tilemap,
        TileFlipableRecordModel tileFlipableRecordModel, 
        DirectionEnum[] multipleDirection
    ){
        foreach (DirectionEnum element in multipleDirection){
            AddVectorFlipable(position, tilemap, tileFlipableRecordModel, element);
        }
    }

    private void AddVectorFlipable(
        Vector2I position, 
        GridGroundTilemap tilemap,
        TileFlipableRecordModel tileFlipableRecordModel, 
        DirectionEnum directionEnum
    ){
        Vector2I tilePosition = TokenFlipManager.GetInstance().GetPositionByDirection(position, directionEnum);
		TileData tileSpotData = tilemap.GetCellTileData(tilemap.TOKEN_PLACEMENT_LAYER, tilePosition);
        Vector2I atlastTileImage = tilemap.GetCellAtlasCoords(tilemap.TOKEN_PLACEMENT_LAYER, tilePosition);

        bool isTileDataEmpty = tileSpotData == null;
        bool isTileImageSameAsTileDisplay = atlastTileImage == GameTurnManager.GetInstance().GetTileForDisplay();

        if (isTileDataEmpty || isTileImageSameAsTileDisplay){
			return;
		}

        tileFlipableRecordModel.AddFlipableTile(tilePosition);
        AddVectorFlipable(tilePosition, tilemap, tileFlipableRecordModel, directionEnum);
    }

}