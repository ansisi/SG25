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
        float fps = 1.0f / deltaTime;//한프레임으로 나눠서 fps가 나온다.=3바퀴 돈다. 
        Debug.Log(fps);
        //cpu가 400번 도는데
        fpsText.text = fps.ToString();
    }


}
