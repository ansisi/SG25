using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Shelf;

    [System.Serializable]
    public class ShelfItem
    {
        public Item item;
        public int index;
        public int count;
        public GameObject itemPrefab;

        public ShelfItem(Item item, int index, int count, GameObject itemPrefab)
        {
            this.item = item;
            this.index = index;
            this.count = count;
            this.itemPrefab = itemPrefab;
        }
    }
public class CubeClick : MonoBehaviour
{


    public GameObject myItem; // ������ ������ ������Ʈ

    private List<ShelfItem> items = new List<ShelfItem>();

    [SerializeField]
    private ShelfItem getItem;

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

        items.Add(getItem);

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

            items.Remove(getItem);

            return myItem;
        }
        else
        {
            return null;
        }
    }
}
