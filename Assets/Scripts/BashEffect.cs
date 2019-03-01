using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashEffect : MonoBehaviour
{
    public float growSpeed;
    public float moveSpeed;
    public float fadeSpeed;
    public float time;
    float timer = 0;

    void Start()
    {

    }

    public IEnumerator TheEffect()
    {
        Vector3 returnPos = transform.localPosition;
        Vector3 returnScale = transform.localScale;
        Color returnCol = GetComponent<MeshRenderer>().material.color;

        while (timer < time)
        {
            Vector3 locPos = transform.localPosition;
            Vector3 locScale = transform.localScale;
            Color locCol = GetComponent<MeshRenderer>().material.color;

            locPos.x += moveSpeed * Time.deltaTime;
            locScale.y += growSpeed * Time.deltaTime;
            locScale.x += growSpeed * Time.deltaTime;
            locScale.z += growSpeed * Time.deltaTime;
            locCol.a -= fadeSpeed * Time.deltaTime;

            transform.localPosition = locPos;
            transform.localScale = locScale;
            GetComponent<MeshRenderer>().material.color = locCol;
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
        timer = 0;

        transform.localPosition = returnPos;
        transform.localScale = returnScale;
        GetComponent<MeshRenderer>().material.color = returnCol;
        gameObject.SetActive(false);
    }

    void Update()
    {
    }

    public void StartEffect()
    {
        if (timer == 0.0f)
            StartCoroutine(TheEffect());
    }
}