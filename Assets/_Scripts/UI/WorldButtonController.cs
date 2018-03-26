using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels.Components;
using GameFramework.GameStructure.Levels.ObjectModel;
using UnityEngine;

public class WorldButtonController : MonoBehaviour
{
    Animator animator;
    int selectedLv = -1, lvButtonId;
    void Start ()
    {
        LevelButton lvButton = GetComponent<LevelButton>();
        lvButtonId = lvButton.GameItem.Number;
        Level[] levels = GameManager.Instance.Levels.Items;
        Level lastUnlocked = null;

        foreach (Level level in levels) {
            if (level.IsUnlocked) {
                lastUnlocked = level;
            }
        }
        if (lastUnlocked != null)
        {
            selectedLv = lastUnlocked.Number;
        }

        animator = GetComponent<Animator>();
        animator.SetBool("Attention", selectedLv == lvButtonId);
    }

    void Update()
    {
    }
}
