using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.SetDirectAudioVolume(0, AudioManager.Instance.GetMasterVolume());
        AudioManager.Instance.StopAll();
    }

    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp) 
    {
        Application.Quit();
    }
}
