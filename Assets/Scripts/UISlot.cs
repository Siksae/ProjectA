using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour , IDropHandler, IPointerEnterHandler, IPointerExitHandler  //ipointenter : pointer가 이미지에 걸칠 경우
{
    private Image img;
    private RectTransform rect; //스크린포트를 이용한 기준
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform); //드래그 해서 놓은 오브젝트는 자식이 됨
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

        }
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        img.color = Color.cyan;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        img.color = Color.white;
    }


    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>(); //GetComponet 내 위치의 컴포넌트를 불러옴
        rect = GetComponent<RectTransform>();
    }
}

