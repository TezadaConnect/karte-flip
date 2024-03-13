using System.Collections.ObjectModel;
using Godot;

class TokenFlipManager {
    private static TokenFlipManager mTokenFlipManagerIntance;

    public static TokenFlipManager GetInsntance(){
        if(mTokenFlipManagerIntance == null){
            mTokenFlipManagerIntance = new TokenFlipManager();
        }
        return mTokenFlipManagerIntance;
    }

    public void FlipTokens(Vector2I inputPosition, GridGroundCustomTilemap tileMap, DirectionEnum[] multipleDirection){
        foreach (DirectionEnum element in multipleDirection){
            FlipTokens(inputPosition, tileMap, element);
        }
    }

    private void FlipTokens(Vector2I inputPosition, GridGroundCustomTilemap tileMap, DirectionEnum directionEnum){
		Vector2I tilePosition = GetPositionByDirection(inputPosition, directionEnum);
		TileData tileSpotData = tileMap.GetCellTileData(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);

		if (tileSpotData == null){
			return;
		}

		SetTileBaseOnPlayersTurn(tilePosition, tileMap);
		FlipTokens(tilePosition, tileMap, directionEnum);
	}

    private Vector2I GetPositionByDirection(Vector2I inputPosition, DirectionEnum directionEnum){
        
        if(directionEnum == DirectionEnum.RIGHT){
            return new Vector2I(inputPosition.X + 1, inputPosition.Y);
        }

        if(directionEnum == DirectionEnum.LEFT){
            return new Vector2I(inputPosition.X - 1, inputPosition.Y);
        }

        if(directionEnum == DirectionEnum.TOP){
            return new Vector2I(inputPosition.X, inputPosition.Y + 1);
        }

        return new Vector2I(inputPosition.X, inputPosition.Y - 1);
    }

    private void SetTileBaseOnPlayersTurn(Vector2I inputPosition, GridGroundCustomTilemap tilemap){
		tilemap.SetCell(
			tilemap.TOKEN_PLACEMENT_LAYER, 
			inputPosition, 
			0,	
			GameTurnManager.GetInstance().GetTileForDisplay()
		);
	}

}