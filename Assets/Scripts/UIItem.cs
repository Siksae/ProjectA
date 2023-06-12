using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Transform canvas;
    private Transform beforeParent;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Image img;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        beforeParent = transform.parent;
        transform.SetParent(canvas);
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas)
        {
            transform.SetParent(beforeParent);
            rect.position = beforeParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(Sprite _spr)
    {
        img = GetComponent<Image>();
        img.sprite = _spr;
        img.SetNativeSize(); //화면 맞추기
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
