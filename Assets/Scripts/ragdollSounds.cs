using UnityEngine;
using System.Collections;

public class ragdollSounds : MonoBehaviour
{
    private float timer = -1;
    private Vector2 clickPoistion;
    private bool playOnce = true;
    private bool attachSound = false;
    private Vector2 previousPosition;
    private AudioSource spring;
    private AudioSource springBack;
    private AudioSource explosion;
    private AudioSource attach;
    private AudioSource click;
    private AudioSource springBackMinor;
    private float distance;


    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        spring = audios[0];
        springBack = audios[1];
        explosion = audios[2];
        attach = audios[3];
        click = audios[4];
        springBackMinor = audios[5];

        spring.volume = 0.5f;
        click.volume = 0.3f;
        attach.volume = 0.7f;
    }

    void OnMouseDown()
    {
        clickPoistion = Input.mousePosition;
        previousPosition = clickPoistion;
        click.Play();
    }

    void OnMouseUp()
    {
        if (attached())
        {
            if (distance < 20)
                return;
            else
            {
                spring.Stop();
                if (distance < 60)
                    springBackMinor.Play();
                else
                    springBack.Play();
            }
        }
    }


    void OnMouseDrag()
    {
        if (!attached() && playOnce)
        {
            explosion.Play();
            playOnce = false;
            attachSound = true;
        }
        else if (attached())
        {
            playOnce = true;
            Vector2 currPosition = Input.mousePosition;
            if (timer < 0 && currPosition != previousPosition)
            {
                spring.Play();
                timer = 0.2f;
            }
            previousPosition = currPosition;
            distance = Vector2.Distance(currPosition, clickPoistion);
            spring.pitch = 1 + 0.015f * distance;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (attachSound && attached())
        {
            attach.Play();
            attachSound = false;
        }
    }

    bool attached()
    {
        return GetComponent<Draggable>().Attached;
    }

}
