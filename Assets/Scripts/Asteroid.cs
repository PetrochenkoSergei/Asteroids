using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Gameobject")]
    public GameObject asteroid;
    [Header("Gameplay Gameobject")]
    public Gameplay gameplay;
    [Header("Max rotation asteroid")]
    public float maxRotation = 25f;
    private float rotationZ;
    private Rigidbody2D rb;
    private Camera mainCam;
    private int _generation;

    void Start()
    {
        mainCam = Camera.main;
        rotationZ = Random.Range(-maxRotation, maxRotation);
        rb = asteroid.GetComponent<Rigidbody2D>();
        setupSpeed(rb, transform.right);
        setupSpeed(rb, transform.up);
    }

    void setupSpeed(Rigidbody2D rb, Vector3 direction)
    {
        float speedX = Random.Range(200f, 800f);
        int selectorX = Random.Range(0, 2);
        float dirX = 0;
        if (selectorX == 1) { dirX = -1; }
        else { dirX = 1; }
        float finalSpeedX = speedX * dirX;
        rb.AddForce(direction * finalSpeedX);
    }

    public void SetGeneration(int generation)
    {
        _generation = generation;
    }

    void Update()
    {
        asteroid.transform.Rotate(new Vector3(0, 0, rotationZ) * Time.deltaTime);
        CheckPosition();
        float dynamicMaxSpeed = 3f;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -dynamicMaxSpeed, dynamicMaxSpeed), Mathf.Clamp(rb.velocity.y, -dynamicMaxSpeed, dynamicMaxSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Bullet(Clone)")
        {
            if (_generation < 3)
            {
                CreateSmallAsteriods(2);
            }
            Destroy();
        }
        if (collision.collider.name == "Ship")
        {
            gameplay.RocketFail();
        }
    }

    void CreateSmallAsteriods(int asteroidsNum)
    {
        int newGeneration = _generation + 1;
        for (int i = 1; i <= asteroidsNum; i++)
        {
            float scaleSize = 0.5f;
            GameObject AsteroidClone = Instantiate(asteroid, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
            AsteroidClone.transform.localScale = new Vector3(AsteroidClone.transform.localScale.x * scaleSize, AsteroidClone.transform.localScale.y * scaleSize, AsteroidClone.transform.localScale.z * scaleSize);
            AsteroidClone.GetComponent<Asteroid>().SetGeneration(newGeneration);
            AsteroidClone.SetActive(true);
        }
    }

    private void CheckPosition()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (transform.position.x > sceneRightEdge)
        {
            transform.position = new Vector2(sceneLeftEdge, transform.position.y);
        }
        if (transform.position.x < sceneLeftEdge) { transform.position = new Vector2(sceneRightEdge, transform.position.y); }
        if (transform.position.y > sceneTopEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneBottomEdge);
        }
        if (transform.position.y < sceneBottomEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneTopEdge);
        }
    }

    public void Destroy()
    {
        gameplay.asterodDestroyed();
        Destroy(gameObject, 0.01f);
    }

}
