using UnityEngine;

public struct HitResult
    {
        public Vector3 origin;
        public Vector3 hitPoint;
        public Vector3 normal;
        public bool hit;
        public Collider collider;
        public SurfaceMaterial surfaceMaterial;
    }