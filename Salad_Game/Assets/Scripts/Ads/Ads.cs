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
    private int lastAdCount = 0;

    void Start()
    {
        if (adScreen != null)
            adScreen.SetActive(false);
    }

    void Update()
    {
        if (MatchGenerator.Instance.likesNumber > 0)
        {
            int likeCount = MatchGenerator.Instance.likesNumber;

            if (likeCount % 3 == 0 && likeCount != lastAdCount)
            {
                PlayRandomAd();
                lastAdCount = likeCount;
            }
        }
    }

    public void PlayRandomAd()
    {
        if (isPlaying) return;

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

    private void OnVideoEnded(VideoPlayer vp)
    {
        isPlaying = false;
        videoPlayer.loopPointReached -= OnVideoEnded;
        videoPlayer.Stop();

        if (adScreen != null)
            adScreen.SetActive(false);
    }
}
