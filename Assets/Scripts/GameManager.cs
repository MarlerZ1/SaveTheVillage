using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class GameManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioMixerGroup mixer;

    [Header("Game Automatic Timers")]
    [SerializeField] private ImageTimer harvistTimer;
    [SerializeField] private ImageTimer eatingTimer;
    [SerializeField] private Image raidTimerImg;

    [Header("Player Timers")]
    [SerializeField] private Image peasantTimerImg;
    [SerializeField] private Image warriorTimerImg;


    [Header("Gameplay Button")]
    [SerializeField] private Button peasantBtn;
    [SerializeField] private Button warriorBtn;

    [Header("Gameplay text")]
    [SerializeField] private TMP_Text resourcesText;

    [Header("GameOver Screen Block")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text looseInformationCount;

    [Header("GameWin Screen Block")]
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private TMP_Text winInformationCount;

    [Header("GamePause Screen Block")]
    [SerializeField] private GameObject gamePauseScreen;

    [Header("Sound Block")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip harvesting;
    [SerializeField] private AudioClip eating;
    [SerializeField] private AudioClip villagers;
    [SerializeField] private AudioClip warriors;
    [SerializeField] private AudioClip attack;

    [Header("Gamedesign settings")]
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
    [SerializeField] private int peasantToWin;




    private bool _isMusicPlay = true;
    private int _currentLvl = 0;

    private int _totalWeatCount;
    private int _enemyDefeats = 0;
    private int _totalWarriorCount = 0;
    private float _peasantTimer = -2;
    private float _warriorTimer = -2;
    private float _raidTimer;
    // Start is called before the first frame update
    void Start()
    {
        _totalWeatCount = wheatCount;
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
            if (_isMusicPlay)
                AudioSource.PlayClipAtPoint(attack, new Vector3(0, 0, -10), 1);
            _raidTimer = raidMaxTime;
            _currentLvl++;
            if (_currentLvl > lvlOfRaidStart) 
            {
                _enemyDefeats += Mathf.Min(warriorCount, nextRaid);
                warriorCount -= nextRaid;
                nextRaid += raidIncrease;
            }
        }

        if (harvistTimer.Tick)
        {
            if (_isMusicPlay)
                AudioSource.PlayClipAtPoint(harvesting, new Vector3(0, 0, -10), 1);
            _totalWeatCount += peasantCount * wheatPerPeasant;
            wheatCount += peasantCount * wheatPerPeasant;
        }

        if (eatingTimer.Tick)
        {
            if (_isMusicPlay)
                AudioSource.PlayClipAtPoint(eating, new Vector3(0, 0, -10), 1);
            wheatCount -= warriorCount * wheatPerWarrior;
        }

        if (_peasantTimer > 0)
        {
            _peasantTimer -= Time.deltaTime;
            peasantTimerImg.fillAmount = _peasantTimer / peasantCreateTime;
        } else if (_peasantTimer > -1)
        {
            if (_isMusicPlay)
                AudioSource.PlayClipAtPoint(villagers, new Vector3(0, 0, -10), 1);
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
            if (_isMusicPlay)
                AudioSource.PlayClipAtPoint(warriors, new Vector3(0, 0, -10), 1);
            warriorTimerImg.fillAmount = 1;
            warriorBtn.interactable = true;
            warriorCount += 1;
            _totalWarriorCount++;
            _warriorTimer = -2;
        }

        ButtonInteractableChanger();
        UpdateText();
        if (warriorCount < 0 || wheatCount < 0)
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);


            looseInformationCount.text = (warriorCount < 0 ? "Осада" : "Голод") + "\n\n" + _totalWarriorCount + "\n" + peasantCount + "\n" + _totalWeatCount + "\n\n" + _enemyDefeats + "\n\n" + _currentLvl;
        }

        if (peasantCount >= peasantToWin)
        {
            Time.timeScale = 0;
            gameWinScreen.SetActive(true);
            winInformationCount.text = _totalWarriorCount + "\n" + peasantCount + "\n" + _totalWeatCount + "\n\n" + _enemyDefeats + "\n\n" + _currentLvl;
        }

    }
    private void ButtonInteractableChanger()
    {
        if (wheatCount < peasantCosts)
        {
            peasantBtn.interactable = false;
        }
        else if (_peasantTimer == -2)
        {
            peasantBtn.interactable = true;
        }

        if (wheatCount < warriorCost)
        {
            warriorBtn.interactable = false;
        }
        else if (_warriorTimer == -2)
        {
            warriorBtn.interactable = true;
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

    public void ReastartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //       StartCoroutine(IERestart());
    }

    public void ChangePauseState() 
    {
        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        gamePauseScreen.SetActive(!gamePauseScreen.activeInHierarchy);
    }

    public void playClick()
    {
        print(_isMusicPlay);
        if (_isMusicPlay)
            AudioSource.PlayClipAtPoint(click, new Vector3(0, 0, -10), 1);
    }

    public void changeMusicState()
    {
        if (_isMusicPlay)
            mixer.audioMixer.SetFloat("AllMusic", -80);
        else
            mixer.audioMixer.SetFloat("AllMusic", 0);
        _isMusicPlay = !_isMusicPlay;
    }
}
