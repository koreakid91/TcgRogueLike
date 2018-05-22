﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character,ITurnAble {

    protected virtual void Start()
    {
        EnterEvent();
    }
    public abstract void DoAct(int turn);
    public void EnterEvent()
    {
        currentRoom.TurnalbeList.Add(this);
    }
}
