using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour , IDropHandler, IPointerEnterHandler, IPointerExitHandler  //ipointenter : pointer�� �̹����� ��ĥ ���
{
    private Image img;
    private RectTransform rect; //��ũ����Ʈ�� �̿��� ����
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform); //�巡�� �ؼ� ���� ������Ʈ�� �ڽ��� ��
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
        img = GetComponent<Image>(); //GetComponet �� ��ġ�� ������Ʈ�� �ҷ���
        rect = GetComponent<RectTransform>();
    }
}

