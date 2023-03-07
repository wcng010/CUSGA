using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    private static DialogSystem instance;
    public static DialogSystem Instance => instance;

    [Header("UI组件")]
    public TextMeshProUGUI textLabel;
    public Image faceImage;

    [Header("字显示间隔时间")]
    public float textSpeed = 0.1f;

    private int index;
    private bool textFinish;

    [HideInInspector]
    public Sprite otherFace;
    [Header("玩家头像")]
    public Sprite playerFace;

    private List<string> textList = new List<string>();

    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Face();
        StartCoroutine(SetTextUI());
    }

    void Update()
    {
        //对话没有完成
        if(Input.GetKeyDown(KeyCode.R) && index != textList.Count)
        {
            Face();

            if (textFinish)
            StartCoroutine(SetTextUI());
            else
            {
                StopCoroutine(SetTextUI());
                textLabel.text = textList[index].ToString();
                index++;
                textFinish = true;
            }
        }
        if(index == textList.Count) //对话结束
        {
            this.gameObject.SetActive(false);
            return;
        }
    }

    public void GetTextFromFile(TextAsset file)
    {
        //先清空
        textList.Clear();
        index = 0;

        //按行切割
        string[] lineData = file.text.Split('\n');

        foreach (string line in lineData)
        {
            //读取文件
            textList.Add(line);
        }
    }

    /// <summary>
    /// 获取头像
    /// </summary>
    void Face()
    {
        switch (textList[index].Trim().ToString())
        {
            case "A":
                if (otherFace != null)
                    faceImage.sprite = otherFace;
                index++;
                break;
            case "B":
                if (playerFace != null)
                    faceImage.sprite = playerFace;
                index++;
                break;
            default:
                break;
        }       
    }

    IEnumerator SetTextUI()
    {
        textFinish = false;
        textLabel.text = "";
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i].ToString();
            yield return new WaitForSeconds(textSpeed);
        }
        textFinish = true;
        index++;
    }
}
