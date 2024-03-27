using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{ 
    public GameObject gameOverScreen;

    public ImageTimer harvistTimer;
    public ImageTimer eatingTimer;
    public Image raidTimerImg;
    public Image peasantTimerImg;
    public Image warriorTimerImg;

    public Button peasantBtn;
    public Button warriorBtn;
    public TMP_Text resourcesText;

    public int peasantCount;
    public int warriorCount;
    public int wheatCount;
    public int wheatPerPeasant;
    public int wheatPerWarrior;
    public int peasantCosts;
    public int warriorCost;
    public int peasantCreateTime;
    public int warriorCreateTime;

    public float raidMaxTime;
    public int raidIncrease;
    public int nextRaid;

    private float peasantTimer = -2;
    private float warriorTimer = -2;
    private float raidTimer;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        raidTimer = raidMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        raidTimer -= Time.deltaTime;
        raidTimerImg.fillAmount = raidTimer / raidMaxTime;

        if (raidTimer <= 0)
        {
            raidTimer = raidMaxTime;
            warriorCount -= nextRaid;
            nextRaid += raidIncrease;
        }

        if (harvistTimer.Tick)
        {
            wheatCount += peasantCount * wheatPerPeasant;
        }

        if (eatingTimer.Tick)
        {
            wheatCount -= warriorCount * wheatPerWarrior;
        }

        if (peasantTimer > 0)
        {
            peasantTimer -= Time.deltaTime;
            peasantTimerImg.fillAmount = peasantTimer / peasantCreateTime;
        } else if (peasantTimer > -1)
        {
            peasantTimerImg.fillAmount = 1;
            peasantBtn.interactable = true;
            peasantCount += 1;
            peasantTimer = -2;
        }

        if (warriorTimer > 0)
        {
            warriorTimer -= Time.deltaTime;
            warriorTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer > -1)
        {
            warriorTimerImg.fillAmount = 1;
            warriorBtn.interactable = true;
            warriorCount += 1;
            warriorTimer = -2;
        }


        UpdateText();
        if (warriorCount < 0)
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
        }
    }

    public void CreatePeasant()
    {
        wheatCount -= peasantCosts;
        peasantTimer = peasantCreateTime;
        peasantBtn.interactable = false;
    }

    public void CreateWarrior() {
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime;
        warriorBtn.interactable = false;
    }
    
    public void UpdateText() 
    {
        resourcesText.text = peasantCount + "\n" + warriorCount + "\n\n" +wheatCount;
    }
}
