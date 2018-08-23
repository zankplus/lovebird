using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWave : MonoBehaviour
{
    public float speed = 8f;
    public float size = 1f;
    public float levelSize;
    public Transform surface;
    public Transform deep;
    public BoxCollider2D drowningZone;
    public Transform target;

    private AudioSource audioSource;
    public AudioClip splashIn;
    public AudioClip splashOut;
    private SoundManager soundManager;
    private PlayerController player;

    public SplashZone splashZone;

    public ParticleSystem splashParticles;

	// Use this for initialization
	void Start ()
    {
        deep.localScale = new Vector3(deep.localScale.x, levelSize, 1f);
        deep.localPosition = new Vector3(0f, -levelSize / 2f - 0.5f, 0f);
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();

        BoxCollider2D drowningZone = GetComponent<BoxCollider2D>();
        drowningZone.offset = new Vector2(deep.localPosition.x, deep.localPosition.y - 0.01f);
        drowningZone.size = new Vector2(deep.localScale.x, deep.localScale.y);

        soundManager = FindObjectOfType<SoundManager>();
	}

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < target.position.y)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (transform.position.y > target.position.y)
                transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            audioSource.PlayOneShot(splashIn);
            soundManager.Submerge();

            Splash();

            player.Submerge();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("out");
            audioSource.PlayOneShot(splashOut);
            soundManager.Emerge();

            Splash();
            player.Emerge();
        }
    }

    public void Splash()
    {
        ParticleSystem ps = Instantiate(splashParticles);

        ps.transform.position = player.transform.position;

        // Set particle velocity according to velocity
        ParticleSystem.MainModule newMain = ps.main;
        float speed = Mathf.Abs(player.rb.velocity.y / 2f);
        newMain.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.8f * 1.5f, speed * 1.2f * 1.5f + 1);

        // Set particle quantity according to velocity
        ParticleSystem.EmissionModule newEmission = ps.emission;
        newEmission.rateOverTimeMultiplier = Mathf.Pow(player.rb.velocity.y, 2) / (900f) * 300f + 50;

        ParticleSystem.TriggerModule newTrigger = ps.trigger;
        newTrigger.SetCollider(0, splashZone._collider);

        ps.Play();
    }
}
