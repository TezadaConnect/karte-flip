using Godot;
using Godot.Collections;
using KarteFlipClient;

static class TokenFlipService {
    public static void FlipTokens(
        Vector2I inputPosition, 
        GridGroundTilemap tileMap, 
        Array<DirectionEnum> multipleDirection, 
        TokenColorEnum tokenColorEnum
    ){
        foreach (DirectionEnum element in multipleDirection){
            FlipTokens(inputPosition, tileMap, element, tokenColorEnum);
        }
    }

    private static void FlipTokens(
        Vector2I inputPosition, 
        GridGroundTilemap tileMap, 
        DirectionEnum directionEnum, 
        TokenColorEnum tokenColorEnum
    ){
		Vector2I tilePosition = TileHelper.GetNextTilePositionByDirectection(inputPosition, directionEnum);
		TileData tileSpotData = tileMap.GetCellTileData(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);
        Vector2I atlastTileImage = tileMap.GetCellAtlasCoords(tileMap.TOKEN_PLACEMENT_LAYER, tilePosition);

        bool isTileDataEmpty = tileSpotData == null;
        bool isTileImageSameAsTileDisplay = atlastTileImage == TileHelper.AtlasLocation(tokenColorEnum);

		if (isTileDataEmpty || isTileImageSameAsTileDisplay){
			return;
		}

		TileHelper.AddAtlasToken(tilePosition, tileMap, tokenColorEnum);

		FlipTokens(tilePosition, tileMap, directionEnum, tokenColorEnum);
	}
}