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

    [Header("对话数据文本")]
    public TextAsset[] textFile;

    [HideInInspector]
    public int index = 0;

    void Start()
    {
        index = 0;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //与人物交互
        if(sprite != null && textFile != null)
        {
            //没有激活的情况下，同时玩家接触按R
            if(sprite.enabled && Input.GetKeyDown(KeyCode.R) && !chatFrame.activeInHierarchy)
            {
                if(index > textFile.Length)
                    index = textFile.Length - 1;

                if(thisFace != null) //获取该人物头像
                DialogSystem.Instance.otherFace = thisFace;

                DialogSystem.Instance.GetTextFromFile(textFile[index]);//读取文件内容
                chatFrame.SetActive(true);//显示对话框
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        sprite.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            sprite.enabled = false;
    }
}
