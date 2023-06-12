using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour

{
    private InventoryManager inven;
    [SerializeField] SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        inven = InventoryManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetItem()
    {
        if (inven.GetItem(spr.sprite) == true)//�κ��丮 �Ŵ��� �������� ���� ������ �ֳ���?A
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("������â�� ���� ��");
        }
    }
}
