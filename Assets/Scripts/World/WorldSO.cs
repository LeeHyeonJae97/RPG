using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "World", menuName = "ScriptableObject/World/World")]
public class WorldSO : ScriptableObject
{    
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public AssetReference[] Stages { get; private set; }
}
