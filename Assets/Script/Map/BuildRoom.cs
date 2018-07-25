﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public static class BuildRoom
{
    static Map newMap;
    static List<string> defaultRoomData;
    static List<string> battleRoomData;
    static List<string> bossRoomData;
    static List<string> eventRoomData;
    static List<string> shopRoomData;
    static Vector2Int size;
    static Tile[,] tiles;
    static Room room;
    static string[,] roomData;

    /// <summary>
    /// 해당 타입의 방을 지정해서 반환
    /// </summary>
    public static Room Build(RoomType type, string name)
    {

        room = InstantiateDelegate.Instantiate(Resources.Load("Room") as GameObject, newMap.transform).GetComponent<Room>();
        room.roomType = type;

        roomData = CsvParser.ReadRoom(1, type, name);
        size = new Vector2Int(roomData.GetLength(1), roomData.GetLength(0));
        Debug.Log(size);
        room.size = size;
        tiles = new Tile[size.x, size.y];
        room.SetTileArray(tiles);

        Draw();
        GenerateGraph();

        roomData = null;
        return room;
    }

    /// <summary>
    /// 해당타입의 방을 랜덤으로 반환
    /// </summary>
    public static Room Build(RoomType type)
    {

        room = InstantiateDelegate.Instantiate(Resources.Load("Room") as GameObject, newMap.transform).GetComponent<Room>();
        room.roomType = type;

        roomData = GetRoomData(type);
        size = new Vector2Int(roomData.GetLength(1), roomData.GetLength(0));
        Debug.Log(size);

        room.size = size;
        tiles = new Tile[size.x, size.y];
        room.SetTileArray(tiles);

        Draw();
        GenerateGraph();

        roomData = null;
        return room;
    }
    public static void SetRoomData(Map _newMap)
    {
        newMap = _newMap;

        defaultRoomData = new List<string>();
        battleRoomData = new List<string>();
        bossRoomData = new List<string>();
        eventRoomData = new List<string>();
        shopRoomData = new List<string>();


        Object[] Data = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/DEFAULT");
        for (int i = 0; i < Data.Length; i++)
        {
            defaultRoomData.Add(Data[i].name);
        }
         Data = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/BATTLE");
        for (int i = 0; i < Data.Length; i++)
        {
            battleRoomData.Add(Data[i].name);
        }
        Data = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/BOSS");
        for (int i = 0; i < Data.Length; i++)
        {
            bossRoomData.Add(Data[i].name);
        }
        Data = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/EVENT");
        for (int i = 0; i < Data.Length; i++)
        {
            eventRoomData.Add(Data[i].name);
        }
        Data = Resources.LoadAll("RoomData/Floor" + newMap.floor + "/SHOP");
        for (int i = 0; i < Data.Length; i++)
        {
            shopRoomData.Add(Data[i].name);
        }
    }
     static string[,] GetRoomData(RoomType rt)
    {
        switch(rt)
        {
            case RoomType.DEFAULT:
                return CsvParser.ReadRoom(newMap.floor, rt, defaultRoomData[Random.Range(0, defaultRoomData.Count)]);
            case RoomType.BATTLE:
                return CsvParser.ReadRoom(newMap.floor, rt, battleRoomData[Random.Range(0, battleRoomData.Count)]);
            case RoomType.BOSS:
                return CsvParser.ReadRoom(newMap.floor, rt, bossRoomData[Random.Range(0, bossRoomData.Count)]);
            case RoomType.EVENT:
                return CsvParser.ReadRoom(newMap.floor, rt, eventRoomData[Random.Range(0, eventRoomData.Count)]);
            case RoomType.SHOP:
                return CsvParser.ReadRoom(newMap.floor, rt, shopRoomData[Random.Range(0, shopRoomData.Count)]);
            default:
                Debug.Log("Room Type ERROR");
                return null;
        }
    }


    static void Draw()
    {
        int tile; int offtile; int item; int entity; int eventlayer;


        //string을 다시 tile , item, player , height 항목으로 나눕니다
        for (int i = 0; i < size.y ; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                string[] temp = roomData[i,j].Split('/');
                tile = int.Parse(temp[0]);
                offtile = int.Parse(temp[1]);
                entity = int.Parse(temp[2]);


                if (tile != 0)
                {
                    Tile tempTile = InstantiateDelegate.ProxyInstantiate(Resources.Load("Fields/Tile/"+tile) as GameObject, room.transform).GetComponent<Tile>();
                    tempTile.SetTile(new Vector2Int(j, (size.y - 1) - i), size);
                    tiles[j, (size.y-1)-i] = tempTile;

                    if (offtile != 0)
                    {
                        OffTile ot = InstantiateDelegate.ProxyInstantiate(Resources.Load("Fields/OffTile/" + offtile) as GameObject, tempTile.transform).GetComponent<OffTile>();
                        tempTile.offTile = ot;

                        if (offtile < 5)//문
                        {
                            room.doorList.Add(ot as Door);
                        }
                    }

                    if (entity != 0) //엔타이티
                    {
                        Entity et = InstantiateDelegate.ProxyInstantiate(Resources.Load("Fields/Entity/"+ entity) as GameObject).GetComponent<Entity>();
                        et.SetRoom(room, new Vector2Int(j, (size.y - 1) - i));
                    }

                }

            }
        }
    }


    /// <summary>
    /// tile 그래프 
    /// </summary>
     static void GenerateGraph()
    {
        //TODO : HERE IS TEMP!
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (x > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x - 1, y]);
                }
                if (x < size.x - 1)
                {
                    tiles[x, y].neighbours.Add(tiles[x + 1, y]);
                }
                if (y > 0)
                {
                    tiles[x, y].neighbours.Add(tiles[x, y - 1]);
                }
                if (y < size.y - 1)
                {
                    tiles[x, y].neighbours.Add(tiles[x, y + 1]);
                }
            }
        }
    }
    
}
