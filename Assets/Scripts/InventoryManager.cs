using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject m_objInventory;
    [SerializeField] private Transform m_trsInven;
    [SerializeField] private GameObject m_objItem;
    private List<Transform> m_listInventory = new List<Transform>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        initInventory();
    }
    private void initInventory()
    {
        m_listInventory.AddRange(m_trsInven.GetComponentsInChildren<Transform>());
        m_listInventory.RemoveAt(0);
        if (m_objInventory.activeSelf == true)
        {
            m_objInventory.SetActive(false);
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        callInventory();
    }
    
    private void callInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (m_objInventory.activeSelf == true)
            {
                m_objInventory.SetActive(false);
            }
            else
            {
                m_objInventory.SetActive(true);
            }
        }
    }

    public bool GetItem(Sprite _spr)
    {
        int slotNum = getEmptyItemSlot();
        if (slotNum == -1)
        {
            return false;
        }
        GameObject obj = Instantiate(m_objItem, m_listInventory[slotNum]);
        UIItem uIItem = obj.GetComponent<UIItem>();
        uIItem.SetItem(_spr);
        return true;
    }

    private int getEmptyItemSlot()
    {
        int count = m_listInventory.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (m_listInventory[iNum].childCount == 0)
            {
                return iNum;
            }
        }return -1;
    }
}
