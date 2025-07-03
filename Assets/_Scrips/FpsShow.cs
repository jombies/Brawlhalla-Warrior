using UnityEngine;
using UnityEngine.UI;

public class FpsShow : MonoBehaviour
{
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    private float totalFps = 0; // Tổng FPS để tính trung bình
    private int totalFrames = 0; // Tổng số khung hình để tính trung bình

    Text textFpsCounter;

    // Use this for initialization
    void Start()
    {
        textFpsCounter = GetComponent<Text>();
        timeleft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        float currentFps = Time.timeScale / Time.deltaTime;
        accum += currentFps;
        ++frames;

        // Tính tổng FPS và tổng số khung hình
        totalFps += currentFps;
        totalFrames++;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0) {
            // Tính FPS trung bình
            float averageFps = totalFps / totalFrames;

            // Hiển thị FPS hiện tại và trung bình
            string format = $"FPS: {Mathf.RoundToInt(accum / frames)} | Avg FPS: {Mathf.RoundToInt(averageFps)}";
            textFpsCounter.text = format;
            // Reset các giá trị cho khoảng thời gian tiếp theo
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
