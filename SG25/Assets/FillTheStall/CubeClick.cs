using UnityEngine;

public class CubeClick : MonoBehaviour
{
    public GameObject myItem; // ������ ������ ������Ʈ

    public bool hasItem = false; // �� ������ �������� ������ �ִ°�

    public void GetItem(GameObject target)
    {
        myItem = target;
        // ť���� ��ġ�� �����ɴϴ�.
        Vector3 cubePosition = transform.position;

        // ������ ������Ʈ�� ��ġ�� ť�� ��ġ�� �����մϴ�.
        myItem.transform.position = cubePosition;

        // ������ ������Ʈ�� �̸��� �����մϴ�.
        myItem.name = "NewPrefabObject";

        // �ֿܼ� ������ ������Ʈ�� �̸��� ����մϴ�.
        Debug.Log("Object instantiated: " + myItem.name);

        // ���Կ� �������� �ִٰ� �ٲߴϴ�.
        hasItem = true;
    }

    public GameObject GiveItem()
    {
        if(myItem != null)
        {
            hasItem = false;
            // �������� �ݶ��̴��� ��Ȱ��ȭ �մϴ�
            myItem.GetComponent<Collider>().enabled = hasItem;

            // �� ���Կ� �������� �ٽ� ���� �� �ְ� Ȱ��ȭ �մϴ�.
            this.GetComponent<Collider>().enabled = !hasItem;

            return myItem;
        }
        else
        {
            return null;
        }
    }
}
