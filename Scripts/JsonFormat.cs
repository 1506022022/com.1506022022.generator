using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/JsonFormat", fileName = "JsonFormat")]
public class JsonFormat : ScriptableObject
{
    public string Format;
}