using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private ImageTimer harvistTimer;
    [SerializeField] private ImageTimer eatingTimer;
    [SerializeField] private Image raidTimerImg;
    [SerializeField] private Image peasantTimerImg;
    [SerializeField] private Image warriorTimerImg;

    [SerializeField] private Button peasantBtn;
    [SerializeField] private Button warriorBtn;
    [SerializeField] private TMP_Text resourcesText;

    [SerializeField] private int peasantCount;
    [SerializeField] private int warriorCount;
    [SerializeField] private int wheatCount;
    [SerializeField] private int wheatPerPeasant;
    [SerializeField] private int wheatPerWarrior;
    [SerializeField] private int peasantCosts;
    [SerializeField] private int warriorCost;
    [SerializeField] private int peasantCreateTime;
    [SerializeField] private int warriorCreateTime;

    [SerializeField] private float raidMaxTime;
    [SerializeField] private int raidIncrease;
    [SerializeField] private int lvlOfRaidStart;
    [SerializeField] private int nextRaid;

    private int _currentLvl = 0;
    

    
    private float _peasantTimer = -2;
    private float _warriorTimer = -2;
    private float _raidTimer;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        _raidTimer = raidMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        _raidTimer -= Time.deltaTime;
        raidTimerImg.fillAmount = _raidTimer / raidMaxTime;

        if (_raidTimer <= 0)
        {
            _raidTimer = raidMaxTime;
            _currentLvl++;
            if (_currentLvl > lvlOfRaidStart) 
            {
                warriorCount -= nextRaid;
                nextRaid += raidIncrease;
            }
        }

        if (harvistTimer.Tick)
        {
            wheatCount += peasantCount * wheatPerPeasant;
        }

        if (eatingTimer.Tick)
        {
            wheatCount -= warriorCount * wheatPerWarrior;
        }

        if (_peasantTimer > 0)
        {
            _peasantTimer -= Time.deltaTime;
            peasantTimerImg.fillAmount = _peasantTimer / peasantCreateTime;
        } else if (_peasantTimer > -1)
        {
            peasantTimerImg.fillAmount = 1;
            peasantBtn.interactable = true;
            peasantCount += 1;
            _peasantTimer = -2;
        }

        if (_warriorTimer > 0)
        {
            _warriorTimer -= Time.deltaTime;
            warriorTimerImg.fillAmount = _warriorTimer / warriorCreateTime;
        }
        else if (_warriorTimer > -1)
        {
            warriorTimerImg.fillAmount = 1;
            warriorBtn.interactable = true;
            warriorCount += 1;
            _warriorTimer = -2;
        }

        ButtonInteractableChanger();
        UpdateText();
        if (warriorCount < 0 || wheatCount < 0)
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
        }

    
    }
    private void ButtonInteractableChanger()
    {
        if (wheatCount < peasantCosts)
        {
            peasantBtn.interactable = false;
        }
        else
        {
            peasantBtn.interactable = true;
        }

        if (wheatCount < warriorCost)
        {
            peasantBtn.interactable = false;
        }
        else
        {
            peasantBtn.interactable = true;
        }
    }

    public void CreatePeasant()
    {
        wheatCount -= peasantCosts;
        _peasantTimer = peasantCreateTime;
        peasantBtn.interactable = false;
    }

    public void CreateWarrior() {
        wheatCount -= warriorCost;
        _warriorTimer = warriorCreateTime;
        warriorBtn.interactable = false;
    }
    
    public void UpdateText() 
    {
        resourcesText.text = peasantCount + "\n" + warriorCount + "\n\n" + wheatCount + "\n\n" + 
            (lvlOfRaidStart - _currentLvl >= 0 ? lvlOfRaidStart - _currentLvl: 0) + "\n\n\n\n" + (lvlOfRaidStart - _currentLvl > 0 ? 0 : nextRaid);
    }
}
