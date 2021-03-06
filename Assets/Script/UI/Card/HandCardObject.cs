﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandCardObject : CardObject, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private Hand hand;

    // Update is called once per frame
    private void Update()
    {
        EnableInteraction(data.IsAvailable());
    }

    public void SetHand(Hand _hand)
    {
        hand = _hand;
    }
    public bool IsAvailable()
    {
        return data.IsAvailable();
    }

    #region UserInput
    private const float handYOffset = -0.8f;
    private Vector3 originPos;
    private const int DragThreshold = 40;
    private const int ActiveThreshold = 200;
    public void OnPointerDown(PointerEventData ped)
    {
        hand.SetJoyStick(false);
        UIManager.instance.CardInfoPanel_On(data);
        data.CardEffectPreview();


        if (!GameManager.instance.IsInputOk || PlayerControl.instance.IsDirCardSelected || !IsAvailable())
        {
            return;
        }

        hand.ChooseOne();

        if (locateRoutine != null)
        {
            StopCoroutine(locateRoutine);
        }
        base.transform.rotation = Quaternion.identity;
    }

    public void OnPointerUp(PointerEventData ped)
    {
        UIManager.instance.CardInfoPanel_Off();
        hand.SetJoyStick(true);
        data.CancelPreview();

        if (!GameManager.instance.IsInputOk || PlayerControl.instance.IsDirCardSelected || !IsAvailable())
        {
            return;
        }

        hand.ChooseRollback();

        if (((Vector2)base.transform.localPosition - (Vector2)originPos).magnitude > ActiveThreshold && GameManager.instance.CurrentTurn == Turn.PLAYER && GameManager.instance.CurrentRoom().IsEnemyAlive() && IsAvailable())
        {
            hand.RemoveCard(this);
            ActiveSelf();
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = Vector3.one;
            //transform.localPosition = originPos;
            locateRoutine = StartCoroutine(LocateRoutine(originPos));
        }
    }

    public void OnDrag(PointerEventData ped)
    {

        if (!GameManager.instance.IsInputOk || PlayerControl.instance.IsDirCardSelected || !IsAvailable())
        {
            return;
        }

        Vector2 touchPos = ped.position;
        base.transform.position = touchPos;



        if (((Vector2)base.transform.localPosition - (Vector2)originPos).magnitude > DragThreshold)
        {
            transform.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        }
        else
        {
            transform.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    #endregion

    public void SetLocation(int total, int target, bool isHided)
    {
        if (locateRoutine != null)
        {
            StopCoroutine(locateRoutine);
        }
        originPos = GetPosition(total, target);
        //originRot = Quaternion.Euler (new Vector3 (0, 0, GetRotation (total, target)));

        if (isHided)
        {
            locateRoutine = StartCoroutine(LocateRoutine(new Vector3(0, handYOffset, -target * 0.5f)));

        }
        else
        {
            locateRoutine = StartCoroutine(LocateRoutine(originPos));
        }
    }

    public void DestroyFromHand()
    {
        hand.RemoveCard(this);
        Destroy(gameObject);
    }

    public void EnableInteraction(bool enable)
    {
        if (enable)
        {
            render.SetEnable(true);
        }
        else
        {
            render.SetEnable(false);
        }
    }

    #region Private
    private void ActiveSelf()
    {
        if (data.IsDirectionCard)
        {
            PlayerControl.instance.SelectedDirCard = data;
        }
        else
        {
            data.DoCard();
            if (data.IsConsumeTurn())
            {
                GameManager.instance.OnEndPlayerTurn();
            }
        }
    }

    private Coroutine locateRoutine;
    private IEnumerator LocateRoutine(Vector3 targetPosition)
    {

        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                base.transform.localPosition = targetPosition;
                //base.transform.rotation = targetRotation;
                transform.localScale = Vector3.one;
                break;
            }
            base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, targetPosition, 0.1f);
            //base.transform.rotation = Quaternion.Slerp (base.transform.rotation, targetRotation, 0.1f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.1f);
            yield return null;
        }
    }



    private static Vector3 GetPosition(int total, int target)
    {
        return new Vector3(-400 + 180 * (5 - total) + target * 185, handYOffset);
    }
    #endregion

}
