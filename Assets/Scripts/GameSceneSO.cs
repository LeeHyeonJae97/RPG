using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum GameSceneType { Title, Game, Manager, Stage, UI }

[CreateAssetMenu(fileName = "GameScene", menuName = "ScriptableObject/GameScene")]
public class GameSceneSO : ScriptableObject
{
    public GameSceneType type;
    public AssetReference sceneReference;
}
