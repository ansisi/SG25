using UnityEngine;

public class InteractableSpace : MonoBehaviour
{
    // 아이템 생성 위치
    public Transform spawnPoint;

    // 아이템 프리팹 배열
    public GameObject[] itemPrefabs;

    // 생성할 아이템 확률 배열 (총 확률 100% 여야 함)
    [Range(0.0f, 1.0f)]
    public float[] itemProbabilities;

    // 아이템 생성 여부 플래그
    private bool isItemSpawned = false;

    // 진열대 오브젝트 (아이템 보관 용도)
    public GameObject displayObject;

    // 진열대에 보여질 아이템 프리팹
    public GameObject displayItemPrefab;

    // 진열대에 보여질 아이템 개수
    public int displayItemCount = 3;

    void Start()
    {
        // 진열대 오브젝트 설정
        if (displayObject == null)
        {
            displayObject = transform.Find("DisplayObject").gameObject;
        }

        // 진열대 아이템 초기화
        UpdateDisplayItems();
    }

    void Update()
    {
        // 공간 클릭 시 아이템 생성
        if (Input.GetMouseButtonDown(0) && !isItemSpawned)
        {
            SpawnItem();
            isItemSpawned = true;
        }
    }

    // 아이템 생성 함수
    private void SpawnItem()
    {
        // 랜덤 확률 기반으로 아이템 인덱스 선택
        int itemIndex = RandomSelectionBasedOnProbability(itemProbabilities);

        // 아이템 프리팹 인스턴스 생성
        GameObject itemInstance = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnPoint.rotation);

        // 아이템 태그 설정
        itemInstance.tag = "Item";

        // 아이템 스크립트 초기화 (필요한 경우)
        Consumable consumable = itemInstance.GetComponent<Consumable>();
        if (consumable != null)
        {
            // 아이템 정보 설정
            consumable.item = new Item(); // 아이템 정보 설정 코드 추가
        }

        // 진열대 아이템 업데이트
        UpdateDisplayItems();
    }

    // 진열대 아이템 업데이트 함수
    private void UpdateDisplayItems()
    {
        // 기존 진열대 아이템 제거
        foreach (Transform child in displayObject.transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 진열대 아이템 생성
        for (int i = 0; i < displayItemCount; i++)
        {
            GameObject displayItemInstance = Instantiate(displayItemPrefab, displayObject.transform);
            displayItemInstance.transform.localPosition = new Vector3(i * 0.5f, 0, 0); // 간격 조정
        }
    }

    // 랜덤 선택 함수 (확률 기반)
    private int RandomSelectionBasedOnProbability(float[] probabilities)
    {
        float totalProbability = 0.0f;
    foreach (float probability in probabilities)
        {
            totalProbability += probability;
        }

        float randomValue = Random.value * totalProbability;
        float accumulatedProbability = 0.0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            accumulatedProbability += probabilities[i];
            if (randomValue < accumulatedProbability)
            {
                return i; // 여기 return 문 추가
            }
        }

        // 모든 확률을 다 사용하지 못한 경우 (예: 확률 합이 1.0보다 작음)
        Debug.LogError("오류: 모든 확률이 사용되지 않았습니다.");
        return probabilities.Length - 1; // 마지막 인덱스 반환 (기본값)
    }

}
