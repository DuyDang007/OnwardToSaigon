using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public string nextLevelName;
    public GameSpawner spawnerRed;
    public GameSpawner spawnerYellow;
    public SkillSpawner skillSpawner;
    public Text moneyText;
    public BuyButton[] buttonArray; // Array index following GameStatus.CharacterClassE index
    public Toggle skillToggle;
    public Toggle muteToggle;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject loadingNextScreen;
    public GameObject loadingCurrentScreen;
    public GameObject startButton;

    [Header("Enable Units")]
    public bool enableGunner;
    public bool enableGrenadier;
    public bool enableSniper;
    public bool enableRocket;
    public bool enableArmour;
    public bool enableTank;
    public bool enableSkill;

    [Header("Camera Settings")]
    [SerializeField]
    private float camMinZ;
    [SerializeField]
    private float camMaxZ;
    [SerializeField]
    private float cameraTranslateSpeed;

    // Base health
    public Image redHealthBar;
    public Image yellowHealthBar;

    private GameStatus.WarStatus warStat = GameStatus.WarStatus.Running;
    private SkillToggleButton skillToggleButton;
    private float cameraMoveMultiply;
    private AudioListener listener;

    // Button press handler
    public void GunnerButtonPress()
    {
        BuyBtnPress(GameStatus.CharacterClassE.Gunner);
    }

    public void GrenadierButtonPress()
    {
        BuyBtnPress(GameStatus.CharacterClassE.Grenadier);
    }

    public void SniperButtonPress()
    {
        BuyBtnPress(GameStatus.CharacterClassE.Sniper);
    }
    public void RocketButtonPress()
    {
        BuyBtnPress(GameStatus.CharacterClassE.Rocket);
    }
    public void ArmourButtonPress()
    {
        BuyBtnPress(GameStatus.CharacterClassE.Armour);
    }
    public void TankButtonPress()
    {
        BuyBtnPress(GameStatus.CharacterClassE.Tank);
    }
    public void SkillTogglePress()
    {
        SkillButtonPress();
    }
    public void PausePress()
    {
        pause();
        pauseScreen.SetActive(true);
    }
    public void MutePress()
    {
        if(muteToggle.isOn)
        {
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("sound", 0);
            PlayerPrefs.Save();
        }
        else
        {
            AudioListener.volume = 1f;
            PlayerPrefs.SetInt("sound", 1);
            PlayerPrefs.Save();
        }
    }
    public void ResumePress()
    {
        resume();
        pauseScreen.SetActive(false);
    }
    public void NextLevelPress()
    {
        loadingNextScreen.SetActive(true);
        AsyncOperation loader = SceneManager.LoadSceneAsync(nextLevelName);
        StartCoroutine(nextLoading(loader));
    }
    public void StartPress()
    {
        loadingCurrentScreen.SetActive(false);
        resume();
    }
    public void PlayAgainPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenuPress()
    {
        SceneManager.LoadScene(0);
    }

    public void LeftPress()
    {
        cameraMoveMultiply = -1f;
    }
    public void RightPress()
    {
        cameraMoveMultiply = 1f;
    }
    public void LeftRightUp()
    {
        cameraMoveMultiply = 0f;
    }

    // --- End of button press handler


    private void BuyBtnPress(GameStatus.CharacterClassE charClass)
    {
        GameStatus.BuyStatusE buyStatus = spawnerRed.Buy(charClass);
        if (buyStatus == GameStatus.BuyStatusE.Success)
        {
            buttonArray[(int)charClass].activateButton(false);
            buttonArray[(int)charClass].updateFillAmount(1f);
        }
    }
    private void SkillButtonPress()
    {
        if(skillToggle.isOn)
        {
            skillSpawner.activeAim();
        }
        else
        {
            skillSpawner.deactiveAim();
        }
    }

    public void pause()
    {
        Time.timeScale = 0f;
    }

    public void resume()
    {
        Time.timeScale = 1f;
    }

    IEnumerator winCoroutine()
    {
        yield return new WaitForSeconds(1f);
        pause();
        warStat = GameStatus.WarStatus.Win;
        winScreen.SetActive(true);
    }

    IEnumerator loseCoroutine()
    {
        yield return new WaitForSeconds(1f);
        warStat = GameStatus.WarStatus.Lose;
        pause();
        loseScreen.SetActive(true);
    }

    IEnumerator nextLoading(AsyncOperation loader)
    {
        while (!loader.isDone)
        {
            yield return null;
        }
    }

    private void Awake()
    {
        skillToggleButton = skillToggle.gameObject.GetComponent<SkillToggleButton>();
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        loadingNextScreen.SetActive(false);
        loadingCurrentScreen.SetActive(true);
        listener = Camera.main.GetComponent<AudioListener>();
    }

    private void Start()
    {
        for(int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].setPrice(spawnerRed.characterPrice[i]);
        }
        pause();
        // Read playerpref to check if sound was off last scene
        if(PlayerPrefs.GetInt("sound") == 0) // OFF
        {
            muteToggle.isOn = true;
            MutePress();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            // Update button image fill amount
            if (spawnerRed.respawnTime[i] > 0)
            {
                float respawnTime = spawnerRed.respawnTime[i];
                float cooldownTime = spawnerRed.cooldownTime[i];
                float amount = respawnTime >= 0f ? respawnTime / cooldownTime : 0f;
                if (amount < 0.01f)
                {
                    amount = 0f;
                    buttonArray[i].activateButton(true);
                }
                buttonArray[i].updateFillAmount(amount);
            }
        }

        // For skill button, we continuous check its spawn state for fill amount
        if(skillSpawner.respawnTime > 0f)
        {
            skillToggle.interactable = false;
            skillToggle.isOn = false;
            float amount = skillSpawner.respawnTime / skillSpawner.cooldownTimeConst;
            if (amount < 0.01f)
            {
                amount = 0f;
                skillToggleButton.activateButton(true);
                skillToggle.interactable = true;
            }
            skillToggleButton.updateFillAmount(amount);
        }

        // Update money text
        moneyText.text = spawnerRed.money.ToString();

        // Update basement health bar
        redHealthBar.fillAmount     = (float)spawnerRed.getBaseHealth() / (float)spawnerRed.getMaxHealth();
        yellowHealthBar.fillAmount  = (float)spawnerYellow.getBaseHealth() / (float)spawnerYellow.getMaxHealth();

        // Check for base health
        if(warStat == GameStatus.WarStatus.Running)
        {
            if (spawnerRed.getBaseHealth() <= 0)
            {
                warStat = GameStatus.WarStatus.Lose;
                StartCoroutine(loseCoroutine());
            }
            else if (spawnerYellow.getBaseHealth() <= 0)
            {
                warStat = GameStatus.WarStatus.Win;
                StartCoroutine(winCoroutine());
            }
        }
        
    }

    private void Update()
    {
        if(cameraMoveMultiply != 0f)
        {
            float newCamZ = Camera.main.transform.position.z + cameraTranslateSpeed * Time.deltaTime * cameraMoveMultiply;
            if (newCamZ > camMinZ && newCamZ < camMaxZ)
            {
                Camera.main.transform.position += new Vector3(0f, 0f, cameraTranslateSpeed * Time.deltaTime * cameraMoveMultiply);
            }
        }

        // Keyboard input
        if(Input.GetKeyDown(KeyCode.Alpha1) && enableGunner)
        {
            GunnerButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && enableGrenadier)
        {
            GrenadierButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && enableSniper)
        {
            SniperButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && enableRocket)
        {
            RocketButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && enableArmour)
        {
            ArmourButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && enableTank)
        {
            TankButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) && enableSkill)
        {
            //if (!skillToggle.isOn) skillToggle.isOn = true;
            skillToggle.isOn = !skillToggle.isOn;
            SkillTogglePress();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftPress();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightPress();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            LeftRightUp();
        }
    }
}
