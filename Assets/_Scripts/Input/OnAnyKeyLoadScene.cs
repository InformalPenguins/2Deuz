using GameFramework.GameStructure;
using System;
using UnityEngine;

public class OnAnyKeyLoadScene : MonoBehaviour {
    
    [Tooltip("Name of the Scene to load")]
    public string SceneName;

	void Update () {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                GameManager.LoadSceneWithTransitions(SceneName);
        }
    }
}
