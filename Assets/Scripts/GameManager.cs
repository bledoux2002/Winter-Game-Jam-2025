using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance; //Singleton

    public PlayerController player;
    
    private InputAction pauseAction;
    private GameObject pauseMenuUI;
    private GameObject resumeButton;
    public GameObject GameOverText;
    private bool _paused;
    public bool IsPaused => _paused;
    private bool _isMenuScene;
    [SerializeField] private string menuScene = "MainMenu";

    [Header("UI")]
    private TMP_Text healthText;
    private TMP_Text armorText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isMenuScene = scene.name == menuScene;
        if (!_isMenuScene)
        {
            player = FindFirstObjectByType<PlayerController>();
            pauseMenuUI = GameObject.Find("PauseMenu")
                          ?? FindInactive("PauseMenu");
            resumeButton = GameObject.Find("ResumeButton")
                          ?? FindInactive("ResumeButton");
            healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
            armorText = GameObject.FindWithTag("ArmorText").GetComponent<TMP_Text>();

            _paused = false;
            Time.timeScale = 1f;
            SetCursorState(_isMenuScene);

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);

        }
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private GameObject FindInactive(string name)
    {
        var all = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in all)
        {
            if (go.name == name && go.hideFlags == HideFlags.None)
                return go;
        }
        return null;
    }

    void Update()
    {
        if (_isMenuScene) return;

        if (pauseAction.WasPressedThisFrame())
        {
            if (_paused) Instance.Resume();
            else Instance.Pause();
        }
    }

    public void Pause()
    {
        if (_paused) return;

        _paused = true;
        Time.timeScale = 0f;
        SetCursorState(true);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        if (player != null)
            player.Paused = true;
    }

    public void Resume()
    {
        if (!_paused) return;

        _paused = false;
        Time.timeScale = 1f;
        SetCursorState(false);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (player != null)
            player.Paused = false;
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        GameOverText.SetActive(false);
        resumeButton.SetActive(true);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void UpdateUI()
    {
        int health = player.GetComponent<HealthManager>().HP;
        int filter = player.GetComponent<FilterManager>().Filter;

        healthText.text = health.ToString();
        armorText.text = filter.ToString();
    }

    public void GameOver()
    {
        Pause();
        GameOverText.SetActive(true);
        resumeButton.SetActive(false);
    }
}
