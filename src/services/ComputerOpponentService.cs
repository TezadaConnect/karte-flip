using System;
using Godot;
using Godot.Collections;

static class ComputerOpponentService{
    public static void GetComputerTileMove(GridGroundTilemap tilemap, CardModel card){        
        Array<Vector2I> vectorHolder = tilemap.GetUsedCells(tilemap.GROUND_LAYER);
        Array<Dictionary> record = new();
        TileFlipableRecordModel newRecord = new();
        Array<Vector2I> listOfVacantTilePosition = new();
    
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

            record.Add(newRecord.Serialize());
            newRecord = new TileFlipableRecordModel();
        }

        TileFlipableRecordModel chosenTile = new();
        
        if(record.Count <= 0){
            Vector2I randomTilePosition = listOfVacantTilePosition[new Random().Next(listOfVacantTilePosition.Count)];
            // TileHelper.AddAtlasFromGameTurnManagerToTilemap(randomTilePosition, tilemap);
            return;
        }

        foreach (Dictionary element in record){
            TileFlipableRecordModel itemRecordModel = TileFlipableRecordModel.Deserialize(element);
            if(itemRecordModel.GetAllFlipableTiles().Count > chosenTile.GetAllFlipableTiles().Count){
                chosenTile = itemRecordModel;
            }
        }

        // TileHelper.AddAtlasFromGameTurnManagerToTilemap(chosenTile.GetTilePosition(), tilemap);
        // TokenFlipService.FlipTokens(chosenTile.GetTilePosition(), tilemap, card.GetCardListFlipDirections());
    }

    private static void AddVectorFlipable(
        Vector2I position, 
        GridGroundTilemap tilemap,
        TileFlipableRecordModel tileFlipableRecordModel, 
        Array<DirectionEnum> multipleDirection
    ){
        foreach (DirectionEnum element in multipleDirection){
            AddVectorFlipable(position, tilemap, tileFlipableRecordModel, element);
        }
    }

    private static void AddVectorFlipable(
        Vector2I position, 
        GridGroundTilemap tilemap,
        TileFlipableRecordModel tileFlipableRecordModel, 
        DirectionEnum directionEnum
    ){
        Vector2I tilePosition = TileHelper.GetNextTilePositionByDirectection(position, directionEnum);
		TileData tileSpotData = tilemap.GetCellTileData(tilemap.TOKEN_PLACEMENT_LAYER, tilePosition);
        Vector2I atlastTileImage = tilemap.GetCellAtlasCoords(tilemap.TOKEN_PLACEMENT_LAYER, tilePosition);

        bool isTileDataEmpty = tileSpotData == null;
        bool isTileImageSameAsTileDisplay = atlastTileImage == TileHelper.GetAtlasPositionFromGameTurnManager();

        if (isTileDataEmpty || isTileImageSameAsTileDisplay){
			return;
		}

        tileFlipableRecordModel.AddFlipableTile(tilePosition);
        AddVectorFlipable(tilePosition, tilemap, tileFlipableRecordModel, directionEnum);
    }
}