using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Money : ScriptableObject
{
    public string moneyName;
    public int value;
    public Sprite icon;
    public GameObject moneyPrefab;
}