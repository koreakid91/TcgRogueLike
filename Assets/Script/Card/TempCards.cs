﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class CardData_BFSword : CardData_Normal
{
    public CardData_BFSword(int index) : base(index)
	{
        damage = 8;
        range = 1;
        //Set ImageInfo
        cardExplain = range + "의 범위의 모든 적에게+" + damage + "의 데미지를 줍니다.";
    }
    public override void CardActive()
    {

        if (TileUtils.IsHitableAround(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range))
        {
            List<OnTileObject> targets = TileUtils.GetNearEnemies(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range);
            foreach(OnTileObject e in targets)
            {
                e.currentHp -= damage;
            }
        }
    }

    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {
        targetTiles = TileUtils.SquareRange(GameManager.instance.GetCurrentRoom().GetPlayerTile(), range);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].mySprite.color = Color.red;
        }
    }
    public override void CancelPreview()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            targetTiles[i].mySprite.color = Color.white;
        }
    }

}
