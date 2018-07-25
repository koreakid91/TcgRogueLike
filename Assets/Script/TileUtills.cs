﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;


/// <summary>
/// Player의 스킬 함수 작성시 도움되는 함수들 입니다.
/// </summary>
public static class TileUtils
{

    public static int CalcRange(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x)+ Mathf.Abs(a.y - b.y);
    }
    /// <summary>
    /// 십자로 타일 가져오기
    /// </summary>
    public static List<Tile> CrossRange(Tile center, int radius)
    {
        List<Tile> crossList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y;

        for (int i = 1; i <= radius; i++)
        {
			crossList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x, y + 1)));
            crossList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x, y - 1)));
            crossList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x + 1, y)));
            crossList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x - 1, y)));
        }
		for (int i = crossList.Count - 1; i >= 0; i--) {
			if (crossList [i] == null) {
				crossList.RemoveAt (i);
			}
		}
        return crossList;
    }
    /// <summary>
    /// 원모양으로 타일 가져오기
    /// </summary>
	public static List<Tile> CircleRange(Tile center, int radius)
    {
        List<Tile> circleList = new List<Tile>();
        int x = (int)center.pos.x; int y = (int)center.pos.y + radius;

        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                if (j == 0)
                {
                    circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x, y - i)));

                }
                else
                {

                    circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x - j, y - i)));
                    circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x + j, y - i)));
                }
            }
        }

        y = (int)center.pos.y - radius;

        for (int i = 0; i <= radius - 1; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                if (j == 0)
                {
                    circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x, y + i)));
                }
                else
                {
                    circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x - j, y + i)));
                    circleList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x + j, y + i)));
                }
            }
        }

        circleList.Remove(center);

        return circleList;
    }

    public static List<Tile> SquareRange(Tile center, int radius)
    {
        List<Tile> squareList = new List<Tile>();
        int x = center.pos.x; int y = center.pos.y;
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                squareList.Add(GameManager.instance.GetCurrentRoom().GetTile(new Vector2Int(x + i, y + j)));
            }
        }
        squareList.Remove(center);
		for (int i = squareList.Count - 1; i >= 0; i--) {
			if (squareList [i] == null) {
				squareList.RemoveAt (i);
			}
		}

        return squareList;
    }





    /// <summary>
    /// 근처의 적 리스트 가져옵니다.
    /// </summary>

    public static List<Entity> GetNearEnemies(Tile center,int radius)
    {
        List<Entity> targets = new List<Entity>();
        List<Tile> range = SquareRange(center, radius);
        for(int i=0; i<range.Count;i++)
        {
            if(range[i].OnTileObj && range[i].OnTileObj.IsHitable)
            {
                targets.Add(range[i].OnTileObj);
            }
        }
        return targets;
    }
    public static Entity AutoTarget(Tile center, int radius)
    {
        List<Entity> targets = new List<Entity>();
        List<Tile> range = SquareRange(center, radius);
        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj && range[i].OnTileObj.IsHitable)
            {
                targets.Add(range[i].OnTileObj);
            }
        }
		if (targets.Count >0)
			return targets [Random.Range (0, targets.Count)];
		else
			return null;
    }
	public static bool IsHitableAround(Tile center, int radius)
	{
		List<Entity> targets = new List<Entity>();
		List<Tile> range = SquareRange(center, radius);
		for (int i = 0; i < range.Count; i++)
		{
			if (range[i].OnTileObj && range[i].OnTileObj.IsHitable)
			{
				return true;
			}
		}
		return false;
	}
    /// <summary>
    /// 2타일의 위치관계를 통해, 동,서,남,북 방향을 구해서 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public static Vector2Int GetDir(Tile center,Tile target)
    {        
        Vector2Int v = target.pos - center.pos;
        if(Mathf.Abs(v.x)>Mathf.Abs(v.y))
        {
            return new Vector2Int(v.x /Mathf.Abs(v.x), 0);
        }else
        {
            return new Vector2Int(0,v.y/Mathf.Abs(v.y));
        }
    }
    #region AI Utils
    /// <summary>
    /// 플레이어가 원모양으로 주위에 있는가 체크
    /// </summary>
    public static bool AI_CircleFind(Tile center, int radius)
    {
        List<Tile> range = CircleRange(center, radius);

        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj != null)
            {
                if (range[i].OnTileObj is Player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static bool AI_SquareFind(Tile center, int radius)
    {
        List<Tile> range = SquareRange(center, radius);
        for (int i = 0; i < range.Count; i++)
        {
            if (range[i].OnTileObj != null)
            {
                if (range[i].OnTileObj is Player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
}


