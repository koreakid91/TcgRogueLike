﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public static MapManager instance;
    private void Awake()
    {
        instance = this;
    }


}