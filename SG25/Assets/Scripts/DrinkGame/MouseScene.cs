using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScene : MonoBehaviour
{
    void Start()
    {
        // 게임이 시작될 때 마우스 커서를 보이게 합니다.
        Cursor.visible = true;
    }

    void OnDestroy()
    {
        // 게임 오브젝트가 제거될 때 마우스 커서를 다시 숨깁니다.
        Cursor.visible = false;
    }
}
