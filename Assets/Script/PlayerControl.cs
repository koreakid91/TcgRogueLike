﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class PlayerControll : MonoBehaviour {

    public static PlayerControll instance;
    public void Awake()
    {
        instance = this;
    }
    public bool InputOk = false;
    // Use this for initialization
    void Start () {
        Input.multiTouchEnabled = false;
        InputOk = true;
	}
	
	public void PlayerMove(Tile pos)
    { 
        if(InputOk)
        StartCoroutine(PlayerMoveRoutine(pos));    
    }
	IEnumerator PlayerMoveRoutine(Tile pos)
    {
        InputOk = false;

        Room cur = Player.instance.currentRoom;
        int curHp = Player.instance.currentHp;

		List<Tile> path = PathFinding.instance.GeneratePath(Player.instance, pos);
		while (path.Count>0 && path[0].OnTileObj == null &&
            cur == Player.instance.currentRoom&& Player.instance.currentHp==curHp)
        {
            Player.instance.MoveTo(path[0].pos);
            path.RemoveAt(0);

            //TODO TurnManager.instance.MoveNextTurn();
            yield return new WaitForSeconds(0.1f);
        }

        InputOk = true;
    }
}
