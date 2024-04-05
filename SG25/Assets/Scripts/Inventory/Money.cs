using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Money : ScriptableObject
{
    public string MoneyName;
    public int value;
    public Sprite icon;
    public string description;
}