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
    [Header("背包格子Grid预设体")]
    public GameObject plaidGrid;
    [Header("合成台格子Grid预设体")]
    public GameObject WorkGrid;
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
    [Header("背包合成台分界线")] 
    public int boundary_workbag;
    [NonSerialized]
    public int End_element;
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
            brushList[i].GetComponent<Plaid_UI>().InitPlaid(DataListClass.BrushList[i],i);
            if (i <= boundary_workbag)
            {
                brushList[i].transform.SetParent(Instance.plaidGrid.transform);
            }
            else
            {
                brushList[i].transform.SetParent(Instance.WorkGrid.transform);
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
            if (i == DataListClass.ObjectList.Count-1)
            {
                ObjectList[i].transform.SetParent(WorkGrid.transform);
            }
            else
            {
                ObjectList[i].transform.SetParent(plaidGrid.transform);
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
        for (int i = boundary_workbag+1,j=0; i < brushList.Count&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++,j++)
        {
            CheckStrings[j] = brushList[i].GetComponent<Plaid_UI>().brushName_item;
        }

        CheckString = String.Concat(CheckStrings);
        foreach (var item in DataListClass.ObjectList)
        {
            if (item != null && item.Brush_composition == CheckString)
            {
                item.ObjectNum++;
                for (int i = boundary_workbag+1; i < brushList.Count&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++)
                {
                    if (DataListClass.BrushList[i]._brushNum == 1)
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
                return;
            }
            else//合成失败
            {
                Debug.Log("合成失败");
            }
        }

    }
    /// <summary>
    /// 遍历所有data笔画让每个笔画数加1
    /// 一开始没有某个笔画
    /// </summary>
    public void Decompose()
    {
        if (ObjectList[End_element].GetComponent<Object_UI>().IsActive)
        {
            for (int i = 0;i<DataListClass.ObjectList[End_element].Brush_composition.Length; i++)
            {
                for (int j = 0; j < DataListClass.BrushList.Count; j++)
                {
                    if (DataListClass.ObjectList[End_element].Brush_composition[i].ToString() ==
                        DataListClass.BrushList[j]._brushName)
                    {
                        
                        Debug.Log(DataListClass.BrushList[j]._brushName+"数量加一");
                        DataListClass.BrushList[j]._brushNum++;
                        break;
                    }
                }
            }
            if (DataListClass.ObjectList[End_element].ObjectNum == 1)
            {
                for (int i = 0; i <boundary_workbag ; i++)
                {
                    if (DataListClass.ObjectList[i] == null)
                    {
                        DataListClass.ObjectList[i] = DataListClass.ObjectList[End_element];
                        DataListClass.ObjectList[i].ObjectNum--;
                        break;
                    }
                }
            }
            DataListClass.ObjectList[End_element] = null;
            BagManager.Instance.RefreshObject();
        }
        else//分解失败
        {
            Debug.Log("分解失败");
        }
    }

    public void Start()
    {
        End_element = DataListClass.ObjectList.Count - 1;
    }
}
