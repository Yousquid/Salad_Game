using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class Ads : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject adScreen;
    public RawImage adImage;

    public VideoClip[] videoClips;

    private bool isPlaying = false;

    void Start()
    {
        if (adScreen != null)
            adScreen.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame && !isPlaying)
        {
            PlayRandomAd();
        }
    }

    void PlayRandomAd()
    {
        if (videoClips == null || videoClips.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, videoClips.Length);
        videoPlayer.clip = videoClips[randomIndex];

        if (adScreen != null)
            adScreen.SetActive(true);

        videoPlayer.Play();
        isPlaying = true;

        videoPlayer.loopPointReached += OnVideoEnded;
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        isPlaying = false;
        videoPlayer.loopPointReached -= OnVideoEnded;

        videoPlayer.Stop();
        if (adScreen != null)
            adScreen.SetActive(false);
    }
}
