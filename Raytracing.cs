using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raytracing : MonoBehaviour
{
    public int MinIterations = 4;
    public ComputeShader shader;
    private RenderTexture result;
    private List<Sphere> spheres = new List<Sphere>();
    private ComputeBuffer spheresBuffer;
    private Camera camera;
    private Material material;
    private int samples;
    public Texture skyboxTexture;
    public float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;
    public int reflectionsCount;
    private float piTime = 0;

    private void Start()
    {
        camera = GetComponent<Camera>();
        Init();
    }

    private void Update()
    {
        Look();
        if (samples > 300000)
            samples = 300000;
        if(transform.hasChanged)
        {
            samples = 0;
            transform.hasChanged = false;
        }
        piTime = (piTime + Time.deltaTime) % (Mathf.PI * 2);
    }

    private void Look()
    {
        //rotation.y += Input.GetAxis("Mouse X");
        //rotation.x += -Input.GetAxis("Mouse Y");
        //rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        //transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
        //transform.localRotation = Quaternion.Euler(rotation.x * lookSpeed, rotation.y * lookSpeed, 0);
    }

    private void Init()
    {
        material = new Material(Shader.Find("Hidden/AntiAliasing"));
        if(result == null)
            result = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat);
        result.enableRandomWrite = true;
        result.Create();

        var rad = 1.1f;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    spheres.Add(new Sphere(new Vector3(i * rad * 2, j * rad*2 + 5, k * rad*2), (Random.value + 0.1f) * 1.1f, new Vector3(Random.value, Random.value, Random.value)));
                }
            }
        }
        
        spheresBuffer = new ComputeBuffer(spheres.Count, Sphere.GetSize());
        spheresBuffer.SetData(spheres);
    }

    private void Render(RenderTexture destination)
    {
        var kernel = shader.FindKernel("CSMain");
        shader.SetBuffer(kernel, "Spheres", spheresBuffer);
        shader.SetMatrix("CameraToWorld", camera.cameraToWorldMatrix);
        shader.SetMatrix("CameraInverseProjection", camera.projectionMatrix.inverse);
        shader.SetTexture(kernel, "Result", result);
        shader.SetTexture(kernel, "SkyboxTexture", skyboxTexture);
        shader.SetInt("ReflectionsCount", reflectionsCount);
        shader.SetFloat("PiTime", piTime);
        for (int i = 0; i < MinIterations; i++)
        {
            material.SetFloat("_Sample", samples);
            shader.SetVector("PixelOffset", new Vector2(Random.value, Random.value));
            shader.Dispatch(
                kernel,
                Mathf.CeilToInt(Screen.width / 8.0f),
                Mathf.CeilToInt(Screen.height / 8.0f),
                1);
            Graphics.Blit(result, destination, material);
            samples++;
        } 
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Render(destination);   
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        if (spheresBuffer != null)
            spheresBuffer.Release();
    }
}
