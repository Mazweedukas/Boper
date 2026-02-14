using UnityEngine;
public enum SurfaceType
{
    Flesh,
    Metal,
    Wood,
    Stone,
    Concrete,
    Dirt,
    Glass,
    Grass,
    Blood
}

public class SurfaceMaterial : MonoBehaviour
{
    public SurfaceType surfaceType;
}