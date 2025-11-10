using System;
using System.Threading.Tasks;
using UnityEngine;

public struct PlaySFXEvent
{
    public enum SFXType
    {
        SwipeRight,
        SwipeLeft,
        Match,
        Unlock,
        SuperLike,
        Report
    }
    public SFXType sfxType;
    public PlaySFXEvent(SFXType type)
    {
        sfxType = type;
    }
}
public class AudioManager : MonoBehaviour
{
    public AudioClip swipeRight;
    public AudioClip swipeLeft;
    public AudioClip match;
    public AudioClip unlock;
    public AudioClip superLike;
    public AudioClip report;
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
        
        EventBetter.Listen(this, (PlaySFXEvent e) =>
        {
            switch (e.sfxType)
            {
                case PlaySFXEvent.SFXType.SwipeRight:
                    PlaySwipeRight();
                    break;
                case PlaySFXEvent.SFXType.SwipeLeft:
                    PlaySwipeLeft();
                    break;
                case PlaySFXEvent.SFXType.Match:
                    PlayMatch();
                    break;
                case PlaySFXEvent.SFXType.Unlock:
                    PlayUnlock();
                    break;
                case PlaySFXEvent.SFXType.SuperLike:
                    PlaySuperLike();
                    break;
                case PlaySFXEvent.SFXType.Report:
                    PlayReport();
                    break;
            }
        });
    }

    public void PlaySwipeRight()
    {
        Play(swipeRight);
    }
    public void PlaySwipeLeft()
    {
        Play(swipeLeft, 0.5f);
    }
    public void PlayMatch()
    {
        Play(match);
    }
    public void PlayUnlock()
    {
        Play(unlock);
    }
    public void PlaySuperLike()
    {
        Play(superLike);
    }

    public void PlayReport()
    {
        Play(report);   
    }
    public void PlaySFX(AudioClip clip, float volume = 1)
    {
        Play(clip, volume);
    }

    private async void Play(AudioClip clip, float volume = 1)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.Play();
        
        await Task.Delay((int)(clip.length * 1000));
        
        if(Application.isPlaying)
            Destroy(source);
    }
}
