﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드 데이터,몬스터 데이터,정보 데이터,카드풀 데이터
/// </summary>
public static class Database
{

     public static Dictionary<int, CardData> cardDatas { get; private set; }
     public static Dictionary<int, CardPoolData> cardPoolDatas { get; private set; }
     public static Dictionary<int, MonsterData> monsterDatas { get; private set; }
     public static Dictionary<int, DiaryData> diaryDatas{ get; private set; }
     public static Dictionary<int, AchiveData> achiveDatas { get; private set; }
     private static bool isParsed;

    public static void ReadDatas()
    {
        if (isParsed) return;
        cardDatas = CsvParser.ReadCardData("Data/CardData/CardData");
        cardPoolDatas = CsvParser.ReadCardPoolData("Data/CardPoolData/CardPoolData");
        monsterDatas = CsvParser.ReadMonsterData("Data/MonsterData/MonsterData");
        diaryDatas = CsvParser.ReadDiaryData("Data/DiaryData");
        achiveDatas = CsvParser.ReadAchiveData("Data/AchiveData");
        isParsed = true;
    }
    public static CardData GetCardData(int i)
    {
        return cardDatas[i];
    }
    public static CardPoolData GetCardPool(int i)
    { 
        return cardPoolDatas[i];
    }
    public static MonsterData GetMonsterData(int i)
    {
        return monsterDatas[i];
    }
    public static CardPoolData GetCardPoolByValue(int value)
    {
        int offset = int.MaxValue;
        int index = 0;
         for(int i=0; i<cardPoolDatas.Count;i++)
        {
            int temp = Mathf.Abs(value - cardPoolDatas[i].value);
            if(temp<offset)
            {
                offset = temp;
                index = i;
            }
        }
        return cardPoolDatas[index];
    }

}
public class CardData
{
    public readonly byte index;
    public readonly string name;
    public readonly byte cost;
    public readonly byte attribute;
    public readonly int val1;
    public readonly int val2;
    public readonly int val3;

    public readonly string _info;
    public readonly string spritePath;
    public readonly string className;

    public CardData(string[] data)
    {
        index = byte.Parse(data[0]);
        name = data[1];
        cost = byte.Parse(data[2]);
        attribute = byte.Parse(data[3]);

        val1 = int.Parse(data[4]);
        val2 = int.Parse(data[5]);
        val3 = int.Parse(data[6]);

        _info = data[7];
        spritePath = data[8];

        className = data[9].Substring(0,data[9].Length-1);
    }
}
public class CardPoolData
{
    public readonly byte num;
    public readonly int value;
    public readonly List<int> cardPool = new List<int>();

    public CardPoolData(string[] data)
    {
        num = byte.Parse(data[0]);
        value = int.Parse(data[1]);
        string[] d = data[2].Split('/');
        for(int i=0; i<d.Length;i++)
        {
            cardPool.Add(int.Parse(d[i]));
        }
    }

    /// <summary>
    /// 카드풀 중에서 랜덤으로 카드를 뽑아서 리턴
    /// </summary>
    public Card GetRandomCard()
    {
        return Card.GetCardByNum(cardPool[Random.Range(0, cardPool.Count)]);
    }
}

public class MonsterData
{
    public readonly short num;
    public readonly int hp;
    public readonly int atk;
    public readonly int def;
    public readonly short rank;
    public readonly bool vision;
    public readonly byte visionDistance;

    public MonsterData(string[] data)
    {
        num = short.Parse(data[0]);
        hp = int.Parse(data[1]);
        atk = int.Parse(data[2]);
        def = int.Parse(data[3]);
        rank = short.Parse(data[4]);
        vision = bool.Parse(data[5]);
        visionDistance = byte.Parse(data[6]);
    }
}

public class DiaryData
{
    public readonly byte num;
    public readonly Category category;
    public readonly string title;
    public readonly string info;
    public readonly string spritePath;

    public DiaryData(string[] data)
    {
        num = byte.Parse(data[0]);
        switch (data[1])
        {
            case "실험체": category = Category.irregulars; break;
            case "R.A": category = Category.raChips; break;
            case "연구기록": category = Category.records; break;
            case "휴먼": category = Category.humans; break;
            default: Debug.Log("다이어리 카테고리 형식이 맞지 않습니다!"); break;
        }
        title = data[2];
        info = data[3];
        spritePath = data[4];
    }
}

public class AchiveData
{
    public readonly byte num;
    public readonly string condition;
    public readonly string type;
    public readonly string addition;
    public readonly string reward;
    public readonly string cardReward;

    public AchiveData(string[] data)
    {
        num = byte.Parse(data[0]);
        condition = data[1];
        type = data[2];
        addition = data[3];
        reward = data[4];
        //cardReward = data[5];
    }
}
