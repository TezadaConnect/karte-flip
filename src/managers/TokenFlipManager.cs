using System.Collections.Generic;
using Godot;

class TokenFlipManager {
    private static TokenFlipManager mTokenFlipManagerIntance;

    public static TokenFlipManager GetInstance(){
        mTokenFlipManagerIntance ??= new TokenFlipManager();
        return mTokenFlipManagerIntance;
    }

    public void FlipTokens(Vector2I inputPosition, GridGroundTilemap tileMap, List<DirectionEnum> multipleDirection){
        foreach (DirectionEnum element in multipleDirection){
            FlipTokens(inputPosition, tileMap, element);
        }
    }

    private void FlipTokens(Vector2I inputPosition, GridGroundTilemap tileMap, DirectionEnum directionEnum){
		Vector2I tilePosition = GetPositionByDirection(inputPosition, directionEnum);
		TileData tileSpotData = tileMap.GetCellTileData(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);
        Vector2I atlastTileImage = tileMap.GetCellAtlasCoords(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);

        bool isTileDataEmpty = tileSpotData == null;
        bool isTileImageSameAsTileDisplay = atlastTileImage == GameTurnManager.GetInstance().GetTileForDisplay();

		if (isTileDataEmpty || isTileImageSameAsTileDisplay){
			return;
		}

		SetTileBaseOnPlayersTurn(tilePosition, tileMap);
		FlipTokens(tilePosition, tileMap, directionEnum);
	}

    public Vector2I GetPositionByDirection(Vector2I inputPosition, DirectionEnum directionEnum){
        
        if(directionEnum == DirectionEnum.RIGHT){
            return new Vector2I(inputPosition.X + 1, inputPosition.Y);
        }

        if(directionEnum == DirectionEnum.LEFT){
            return new Vector2I(inputPosition.X - 1, inputPosition.Y);
        }

        if(directionEnum == DirectionEnum.TOP){
            return new Vector2I(inputPosition.X, inputPosition.Y - 1);
        }

        return new Vector2I(inputPosition.X, inputPosition.Y + 1);
    }

    public void SetTileBaseOnPlayersTurn(Vector2I inputPosition, GridGroundTilemap tilemap){
		tilemap.SetCell(
			tilemap.TOKEN_PLACEMENT_LAYER, 
			inputPosition, 
			tilemap.ADD_TILE_ACTION,	
			GameTurnManager.GetInstance().GetTileForDisplay()
		);
	}

}