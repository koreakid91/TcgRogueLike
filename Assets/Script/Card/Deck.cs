﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	public Text txt_RemainCard;
	public List<Card> remainDeck;

	public CardObject Draw(){
		if (remainDeck.Count <= 0) {
			return null;
		}
		Card c = remainDeck [remainDeck.Count - 1];
		remainDeck.RemoveAt(remainDeck.Count - 1);
		RefreshText ();
		return c.InstantiateHandCard ();
	}

	public void Load()
    {
		remainDeck = new List<Card> (PlayerData.Deck);
		Shuffle ();
		remainDeck.Add (remainDeck [0]);
		remainDeck [0] = new Card_Reload (Database.GetCardData(1));//1번은 리로드
		RefreshText ();
	}

	#region Private

	private void Shuffle(){
		Card temp;
		int randIndex;
		for (int i = 0; i < remainDeck.Count - 1; i++) {
			randIndex = Random.Range (0, remainDeck.Count);
			temp = remainDeck [i];
			remainDeck [i] = remainDeck [randIndex];
			remainDeck [randIndex] = temp;
		}
	}

	private void RefreshText(){
		txt_RemainCard.text = "X " + remainDeck.Count; 
	}
	#endregion
}
