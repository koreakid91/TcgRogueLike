﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void CallBack();

/// <summary>
/// UI MANAGER 
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static Transform canvas;
    private void Awake()
    {
        instance = this;
        canvas = transform;
        mapUI = transform.Find("MapUI").GetComponent<MapUI>();
        hpUI = transform.Find("StatusUI").Find("HpUI").GetComponent<HpUI>();
        akashaUI = transform.Find("StatusUI").Find("AkashaUI").GetComponent<AkashaUI>();
        deckUI = transform.Find("Deck").GetComponent<DeckEditUI>();
        gameOverUI = transform.Find("GameOverUI").GetComponent<GameOverUI>();
        gameWinUI = transform.Find("GameWinUI").GetComponent<GameWinUI>();

        deck = transform.Find("Deck").GetComponent<Deck>();
        hand = transform.Find("HandCards").Find("HandOffSet").Find("Hand").GetComponent<Hand>();
        textUI = transform.Find("TextUI").GetComponent<TextUI>();
        error = transform.Find("ErrorPopUp").GetComponent<ErrorPopUpUI>();   
    }
    TextUI textUI;
    AkashaUI akashaUI;
    HpUI hpUI;
    GameOverUI gameOverUI;
    DeckEditUI deckUI;
    MapUI mapUI;
    Deck deck;
    Hand hand;
    ErrorPopUpUI error;
    GameWinUI gameWinUI;

    #region Status
    public void HpUpdate(int currentHp_, int fullHp_)
    {
        hpUI.HpUpdate(currentHp_, fullHp_);
    }
    public void AkashaUpdate(int current, int full)
    {
        akashaUI.AkashaUpdate(current, full);
    }

    #endregion

    #region DeckEdit
    public void DeckEditUIOn(bool b = false)
    {
        if (!GameManager.instance.IsInputOk)
            return;

        deckUI.On(b);
        GameManager.instance.IsInputOk = false;
    }
    public void DeckEditUIOff()
    {
        deckUI.Off();
        GameManager.instance.IsInputOk = true;
    }
    public void ExchangeButton()
    {
        deckUI.ExchangeCards();
    }
    #endregion

    #region Map
    /// <summary>
    /// 맵 이미지에 텍스쳐 설정, 크기 설정
    /// </summary>
    public void SetMapTexture(Texture2D texture, Vector2Int size)
    {
        mapUI.SetMapTexture(texture, size);
    }
    /// <summary>
    /// 미니맵 움직이기 target 포지션으로 코루틴써서 옮김
    /// </summary>
    public void MoveMiniMap(Vector3 origin, Vector3 target)
    {
        mapUI.MoveMiniMap(origin, target);
    }


    public void OpenFullMap()
    {
        mapUI.OpenFullMap();
    }
    public void CloseFullMap()
    {
        mapUI.CloseFullMap();
    }
    #endregion

    public void ErrorPopUpOn(Debuffs de)
    {
        error.gameObject.SetActive(true);
    }
    public void ErrorPopUpOff()
    {
        error.gameObject.SetActive(false);
    }
    public Deck GetDeck()
    {
        return deck;
    }
    public Hand GetHand()
    {
        return hand;
    }
    public void GameOverUIOn()
    {
        gameOverUI.On();
    }
    public void GameWinUIOn()
    {
        gameWinUI.On();
    }

    public void ShowTextUI(string[] s, CallBack cb)
    {
        textUI.StartText(s, cb);
    }
    public void TextUIGoNext()
    {
        textUI.GoNext();
    }

}
