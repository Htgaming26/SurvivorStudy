using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel, totalExperience;
    int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;


    //Bullet bullet;
    Player player;

    public void Init(Player player)
    {
        this.player = player;
    }


    void Start()
    {
        UpdateLevel();
    }

    void Update()
    {
        
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
            
                if (currentLevel == 1)
                    player.timeShoot -= 0.3f; // hard code
                if (currentLevel == 2)
                    player.timeShoot -= 0.5f;
                if (currentLevel == 3)
                    player.timeShoot -= 0.5f;
                //bullet.damageShoot -= 2;
                if (currentLevel == 4)
                    player.timeShoot -= 0.3f;
            //bullet.damageShoot -= 2;
            if (currentLevel == 5)
            {
                Debug.Log("Unlock Boom");
                player.UnlockBoom();
            }
           
            //todo
            player.SetTimeShoot(player.timeShoot);
            Debug.Log("Level up => timeShoot = " + player.timeShoot);
        }
    }

    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;

        levelText.text = currentLevel.ToString();
        experienceText.text = start +" exp / "+ end +" exp";
        experienceFill.fillAmount = (float)start / (float)end;
    }
}
