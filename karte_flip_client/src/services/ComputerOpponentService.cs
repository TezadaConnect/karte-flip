using System;
using Godot;
using Godot.Collections;
using KarteFlipClient;

static class ComputerOpponentService{
    public static void GetComputerTileMove(GridGroundTilemap tilemap, CardModel card, TokenColorEnum currentTokenColor){        
        Array<Vector2I> vectorHolder = tilemap.GetUsedCells(tilemap.GROUND_LAYER);
        Array<Dictionary> record = new();
        TileFlipableRecordModel newRecord = new();
        Array<Vector2I> listOfVacantTilePosition = new();
    
        foreach (Vector2I elementVector in vectorHolder){
            newRecord.TilePosition = elementVector;
            TileData getTileData = tilemap.GetCellTileData(tilemap.TOKEN_PLACEMENT_LAYER, elementVector);
            if(getTileData != null){
                continue;
            }
            listOfVacantTilePosition.Add(elementVector);
            AddVectorFlipable(elementVector, tilemap, newRecord, card.CardListFlipDirections, currentTokenColor);

            if(newRecord.AllFlipableTiles.Count <= 0){
                continue;
            }

            record.Add(newRecord.Serialize());
            newRecord = new TileFlipableRecordModel();
        }

        TileFlipableRecordModel chosenTile = new();
        
        if(record.Count <= 0){
            Vector2I randomTilePosition = listOfVacantTilePosition[new Random().Next(listOfVacantTilePosition.Count)];
            TileHelper.AddAtlasToken(
				randomTilePosition, 
				tilemap, 
				currentTokenColor
			);
            return;
        }

        foreach (Dictionary element in record){
            TileFlipableRecordModel itemRecordModel = TileFlipableRecordModel.Deserialize(element);
            if(itemRecordModel.AllFlipableTiles.Count > chosenTile.AllFlipableTiles.Count){
                chosenTile = itemRecordModel;
            }
        }

        TileHelper.AddAtlasToken(chosenTile.TilePosition, tilemap, currentTokenColor);
        TokenFlipService.FlipTokens(
            chosenTile.TilePosition, 
            tilemap, 
            card.CardListFlipDirections, 
            currentTokenColor
        );
    }

    private static void AddVectorFlipable(
        Vector2I position, 
        GridGroundTilemap tilemap,
        TileFlipableRecordModel tileFlipableRecordModel, 
        Array<DirectionEnum> multipleDirection,
        TokenColorEnum currentColor
    ){
        foreach (DirectionEnum element in multipleDirection){
            AddVectorFlipable(position, tilemap, tileFlipableRecordModel, element, currentColor);
        }
    }

    private static void AddVectorFlipable(
        Vector2I position, 
        GridGroundTilemap tilemap,
        TileFlipableRecordModel tileFlipableRecordModel, 
        DirectionEnum directionEnum,
        TokenColorEnum currentColor
    ){
        Vector2I tilePosition = TileHelper.GetNextTilePositionByDirectection(position, directionEnum);
		TileData tileSpotData = tilemap.GetCellTileData(tilemap.TOKEN_PLACEMENT_LAYER, tilePosition);
        Vector2I atlastTileImage = tilemap.GetCellAtlasCoords(tilemap.TOKEN_PLACEMENT_LAYER, tilePosition);

        bool isTileDataEmpty = tileSpotData == null;
        bool isTileImageSameAsTileDisplay = atlastTileImage == TileHelper.AtlasLocation(currentColor);

        if (isTileDataEmpty || isTileImageSameAsTileDisplay){
			return;
		}

        tileFlipableRecordModel.AddFlipableTile(tilePosition);
        AddVectorFlipable(tilePosition, tilemap, tileFlipableRecordModel, directionEnum, currentColor);
    }
}