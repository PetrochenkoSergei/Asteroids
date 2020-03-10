using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Over : MonoBehaviour
{
    public Text scoreTextfield;
    void Start()
    {
        scoreTextfield.text = $"Your score:\r\n{PlayerStats.Score.ToString()}";
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync("Level");
    }

    void Update()
    {
        
    }
}
