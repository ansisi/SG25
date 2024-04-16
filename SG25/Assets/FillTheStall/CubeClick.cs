using UnityEngine;

public class CubeClick : MonoBehaviour
{
    public GameObject prefabToInstantiate; // 생성할 프리팹 오브젝트
    public GameObject shelfPrefab; // CSSfinal2 진열대 프리팹

    private void OnMouseDown()
    {
        // 큐브의 위치를 가져옵니다.
        Vector3 cubePosition = transform.position;

        // 프리팹 오브젝트를 생성하고 진열대 프리팹의 자식으로 설정합니다.
        GameObject newObject = Instantiate(prefabToInstantiate, shelfPrefab.transform);

        // 생성된 오브젝트의 위치를 큐브 위치로 설정합니다.
        newObject.transform.position = cubePosition;

        // 생성된 오브젝트의 이름을 변경합니다.
        newObject.name = "NewPrefabObject";

        // 콘솔에 생성된 오브젝트의 이름을 출력합니다.
        Debug.Log("Object instantiated: " + newObject.name);
    }
}
