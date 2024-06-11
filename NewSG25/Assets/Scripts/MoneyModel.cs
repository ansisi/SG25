using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMoneyData", menuName = "ScriptableObjects/MoneyModel")]
[SerializeField]
public class MoneyModel : ScriptableObject
{
    public int value;
    public GameObject moneyPrefab;
}
