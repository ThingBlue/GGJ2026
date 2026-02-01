using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    #region Inspector members

    public Image fadeImage;
    public float fadeAlphaSmoothTime;

    #endregion

    public static SceneLoader instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    float targetFadeAlpha = 0;
    string sceneToLoad = "";

    private void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    private void Update()
    {
        float fadeAlpha = Mathf.MoveTowards(fadeImage.color.a, targetFadeAlpha, (1.0f / fadeAlphaSmoothTime) * Time.deltaTime);
        fadeImage.color = new Color(0, 0, 0, fadeAlpha);

        if (sceneToLoad != "" && targetFadeAlpha >= 1 && fadeImage.color.a >= 1)
        {
            // Fade finished, load scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void loadScene(string sceneName)
    {
        targetFadeAlpha = 1;
        sceneToLoad = sceneName;
    }
}
