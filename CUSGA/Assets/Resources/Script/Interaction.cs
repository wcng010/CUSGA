using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;

    public GameObject chatFrame;
    [Header("头像")]
    public Sprite thisFace;
    [Header("获取道具")]
    public string ObjName;
    public bool IsObj;

    [Header("对话数据文本")]
    public TextAsset[] textFile;

    [HideInInspector]
    public int index = 0;

    private List<ObjectData> dataList;

    void Start()
    {
        index = 0;
        sprite = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        InteractionObj();
    }

    void InteractionObj()
    {
        if (IsObj && ObjName != null)
        {
            if (sprite.enabled && Input.GetKeyDown(KeyCode.F))
            {
                dataList = BagManager.Instance.DataListClass.ObjectList;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i] != null && dataList[i].ObjectNames == ObjName.ToString())
                    {
                        dataList[i].ObjectNum++;
                        if (transform.parent != null)
                            Destroy(transform.parent.gameObject);
                        break;
                    }

                }
            }
        }
    }

    public void InteractionChat()
    {
        //与人物交互
        if (sprite != null && !IsObj)
        {
            //没有激活的情况下，同时玩家接触按R
            if (sprite.enabled && Input.GetKeyDown(KeyCode.F) && !chatFrame.activeInHierarchy)
            {
                if (index >= textFile.Length)
                    return;

                DialogSystem.Instance.otherFace = thisFace;

                DialogSystem.Instance.GetTextFromFile(textFile[index]);//读取文件内容
                chatFrame.SetActive(true);//显示对话框
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsObj)
        {
            if (collision.tag == "Player")
                sprite.enabled = true;
        }
        else if(collision.tag == "Player" && index < textFile.Length)
            sprite.enabled = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            sprite.enabled = false;
    }
}
