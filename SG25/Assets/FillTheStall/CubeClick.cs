using UnityEngine;

public class CubeClick : MonoBehaviour
{
    public GameObject prefabToInstantiate; // ������ ������ ������Ʈ
    public GameObject shelfPrefab; // CSSfinal2 ������ ������

    private void OnMouseDown()
    {
        // ť���� ��ġ�� �����ɴϴ�.
        Vector3 cubePosition = transform.position;

        // ������ ������Ʈ�� �����ϰ� ������ �������� �ڽ����� �����մϴ�.
        GameObject newObject = Instantiate(prefabToInstantiate, shelfPrefab.transform);

        // ������ ������Ʈ�� ��ġ�� ť�� ��ġ�� �����մϴ�.
        newObject.transform.position = cubePosition;

        // ������ ������Ʈ�� �̸��� �����մϴ�.
        newObject.name = "NewPrefabObject";

        // �ֿܼ� ������ ������Ʈ�� �̸��� ����մϴ�.
        Debug.Log("Object instantiated: " + newObject.name);
    }
}
