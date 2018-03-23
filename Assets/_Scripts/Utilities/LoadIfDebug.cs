using UnityEngine;

public class LoadIfDebug : MonoBehaviour
{
    public GameObject[] objects;
    public bool isDebug = false;
    // Use this for initialization
    void Start () {
        if (objects == null)
            return;

        foreach (GameObject obj in objects) {
            obj.SetActive(isDebug);
        }
		
	}
}
