using System.Collections.Generic;
using Godot;

static class TokenFlipService {
    public static void FlipTokens(Vector2I inputPosition, GridGroundTilemap tileMap, List<DirectionEnum> multipleDirection){
        foreach (DirectionEnum element in multipleDirection){
            FlipTokens(inputPosition, tileMap, element);
        }
    }

    private static void FlipTokens(Vector2I inputPosition, GridGroundTilemap tileMap, DirectionEnum directionEnum){
		Vector2I tilePosition = TileHelper.GetNextTilePositionByDirectection(inputPosition, directionEnum);
		TileData tileSpotData = tileMap.GetCellTileData(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);
        Vector2I atlastTileImage = tileMap.GetCellAtlasCoords(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);

        bool isTileDataEmpty = tileSpotData == null;
        bool isTileImageSameAsTileDisplay = atlastTileImage == TileHelper.GetAtlasPositionFromGameTurnManager();

		if (isTileDataEmpty || isTileImageSameAsTileDisplay){
			return;
		}

		TileHelper.AddAtlasFromGameTurnManagerToTilemap(tilePosition, tileMap);
		FlipTokens(tilePosition, tileMap, directionEnum);
	}
}