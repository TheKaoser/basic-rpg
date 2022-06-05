using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public List <AudioClip> sounds = new List<AudioClip>();
    List <AudioSource> audioSources = new List<AudioSource>();

    public void PlaySound (int sound, float delay = 0, bool loop = false, bool fadeIn = false)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.volume = 1;
        a.clip = sounds[sound];
        a.PlayDelayed(delay);
        StartCoroutine(CleanClip(a, delay + sounds[sound].length, loop, sound));
        if (loop)
        {
            a.loop = true;
        }
        if (fadeIn)
        {
            StartCoroutine(FadeIn(a));
        }
        audioSources.Add(a);
    }

    public void PauseSound (int sound)
    {
        foreach (AudioSource a in audioSources)
        {
            if (a.clip == sounds[sound])
            {
                a.Pause();
                a.clip = null;
                audioSources.Remove(a);
                Destroy (a);
                break;
            }
        }
    }

    IEnumerator FadeIn (AudioSource audioSource)
    {
        float currentTime = 0;
        audioSource.volume = 0;

        while (currentTime < 1f)
        {
            if (audioSource)
            {
                currentTime += 0.01f;
                audioSource.volume = Mathf.Lerp(audioSource.volume, 1f, currentTime / 1f);
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                break;
            }
        }
        yield return null;
    }

    IEnumerator CleanClip (AudioSource audioSource, float time, bool loop, int sound)
    {
        yield return new WaitForSeconds(time);
        if (audioSource && !loop && audioSource.clip == sounds[sound])
        {
            audioSource.clip = null;
            audioSources.Remove(audioSource);
            Destroy (audioSource);
        }
    }
}
