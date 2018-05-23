﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
/// <summary>
/// 타일 속성
/// </summary>
public abstract class OffTile :MonoBehaviour
{
	protected Tile thisTile;

	public void setTile(Tile tile)
    {
        thisTile = tile;
        transform.parent = tile.transform;
        transform.position = tile.transform.position;
    }
    public abstract void SomethingUpOnThis(OnTileObject ot);
    public abstract bool IsStandAble(OnTileObject ot);
}
