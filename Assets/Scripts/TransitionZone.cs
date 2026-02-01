using UnityEngine;

public class TransitionZone : MonoBehaviour
{
    #region Inspector members

    public string sceneToLoad;

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.instance.loadScene(sceneToLoad);
        }
    }
}
