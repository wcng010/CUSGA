using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ohters<T> : MonoBehaviour where T : class
{
    private static T instance;
    public static T Instance => instance;

    public string[] needStrings;

    protected Collider2D coll;
    protected SpriteRenderer spr;
    public Interaction inter;

    protected int succeed;
    protected bool close = false;
    protected List<ObjectData> dataList;

    protected virtual void Awake()
    {
        instance = this as T;
    }

    public virtual void Start()
    {
        succeed = 0;
        inter = GetComponentInChildren<Interaction>();
        coll = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
    }
    public virtual void FindneedObject()
    {
        //����Ҫ���� ����û�гɹ��� �����û�г���
        if (inter.sprite.enabled && !close && !inter.chatFrame.activeInHierarchy)
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
                                break;
                            }
                        }

                    }

                }
            }

            if (succeed >= needStrings.Length)
            {
                //�ҵ�ȫ�����ܼ���
                for (int i = 0; i < needStrings.Length; i++)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        if (dataList[j] != null && dataList[j].ObjectNames == needStrings[i].ToString())
                        {
                            if (dataList[j].ObjectNum > 0)
                            {
                                dataList[j].ObjectNum--;
                                break;
                            }
                        }

                    }

                }

                inter.index++;
                close = true;
            }

        }

        
    }

    public virtual void ShowObject()
    {
        coll.isTrigger = true;
        spr.enabled = true;
        inter.index++;
    }
}
