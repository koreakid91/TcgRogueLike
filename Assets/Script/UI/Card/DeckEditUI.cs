﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 덱 수정 UI
/// </summary>
public class DeckEditUI : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 offPos = new Vector3(0, 3000, 0);
    private bool isEditOk=false;
    public bool IsEditOk { get { return isEditOk; } }
    private List<EditCardObject> deckCardObjects;
    private List<EditCardObject> attainCardObjects;
    private List<Card> deck;
    private List<Card> attain;
    private List<EditCardObject> deckSelected;
    private List<EditCardObject> attainSelected;
    

    RectTransform deckViewPort;
    public RectTransform DeckViewPort { get { return deckViewPort; } }
    RectTransform attainViewPort;
    public RectTransform AttainViewPort { get { return attainViewPort; } }

    RectTransform exchangePopUp;

    Button changeButton;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        deckViewPort = transform.Find("DeckPanel").Find("DeckCardPool").Find("viewport").GetComponent<RectTransform>();
        attainViewPort = transform.Find("DeckPanel").Find("AttainCardPool").Find("viewport").GetComponent<RectTransform>();

        changeButton = transform.Find("DeckPanel").Find("Buttons").Find("changeButton").GetComponent<Button>();
        exchangePopUp = transform.Find("DeckPanel").Find("Buttons").Find("check").GetComponent<RectTransform>();
    }

    public void On()
    {
        GameManager.instance.IsInputOk = false;
        isEditOk = true;//bool
        rect.anchoredPosition = Vector3.zero;
        MakeCardObjects();
        changeButton.gameObject.SetActive(true);//b
        //if
        RevealAttainCards();
        deckSelected = new List<EditCardObject>();
        attainSelected = new List<EditCardObject>();  
        //     
        UIManager.instance.CardInfoPanel_Off();
    }
    public void Off()
    {
        UIManager.instance.CardInfoPanel_Off();
        StartCoroutine(OffRoutine());
        changeButton.interactable = false;
    }

    public void ExchangePopUp_On()
    {
        exchangePopUp.gameObject.SetActive(true);
    }
    public void ExchangePopUp_Off()
    {
        exchangePopUp.gameObject.SetActive(false);
    }
    public void ExchangeCards()
    {

            for(int i=0; i<deckSelected.Count;i++)
            {
                int di = deckSelected[i].Index;
                int ai = attainSelected[i].Index;
                deck.Remove(deckSelected[i].GetCard());//덱에서 어테인으로
                deckCardObjects.Remove(deckSelected[i]);
                attain.Add(deckSelected[i].GetCard());
                attainCardObjects.Add(deckSelected[i]);
                deckSelected[i].SetParent(attainViewPort, false);
                deckSelected[i].Index = ai;
                deckSelected[i].Locate(ai);

                attain.Remove(attainSelected[i].GetCard());//어테인에서 덱으로
                attainCardObjects.Remove(attainSelected[i]);
                deck.Add(attainSelected[i].GetCard());
                deckCardObjects.Add(attainSelected[i]);
                attainSelected[i].SetParent(deckViewPort, true);
                attainSelected[i].Index = di;
                attainSelected[i].Locate(di);
            }
         
        for(int i=deckSelected.Count-1; i>=0;i--)
        {
            DeckCardSelectOff(deckSelected[i]);
        }

        for (int i = attainSelected.Count-1; i >= 0; i--)
        {
            AttainCardSelectOff(attainSelected[i]);
        }
        isEditOk = false;
        changeButton.interactable = false;
        UIManager.instance.CardInfoPanel_Off();
        ExchangePopUp_Off();
    }


    public void RevealAttainCards()
    {
       StartCoroutine(RevealCardsRoutine());
    }
    public void DeckCardSelectOff(EditCardObject ec)
    {
        ec.HighLightOff();
        deckSelected.Remove(ec);
        CheckExchangeAvail();

    }
    public void AttainCardSelectOff(EditCardObject ec)
    {
        ec.HighLightOff();
        attainSelected.Remove(ec);
        CheckExchangeAvail();
    }
    public void DeckCardSelect(EditCardObject ec)
    {
        if(deckSelected.Count>=3)
        {
            return;
        }
        ec.HighLightOn();
        deckSelected.Add(ec);
        CheckExchangeAvail();
    }
    public void AttainCardSelect(EditCardObject ec)
    {
        if (attainSelected.Count >=3)
        {
            return;
        }
        ec.HighLightOn();
        attainSelected.Add(ec);
        CheckExchangeAvail();
    }
    void CheckExchangeAvail()
    {
        if(attainSelected.Count==deckSelected.Count)
        {
            changeButton.interactable = true;
        }else
        {
            changeButton.interactable = false;
        }
    }

    #region private
    private void MakeCardObjects()
    {
        deck = new List<Card>(PlayerData.Deck);
        deckCardObjects = new List<EditCardObject>();
        for(int i=0; i<deck.Count;i++)
        {
            deckCardObjects.Add(deck[i].InstantiateEditCard());
        }
        for (int i = 0; i < deckCardObjects.Count; i++)
        {
            deckCardObjects[i].SetParent(deckViewPort,true);
            deckCardObjects[i].SetDeckUI(this);
            deckCardObjects[i].SetRenderKnown();
        }

        attain = new List<Card>(PlayerData.AttainCards);
        attainCardObjects = new List<EditCardObject>();
        for (int i = 0; i < attain.Count; i++)
        {
            attainCardObjects.Add(attain[i].InstantiateEditCard());
        }
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            attainCardObjects[i].SetParent(attainViewPort,false);
            attainCardObjects[i].SetDeckUI(this);
            deckCardObjects[i].SetRenderUnknown();
        }
        SortCards();
    }

    private void DeleteAllObjects()
    {
        for(int i=deckCardObjects.Count-1;i>=0;i--)
        {
            Destroy(deckCardObjects[i].gameObject);
        }
        for (int i=attainCardObjects.Count-1;i>=0;i--)
        {           
            Destroy(attainCardObjects[i].gameObject);
        }
    }
    IEnumerator RevealCardsRoutine()
    {
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            if(!attainCardObjects[i].IsReavealed)
            {
                attainCardObjects[i].SetRenderKnown();
                //TODO : 미지카드밝혀지는 애니메이션
            }
        }
        yield return null;
    }
    IEnumerator OffRoutine()
    {

         PlayerData.Deck = deck;
         PlayerData.AttainCards.Clear();
         PlayerControl.instance.ReLoadDeck();
         yield return null;

        DeleteAllObjects();
        rect.anchoredPosition = offPos;
    }
    private void SortCards()
    {
        deckCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {
            int aIndex = (int)A.GetCard().Type*1000 + A.GetCard().Index;
            int bIndex = (int)B.GetCard().Type*1000 + B.GetCard().Index;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        attainCardObjects.Sort(delegate (EditCardObject A, EditCardObject B)
        {           
            int aIndex = (int)A.GetCard().Type * 1000 + A.GetCard().Index;
            int bIndex = (int)B.GetCard().Type * 1000 + B.GetCard().Index;
            if (aIndex > bIndex)
                return 1;
            else if (aIndex < bIndex)
                return -1;
            return 0;
        });
        for (int i = 0; i < deckCardObjects.Count; i++)
        {
            deckCardObjects[i].Index = i;
            deckCardObjects[i].Locate(i);
        }
        for (int i = 0; i < attainCardObjects.Count; i++)
        {
            attainCardObjects[i].Index = i;
            attainCardObjects[i].Locate(i);
        }
    }



    #endregion
}
