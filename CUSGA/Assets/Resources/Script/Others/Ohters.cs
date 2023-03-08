using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ohters : MonoBehaviour
{
    public string[] needStrings;
    private Interaction inter;

    private int succeed;
    private List<ObjectData> dataList;
    void Start()
    {
        succeed = 0;
        inter = GetComponentInChildren<Interaction>();
    }

    void Update()
    {
        if(inter == null)
            return;

       FindneedObject();
    }

    private void FindneedObject()
    {
        if (inter.sprite.enabled)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                dataList = BagManager.Instance.DataListClass.ObjectList;
                for (int i = 0; i < needStrings.Length; i++)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        if (dataList[j] != null && dataList[j].ObjectNames == needStrings[i].ToString())
                        {
                            if (dataList[j].ObjectNum > 0)
                            {
                                succeed++;
                                dataList[j].ObjectNum--;
                                break;
                            }
                        }

                    }

                }
            }
        }

        if (succeed >= needStrings.Length)
        {
            inter.index++;
            this.enabled = false;
        }
    }
}
