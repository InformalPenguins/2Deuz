using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButtonDebugger : MonoBehaviour {
    Vector2 lastPosition = new Vector2();
    public Camera camera;
	
	void Update () {
        Vector2 newPosition = WorldToScreenPoint(camera, transform.position);
       
        if(newPosition.x != lastPosition.x)
        {
            lastPosition = newPosition;
            print(newPosition); 
        }
    }
    public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
    {
        if ((Object)cam == (Object)null)
            return new Vector2(worldPoint.x, worldPoint.y);
        return (Vector2)cam.WorldToScreenPoint(worldPoint);
    }
}
