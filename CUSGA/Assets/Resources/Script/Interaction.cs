using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;

    public GameObject chatFrame;
    [Header("ͷ��")]
    public Sprite thisFace;

    [Header("�Ի������ı�")]
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
        //�����ｻ��
        if(sprite != null && textFile != null)
        {
            //û�м��������£�ͬʱ��ҽӴ���R
            if(sprite.enabled && Input.GetKeyDown(KeyCode.R) && !chatFrame.activeInHierarchy)
            {
                if(index > textFile.Length)
                    index = textFile.Length - 1;

                if(thisFace != null) //��ȡ������ͷ��
                DialogSystem.Instance.otherFace = thisFace;

                DialogSystem.Instance.GetTextFromFile(textFile[index]);//��ȡ�ļ�����
                chatFrame.SetActive(true);//��ʾ�Ի���
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
