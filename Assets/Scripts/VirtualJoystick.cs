using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [NonSerialized] public Vector2 constantOrigin;
    [NonSerialized] public Vector2 currOrigin;
    [NonSerialized] public Vector2 touchPos;
    public Image background;
    public Image controller;
    [Range(0f, 0.5f)]
    public float dragSensitive;
    public UnityEvent OnStickDown;
    public UnityEvent<Vector3> OnStickDrag;
    public UnityEvent<Vector3, float> OnStickUp;

    private void Start()
    {
        constantOrigin = background.transform.position;
        currOrigin = constantOrigin;
    }

    public void SetOrigin(Vector2 touchPos)
    {
        currOrigin = touchPos - new Vector2(background.rectTransform.sizeDelta.x * 0.5f, background.rectTransform.sizeDelta.y * 0.5f); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnStickDown != null)
            OnStickDown.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background.rectTransform, eventData.position, eventData.pressEventCamera, out touchPos))
        {
            touchPos.x = touchPos.x / background.rectTransform.sizeDelta.x * 2f;
            touchPos.y = touchPos.y / background.rectTransform.sizeDelta.y * 2f;
            touchPos = touchPos.magnitude > 1 ? touchPos.normalized : touchPos;

            controller.rectTransform.anchoredPosition = new Vector2
                (touchPos.x * background.rectTransform.sizeDelta.x * 0.5f, touchPos.y * background.rectTransform.sizeDelta.y * 0.5f);
        }
        Vector3 touchPosV3 = new Vector3(touchPos.x, 0f, touchPos.y);
        if (OnStickDrag != null)
            OnStickDrag.Invoke(touchPosV3);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 touchPosV3 = new Vector3(touchPos.x, 0f, touchPos.y);
        if (OnStickUp != null)
            OnStickUp.Invoke(touchPosV3.normalized, touchPosV3.magnitude);
        touchPos = Vector2.zero;
        controller.rectTransform.anchoredPosition = Vector2.zero;
        currOrigin = constantOrigin;
    }
}
