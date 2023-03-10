using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class BagManager : Singleton<BagManager>
{
    [Header("信息Data数组")]
    public ListData DataListClass;
    [Header("背包格子Grid")]
    public GameObject plaidGrid;
    [Header("合成台格子Grid")]
    public GameObject WorkGrid;
    [Header("交换台格子Grid")]
    public GameObject exchangeGrid;
    [Header("物品栏格子Grid")] 
    public GameObject InventoryGrid;
    [Header("格子Brush预设体")]
    public GameObject plaid;
    [Header("格子笔画数组")]
    public List<GameObject> brushList=new List<GameObject>();
    [Header("格子Object预设体")] 
    public GameObject plaid_Object;
    [Header("格子道具数组")]
    public List<GameObject> ObjectList = new List<GameObject>();
    [Header("防遮罩坐标")] 
    public Transform EndTransform;
    [Header("背包笔画合成台分界线")] 
    public int boundary_workbag;
    [Header("背包兑换台分界线")] 
    public int boundary_exchange;
    [Header("背包物品栏分界线")] 
    public int boundary_Inventory;
    [Header("合成检测数组")] 
    private string[] CheckStrings = new string[9];
    private String CheckString;

    

    public void RefreshBrush()
    {
       BagClear();
       brushList.Clear();
       for (int i = 0; i<DataListClass.BrushList.Count; i++)
        {
            brushList.Add(Instantiate(plaid));
            brushList[i].GetComponent<Plaid_UI>().plaid_ID = i;
            brushList[i].GetComponent<Plaid_UI>().InitPlaid(DataListClass.BrushList[i]);
            if (i <= boundary_workbag)
            {
                brushList[i].transform.SetParent(Instance.plaidGrid.transform);
            }
            else if(i<=boundary_exchange)
            {
                brushList[i].transform.SetParent(Instance.WorkGrid.transform);
            }
            else
            {
                brushList[i].transform.SetParent(Instance.exchangeGrid.transform);
            }
        }
    }
    

    public void RefreshObject()
    {
        BagClear();
        ObjectList.Clear();
        for (int i = 0; i<DataListClass.ObjectList.Count; i++)
        {
            
            ObjectList.Add(Instantiate(plaid_Object));
            ObjectList[i].GetComponent<Object_UI>().Object_ID = i;
            ObjectList[i].GetComponent<Object_UI>().InitObject(DataListClass.ObjectList[i],i);
            if(i<boundary_Inventory)
            {
                ObjectList[i].transform.SetParent(plaidGrid.transform);
            }
            else if (i == boundary_Inventory)
            {
                ObjectList[i].transform.SetParent(WorkGrid.transform);
            }
            else if (i > boundary_Inventory)
            {
                ObjectList[i].transform.SetParent(InventoryGrid.transform);
            }
        }
    }
    
    public void BagClear()
    {
        for (int i = 0; i < WorkGrid.transform.childCount; i++)
        {
            Destroy(WorkGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < plaidGrid.transform.childCount; i++) //遍历所有格子
        {
            Destroy(plaidGrid.transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < exchangeGrid.transform.childCount; i++) //遍历所有格子
        {
            Destroy(exchangeGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < InventoryGrid.transform.childCount; i++)
        {
            Destroy(InventoryGrid.transform.GetChild(i).gameObject);
        }

        Debug.Log("背包清空");
    }

    public void ExitClear()
    {
        BagClear();
        brushList.Clear();
        ObjectList.Clear();
    }
    /// <summary>
    /// 遍历所有data道具，使得匹配道具数量加1.
    /// 存在问题：一开始没有某个道具。
    /// </summary>
    public void Synthesis_Brush()
    {
        for (int i = boundary_workbag+1,j=0; i < boundary_exchange+1&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++,j++)
        {
            CheckStrings[j] = brushList[i].GetComponent<Plaid_UI>().brushName_item;
        }

        CheckString = String.Concat(CheckStrings);
        foreach (var item in DataListClass.ObjectList)
        {
            if (item != null && item.Brush_composition == CheckString)
            {
                item.ObjectNum++;
                CorrectionFor_01B(boundary_workbag+1,boundary_exchange+1);
                RefreshBrush();
                return;
            }
        }
        Debug.Log("合成失败");
    }
    /// <summary>
    /// 遍历所有data笔画让每个笔画数加1
    /// </summary>
    public void Decompose()
    {
        if (ObjectList[boundary_Inventory].GetComponent<Object_UI>().IsActive)
        {
            for (int i = 0;i<DataListClass.ObjectList[boundary_Inventory].Brush_composition.Length; i++)
            {
                for (int j = 0; j < DataListClass.BrushList.Count; j++)
                {
                    if (DataListClass.ObjectList[boundary_Inventory].Brush_composition[i].ToString() ==
                        DataListClass.BrushList[j]._brushName)
                    {
                        
                        Debug.Log(DataListClass.BrushList[j]._brushName+"数量加一");
                        DataListClass.BrushList[j]._brushNum++;
                        break;
                    }
                }
            }
            if (DataListClass.ObjectList[boundary_Inventory].ObjectNum == 1&&!CorrectionFor_12O(DataListClass.ObjectList[boundary_Inventory].ObjectNames))
            {
                for (int i = 0; i <boundary_workbag ; i++)
                {
                    if (DataListClass.ObjectList[i] == null)
                    {
                        DataListClass.ObjectList[i] = DataListClass.ObjectList[boundary_Inventory];
                        DataListClass.ObjectList[i].ObjectNum--;
                        break;
                    }
                }
            }
            DataListClass.ObjectList[boundary_Inventory] = null;
            BagManager.Instance.RefreshObject();
        }
        else//分解失败
        {
            Debug.Log("分解失败");
        }
    }
    
    /// <summary>
    /// 将两个笔画交换为一个笔画
    /// </summary>
    public void Exchange()
    {
        if (brushList[boundary_exchange + 1].GetComponent<Plaid_UI>().IsActive == true &&
            brushList[boundary_exchange + 2].GetComponent<Plaid_UI>().IsActive == true &&
            brushList[boundary_exchange + 3].GetComponent<Plaid_UI>().IsActive == true)
        {
            DataListClass.BrushList[boundary_exchange+3]._brushNum++;
            CorrectionFor_01B(boundary_exchange+1,boundary_exchange+3);
            RefreshBrush();
        }
        else
        {
            Debug.Log("兑换失败");
        }
        
    }
    
    /// <summary>
    /// 数量0，1的处理
    /// </summary>
    /// <param name="FirstIndex"></param>
    /// <param name="EndIndex"></param>
    public void CorrectionFor_01B(int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex; i < EndIndex&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++)
        {
            if (DataListClass.BrushList[i]._brushNum == 1&&!CorrectionFor_12B(DataListClass.BrushList[i]._brushName))
            {
                for (int j = 0; j <= boundary_workbag; j++)
                {
                    if (DataListClass.BrushList[j] == null)
                    {
                        DataListClass.BrushList[j] = DataListClass.BrushList[i];
                        break;
                    }
                }
                DataListClass.BrushList[i]._brushNum--;
            }
            DataListClass.BrushList[i] = null;
        }
        RefreshBrush();
    }

    public void CorrectionFor_01O(int ClickItemID)
    {
        if (DataListClass.ObjectList[ClickItemID].ObjectNum == 1&&!CorrectionFor_12O(DataListClass.ObjectList[ClickItemID].ObjectNames))
        {
            for(int i=0;i<boundary_workbag;i++)
                if (DataListClass.ObjectList[i] == null)
                {
                    DataListClass.ObjectList[i] = DataListClass.ObjectList[ClickItemID];
                    DataListClass.ObjectList[ClickItemID] = null;
                    RefreshObject();
                    return;
                }
        }
    }


    /// <summary>
    /// true为数量为2的情况，false为数量为1的情况
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CorrectionFor_12B(string name)
    {
        for (int i = 0; i <= boundary_workbag; i++)
        {
            if (DataListClass.BrushList[i]!=null&&name == DataListClass.BrushList[i]._brushName)
            {
                return true;
            }
        }
        return false;
    }


    public bool CorrectionFor_12O(string name)
    {
        for (int i = 0; i < boundary_Inventory; i++)
        {
            if (DataListClass.ObjectList[i] != null && name == DataListClass.ObjectList[i].ObjectNames)
            {
                return true;
            }
        }

        return false;
    }

    public void DeleteSameObject(int MyID,int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex; i < EndIndex; i++)
        {
            if (DataListClass.ObjectList[i] != null &&
                DataListClass.ObjectList[i].ObjectNames == DataListClass.ObjectList[MyID].ObjectNames&&i!=MyID)
            {
                DataListClass.ObjectList[i].ObjectNum++;
                DataListClass.ObjectList[i] = null;
            }
        }
    }

    public void ChangeCorrection()
    {
        if (DataListClass.BrushList[boundary_exchange+1]!=null&&
            DataListClass.BrushList[boundary_exchange+3]!=null&&
            (DataListClass.BrushList[boundary_exchange + 1]._brushName ==
             DataListClass.BrushList[boundary_exchange + 3]._brushName))
        {
            DataListClass.BrushList[boundary_exchange + 3]._brushNum++;
            DataListClass.BrushList[boundary_exchange + 3] = null;
        }

        if (DataListClass.BrushList[boundary_exchange + 2] != null &&
            DataListClass.BrushList[boundary_exchange + 3] != null &&
            (DataListClass.BrushList[boundary_exchange + 2]._brushName ==
             DataListClass.BrushList[boundary_exchange + 3]._brushName))
        {
            DataListClass.BrushList[boundary_exchange + 3]._brushNum++;
            DataListClass.BrushList[boundary_exchange + 3] = null;
        }
    }


    private void Start()
    {
        RefreshObject();
    }
}
