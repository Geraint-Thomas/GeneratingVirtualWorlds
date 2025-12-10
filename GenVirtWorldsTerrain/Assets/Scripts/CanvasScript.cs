using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public Slider sliderViewDistance;
    public TextMeshProUGUI textViewDistance;
    public Slider sliderChunkHeight;
    public TextMeshProUGUI textChunkHeight;
    public Slider sliderChunkSize;
    public TextMeshProUGUI textChunkSize;
    public Slider sliderNoise;
    public TextMeshProUGUI textNoise;
    private World world;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get world script
        world = GameObject.Find("World").GetComponent<World>();

        //set initial values
        sliderViewDistance.value = VoxelData.viewDistanceInChunks;
        sliderChunkHeight.value = VoxelData.chunkHeight;
        sliderChunkSize.value = VoxelData.chunkWidth;
        sliderNoise.value = world.noiseScale *10;

        //setup listeners for slideers
        sliderViewDistance.onValueChanged.AddListener((v) =>
        {
            textViewDistance.text = "View Distance " +(Mathf.Floor(v)).ToString();
            VoxelData.viewDistanceInChunks = (int)(Mathf.Floor(v));
        });

        sliderChunkHeight.onValueChanged.AddListener((v) =>
        {
            textChunkHeight.text = "Chunk Height " + (Mathf.Floor(v)).ToString();
            VoxelData.chunkHeight = (int)(Mathf.Floor(v));
        });

        sliderChunkSize.onValueChanged.AddListener((v) =>
        {
            textChunkSize.text = "Chunk Size " + (Mathf.Floor(v)).ToString();
            VoxelData.chunkWidth = (int)(Mathf.Floor(v));
        });

        sliderNoise.onValueChanged.AddListener((v) =>
        {
            textNoise.text = "Noise Scale " + v.ToString("#.00");
            world.noiseScale = v/10;
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
