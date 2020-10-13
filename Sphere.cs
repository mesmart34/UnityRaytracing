using UnityEngine;

struct Sphere
{
    Vector3 position;
    float radius;
    Vector3 color;

    public Sphere(Vector3 position, float radius, Vector3 color)
    {
        this.position = position;
        this.color = color;
        this.radius = radius;
    }

    public static int GetSize()
    {
        var size = sizeof(float);
        return size * 3 + size * 1 + size * 3;
    }
};