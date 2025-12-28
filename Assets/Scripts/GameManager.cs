using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance; //Singleton

    public GameObject player;
    
    private InputAction pauseAction;

    [Header("UI")]
    public TMP_Text healthText;
    public TMP_Text armorText;

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
    
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void UpdateUI()
    {
        int health = player.GetComponent<Health>().HP;
        int armor = player.GetComponent<Health>().Armor;

        healthText.text = health.ToString();
        armorText.text = armor.ToString();
    }
}
