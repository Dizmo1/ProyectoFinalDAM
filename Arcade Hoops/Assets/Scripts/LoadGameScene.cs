using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour {
    public void LoadScene() {
        SceneManager.LoadScene("GameScene"); // Nombre exacto de la escena
    }
}