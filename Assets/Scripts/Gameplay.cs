using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{

    [Header("Asteroid Gameobject")]
    public GameObject asteroid;
    [Header("UFO Gameobject")]
    public GameObject ufo;
    [Header("Ship Gameobject")]
    public GameObject ship;
    [Header("UI text fields")]
    public Text lifeTextfield;
    public Text scoreTextfield;
    [Header("Ship lives")]
    public int life = 3;
    [Header("UFO respawn every score points")]
    public int ufoScoreRespawn = 150;
    private int _startLevelAsteroidsNum;
    private Camera mainCam;
    private int asteroidLife;
    private int score = 0;


    private void Start()
    {
        asteroid.SetActive(false);
        ufo.SetActive(false);
        mainCam = Camera.main;
        _startLevelAsteroidsNum = 1;
        CreateAsteroids(_startLevelAsteroidsNum);
    }

    private void Update()
    {
        scoreTextfield.text = score.ToString();
        if (asteroidLife <= 0)
        {
            asteroidLife = 6;
            CreateAsteroids(1);
        }

        if (score > 0 && score % ufoScoreRespawn == 0)
        {
            GameObject ufoObject = GameObject.FindWithTag("UFO");
            if (ufoObject == null)
            {
                CreateUFO();
            }
        }

    }

    private void CreateUFO()
    {
        GameObject AsteroidClone = Instantiate(ufo, new Vector2(Random.Range(-10, 10), 6f), transform.rotation);
        AsteroidClone.SetActive(true);
    }

    private void CreateAsteroids(float asteroidsNum)
    {
        for (int i = 1; i <= asteroidsNum; i++)
        {
            GameObject AsteroidClone = Instantiate(asteroid, new Vector2(Random.Range(-10, 10), 6f), transform.rotation);
            AsteroidClone.GetComponent<Asteroid>().SetGeneration(1);
            AsteroidClone.SetActive(true);
        }
    }

    public void RocketFail()
    {
        bool dead = (life == 0) ? false : true;
        if (!dead)
        {
            PlayerStats.Score = score;
            SceneManager.LoadSceneAsync("Over");
        } else
        {
            lifeTextfield.text = lifeTextfield.text.Remove(lifeTextfield.text.Length - 2);
            DestroyAll();
            ship.GetComponent<Ship>().ResetRocket();
            life--;
            CreateAsteroids(2);
        }
    }

    private void DestroyAll()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in asteroids)
            Destroy(asteroid);

        GameObject[] ufos = GameObject.FindGameObjectsWithTag("UFO");
        foreach (GameObject ufo in ufos)
            Destroy(ufo);
    }

    public void asterodDestroyed()
    {
        GetComponent<AudioSource>().Play();
        asteroidLife--;
        score += 10;
    }

    public void ufoDestroyed()
    {
        GetComponent<AudioSource>().Play();
        score += 20;
    }

}