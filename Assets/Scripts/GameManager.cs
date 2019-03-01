using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static bool _playerDeath = false;
    public static bool _shieldDeath = false;
    public static bool _paused = true;
    public static float hits = 0;
    public AudioClip[] clips;
    GameObject player;
    GameObject thrownShield;
    GameObject title;
    AudioSource source;
    bool startedFirst = false;
    bool started = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        title = transform.GetChild(0).GetChild(0).gameObject;
        source = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    IEnumerator FadeObject(GameObject thing, float speed)
    {
        float a = 1.0f;
        if (speed < 0.0f)
            a = 0.0f;
        while ((a > 0.0f && speed > 0.0f) || (a < 1.0f && speed < 0.0f))
        {
            Color c;
            a -= speed * Time.deltaTime;
            if (thing.transform.childCount > 0.0f)
            {
                for (int i = 0; i < thing.transform.childCount; i++)
                {
                    if (thing.transform.GetChild(i).gameObject.GetComponent<Text>() != null)
                    {
                        c = thing.transform.GetChild(i).gameObject.GetComponent<Text>().color;
                        c.a = a;
                        thing.transform.GetChild(i).gameObject.GetComponent<Text>().color = c;
                    }
                    else if (thing.transform.GetChild(i).gameObject.GetComponent<Image>() != null)
                    {
                        c = thing.transform.GetChild(i).gameObject.GetComponent<Image>().color;
                        c.a = a;
                        thing.transform.GetChild(i).gameObject.GetComponent<Image>().color = c;
                    }
                }
            }
            else
            {
                if (thing.GetComponent<Text>() != null)
                {
                    c = thing.GetComponent<Text>().color;
                    c.a = a;
                    thing.GetComponent<Text>().color = c;
                }
                else if (thing.GetComponent<Image>() != null)
                {
                    c = thing.GetComponent<Image>().color;
                    c.a = a;
                    thing.GetComponent<Image>().color = c;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator StartGame()
    {
        started = true;
        startedFirst = true;
        StartCoroutine(FadeObject(title, 2.0f));
        source.clip = clips[0];
        source.Play();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(1).gameObject, -1.5f));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(1).gameObject, 1.5f));
        yield return new WaitForSeconds(0.5f);
        source.clip = clips[1];
        source.Play();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(2).gameObject, -1.5f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(2).gameObject, 1.5f));
        yield return new WaitForSeconds(1.0f);
        source.clip = clips[2];
        source.Play();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(3).gameObject, -1.0f));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(3).gameObject, 1.0f));
        yield return new WaitForSeconds(2.0f);
        player.transform.GetChild(2).gameObject.GetComponent<Animator>().SetFloat("Speed", 1.0f);
        AnimationStateController._state = 0;
        yield return new WaitForSeconds(3.25f);
        _paused = false;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator ReStartGame()
    {
        started = true;
        StartCoroutine(FadeObject(title, 2.0f));
        source.clip = clips[0];
        source.Play();
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(1).gameObject, -1.5f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(1).gameObject, 1.5f));
        yield return new WaitForSeconds(0.25f);
        source.clip = clips[1];
        source.Play();
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(2).gameObject, -1.5f));
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(2).gameObject, 1.5f));
        yield return new WaitForSeconds(0.5f);
        source.clip = clips[2];
        source.Play();
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(3).gameObject, -1.0f));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(3).gameObject, 1.0f));
        yield return new WaitForSeconds(1.0f);
        player.transform.GetChild(2).gameObject.GetComponent<Animator>().SetFloat("Speed", 2.0f);
        AnimationStateController._state = 0;
        yield return new WaitForSeconds(1.625f);
        _paused = false;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator DeathEffects()
    {
        _playerDeath = false;
        _paused = true;
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(4).gameObject, -1.0f));
        yield return new WaitForSeconds(1.0f);
        ResetEverything();
        StartCoroutine(FadeObject(transform.GetChild(0).GetChild(4).gameObject, 1.0f));
    }

    void Update()
    {
        if (!startedFirst && !started && (Input.anyKeyDown || InputManager._ps4AnyButtonDown))
        {
            //StartCoroutine(StartGame());
            StartCoroutine(ReStartGame());
        }
        else if (!started && (Input.anyKeyDown || InputManager._ps4AnyButtonDown))
        {
            StartCoroutine(ReStartGame());
        }

        if (hits >= 3)
        {
            hits = 0;
            Destroy(player);
            _playerDeath = true;
        }

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (thrownShield == null && GameObject.FindGameObjectWithTag("ThrownShield") != null)
            thrownShield = GameObject.FindGameObjectWithTag("ThrownShield");

        if (_playerDeath)
        {
            //Death Effects
            StartCoroutine(DeathEffects());
        }
        if (!_paused && _shieldDeath)
        {
            _shieldDeath = false;
            thrownShield.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

            if (Random.Range(0, 2) == 0)
                thrownShield.transform.position = new Vector3(player.transform.position.x + 1.0f, player.transform.position.y, 0.0f);
            else
                thrownShield.transform.position = new Vector3(player.transform.position.x - 1.0f, player.transform.position.y, 0.0f);
        }
    }

    private void ResetEverything()
    {
        AnimationStateController._state = -1;
        started = false;
        ThrowShield.hasShield = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(FadeObject(title, -100.0f));
    }
}