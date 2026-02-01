using UnityEngine;

public enum GameState
{
    MENU,
    INTRO,
    GATE,
    BANQUET,
    END
};

public class GameManager : MonoBehaviour
{
    public GameState gameState;

    public static GameManager instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        
    }
}
