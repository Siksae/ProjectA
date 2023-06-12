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
        if (inven.GetItem(spr.sprite) == true)//인벤토리 매니저 아이템을 먹을 공간이 있나요?A
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("아이템창이 가득 참");
        }
    }
}
