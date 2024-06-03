using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField]
    private float deltaTime = 0.0f;
    public TMP_Text fpsText;


    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;//������������ ������ fps�� ���´�.=3���� ����. 
        Debug.Log(fps);
        //cpu�� 400�� ���µ�
        fpsText.text = fps.ToString();
    }


}
