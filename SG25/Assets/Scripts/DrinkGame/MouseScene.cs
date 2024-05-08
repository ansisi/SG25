using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScene : MonoBehaviour
{
    void Start()
    {
        // ������ ���۵� �� ���콺 Ŀ���� ���̰� �մϴ�.
        Cursor.visible = true;
    }

    void OnDestroy()
    {
        // ���� ������Ʈ�� ���ŵ� �� ���콺 Ŀ���� �ٽ� ����ϴ�.
        Cursor.visible = false;
    }
}
