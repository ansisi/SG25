using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashObject : MonoBehaviour
{
    public float lifeTime = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime < 0 )
        {
            GameManager.Instance.TrashCount(-1);
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        GameManager.Instance.TrashCount(-1);
    }
}
