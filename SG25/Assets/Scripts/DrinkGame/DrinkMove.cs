using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkMove : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 플레이어인지 확인합니다.
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌하면 음료 미니게임 씬으로 넘어갑니다.
            SceneManager.LoadScene("DrinkMiniGameScene");
        }
    }
}
