﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void LocateCallback();
public class CardObject : MonoBehaviour {
	private void Awake(){
		rendererParent = transform.Find ("Renderer");
		spriteRenderer = rendererParent.Find("Sprite").GetComponent<SpriteRenderer> ();
		collider = GetComponent<Collider2D> ();
		Transform canvas = rendererParent.Find ("Canvas");
		txt_name = canvas.Find ("Name").GetComponent<Text>();
		txt_explain = canvas.Find ("Explain").GetComponent<Text> ();
	}
	private Text txt_name;
	private Text txt_explain;

	private new Collider2D collider;
	private Transform rendererParent;
	private SpriteRenderer spriteRenderer;
	private CardData data;
	public void Init(CardData data_, Sprite sprite_){
		data = data_;
		spriteRenderer.sprite = sprite_;
		txt_name.text = data.CardName;
		txt_explain.text = data.CardExplain;
	}

	public void Active(){
		data.CardActive ();
	}

	public bool IsAvailable(){
		return data.IsAvailable ();
	}


	#region UserInput
	private const float handYOffset = -3.5f;
	private Vector3 originPos;
	private Quaternion originRot;
	private const int DragThreshold = 2;
	private const int ActiveThreshold = 4;
	void OnMouseDown(){
		data.CardEffectPreview ();

		if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}
		transform.rotation = Quaternion.identity;
	}

	void OnMouseUp(){
		data.CancelPreview ();

		if (((Vector2)transform.localPosition - (Vector2)originPos).magnitude > ActiveThreshold) {
			//TODO : Active Card
			Debug.Log("Active!");
		} else {
			rendererParent.localScale = Vector3.one;
			rendererParent.localPosition = Vector3.zero;
			locateRoutine = StartCoroutine (LocateRoutine (originPos, originRot, null));
		}
	}

	void OnMouseDrag(){
		Vector2 touchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		transform.position = touchPos;

		if (((Vector2)transform.localPosition - (Vector2)originPos).magnitude > DragThreshold) {
			rendererParent.transform.localScale = Vector3.one;
			rendererParent.transform.localPosition = Vector3.zero;
		} else {
			rendererParent.transform.localScale = new Vector3 (2.5f, 2.5f, 1f);
			Vector3 temp = originPos;
			Debug.Log(temp);
			temp.y = 0;
			temp.z = -5f;
			rendererParent.transform.localPosition = temp - transform.localPosition;
		}
	}
	#endregion

	public void SetLocation(int total, int target, bool isHided){
		if (locateRoutine != null) {
			StopCoroutine (locateRoutine);
		}
		originPos = GetPosition (total, target);
		originRot = Quaternion.Euler (new Vector3 (0, 0, GetRotation (total, target)));

		if (isHided) {
			locateRoutine = StartCoroutine (LocateRoutine (new Vector3 (0, handYOffset, -target * 0.5f), Quaternion.identity, null));
			DisableInteraction ();
		} else {
			locateRoutine = StartCoroutine (LocateRoutine (originPos, originRot, EnableInteraction));
		}
	}

	private void HideCard(int target){
		
		Vector3 targetPos = originPos;
		targetPos.x = 0;
		targetPos.y = -2.5f;

	}

	private void EnableInteraction(){
		collider.enabled = true;
	}
	private void DisableInteraction(){
		collider.enabled = false;
	}

	#region Private
	private Coroutine locateRoutine;
	private IEnumerator LocateRoutine(Vector3 targetPosition, Quaternion targetRotation, LocateCallback callBack){

		float timer = 0;
		while (true) {
			timer += Time.deltaTime;
			if (timer > 1) {
				transform.localPosition = targetPosition;
				transform.rotation = targetRotation;
				break;
			}
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPosition, 0.1f);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 0.1f);
			yield return null;
		}

		if (callBack != null) {
			callBack ();
		}
	}

	private const float maxRotation = 30f;
	private static float GetRotation(int total, int target){
		if (total < 4) {
			return 0;
		} else {
			return 0.5f * maxRotation - (maxRotation / (total - 1)) * target;
		}
	}
		
	private static Vector3 GetPosition(int total, int target){
		if (total == 1) {
			return new Vector3 (0, handYOffset, 0);
		}
		float totalInterval = 0.3f + total * 0.4f;
		Vector3 result = new Vector3 ();
		result.x = -totalInterval + (totalInterval * 2 / (total - 1)) * target;
		result.y = Mathf.Sqrt(100 - result.x * result.x) - 10f + handYOffset;
		result.z = -target * 0.2f;
		return result;
	}
	#endregion
}
