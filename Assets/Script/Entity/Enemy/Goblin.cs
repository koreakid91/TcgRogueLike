﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy {

    protected override void Start()
    {
        base.Start();
        fullHp = 10; currentHp = 10;
        damage = 1;
        range = 1;
    }
    protected int range;

    public override void DoAct()
    {
		if (Room.CalcRange (currentTile.pos, currentRoom.GetPlayerTile().pos) <= range)
        {
            Debug.Log("Attack");
            currentRoom.GetPlayerTile().OnTileObj.currentHp -= damage;
			EffectDelegate.instance.MadeEffect (CardEffectType.Hit, currentRoom.GetPlayerTile ());
            OnEndTurn();
		} else {
			MoveTo (PathFinding.instance.GeneratePath (this, currentRoom.GetPlayerTile()) [0].pos);
		}
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        gameObject.name = "Goblin" + _pos;
    }

	protected override void OnDieCallback (){
		//TODO : DROP CARD TEMP
		if (UnityEngine.Random.Range (0, 3) == 0) {
			PlayerControl.instance.AddCard (new CardData_Stone (5));
		} else if (UnityEngine.Random.Range (0, 10) == 0) {
			PlayerControl.instance.AddCard (new CardData_Portion (6));
		}

		base.OnDieCallback ();
	}
}
