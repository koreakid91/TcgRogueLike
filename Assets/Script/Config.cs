﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config instance;

    private void Awake()
    {
        instance = this;
    }
    [Header("Seed")]
    public bool UseRandomSeed;
    public int Seed;

    [Header("Deck Settings")]
    public int HandMax = 5;

    public bool UseCustomDeck;
    public string[] CustomDeck;

    [Header("Map Settings")]
    public int floorNum = 1;
    public int battleRoomNum;
    public int eventRoomNum;
    public int shopRoomNum;

    [Header("TestMode")]
    public bool RoomTestMode;
    public RoomType TestRoomType;
    public string TestRoomName;

}
