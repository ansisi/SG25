using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScene : MonoBehaviour
{
    void Start()
    {
        // DrinkMiniGameScene에서는 마우스 커서를 보이도록 설정
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // 마우스 커서가 화면 내에서 자유롭게 움직일 수 있도록 설정
    }
}
