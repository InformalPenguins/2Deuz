using UnityEngine;

public class HazardHpBarController : MonoBehaviour
{
    public float MaxHP = 1;
    private float _CurrentHP = 1;
    public float CurrentHP {
        get
        {
            return this._CurrentHP;
        }
        set
        {
            this._CurrentHP = value;
            HpChanged();
        }
    }

    // Update is called once per frame
    void HpChanged()
    {
        transform.localScale = new Vector2(_CurrentHP / MaxHP, 3f);
    }
}
