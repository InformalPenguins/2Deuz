using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EditorCellController : MonoBehaviour {
    public GameObject normalGameObject;
    public GameObject hoveredGameObject;

    public void SetHovered(bool isHovered) {
        normalGameObject.SetActive(!isHovered);
        hoveredGameObject.SetActive(isHovered);
    }

    void OnMouseOver()
    {
        SetHovered(true);
    }

    void OnMouseExit()
    {
        SetHovered(false);
    }
}
