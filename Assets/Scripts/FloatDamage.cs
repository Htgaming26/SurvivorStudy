using UnityEngine;
using TMPro;

public class FloatDamage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void SetText(int value)
    {
        text.text = value.ToString();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
