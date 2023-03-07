using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ohters : MonoBehaviour
{
    public string[] needStrings;
    private Interaction inter;

    private int succeed;
    private bool have;
    private bool one;//已经满足需求，只执行一次
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
        if (inter.sprite.enabled && !have)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                dataList = BagManager.Instance.DataListClass.ObjectList;
                for (int i = 0; i < needStrings.Length; i++)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        if (dataList[j] != null && dataList[j].ObjectNames == needStrings[i])
                        {
                            if (dataList[i].ObjectNum > 0)
                            {
                                succeed++;
                                break;
                            }
                        }

                    }

                }
            }
        }

        if (succeed >= needStrings.Length && !one)
        {
            have = true;
            inter.index++;
            one = true;
        }
    }
}
