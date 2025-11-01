using UnityEngine;
using UnityEngine.Video;

public class SeamlessDualVideoLoop : MonoBehaviour
{
    public VideoPlayer playerA;
    public VideoPlayer playerB;
    public double preloadTime = 0.5; // seconds before end to preload next video

    private VideoPlayer activePlayer;
    private VideoPlayer nextPlayer;
    private bool isAActive = true;

    void Start()
    {
        // Initialize players
        activePlayer = playerA;
        nextPlayer = playerB;

        playerA.isLooping = false;
        playerB.isLooping = false;

        playerA.loopPointReached += OnLoopPointReached;
        playerB.loopPointReached += OnLoopPointReached;

        // Prepare and start Player A
        playerA.Prepare();
        playerA.prepareCompleted += (v) => playerA.Play();
    }

    void Update()
    {
        // Preload next video slightly before current one ends
        if (activePlayer.isPlaying && activePlayer.length - activePlayer.time <= preloadTime && !nextPlayer.isPrepared)
        {
            nextPlayer.time = 0;
            nextPlayer.Prepare();
        }
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        // When one ends, start the other immediately
        nextPlayer.Play();

        // Swap references
        isAActive = !isAActive;
        activePlayer = isAActive ? playerA : playerB;
        nextPlayer = isAActive ? playerB : playerA;

        // Stop the one that just finished and reset it for next cycle
        vp.Stop();
        vp.time = 0;
    }
}
