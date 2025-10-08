using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour
{

    public VideoPlayer videoPlayer; // Assign your VideoPlayer component here
    public int nextSceneBuildIndex; // Or the name of your next scene as a string

    void Start()
    {
        // Add a listener for the loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Load the next scene
        SceneManager.LoadScene(nextSceneBuildIndex); // Or SceneManager.LoadScene("YourNextSceneName");
    }

    void OnDestroy()
    {
        // Remove the listener when the object is destroyed
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}

