using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Transform fill;

    Player player;
    public void Init(Player player)
    {
        this.player = player;
    }

    public void UpdateHealth()
    {
        //Debug.Log("update health");
        float x = (float)player.hp / (float)player.maxHp;
        x = Mathf.Clamp01(x);
        fill.localScale = new Vector3(x, 1, 1);
    }
}
