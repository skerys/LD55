using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float speed;

    private Rigidbody _body;
    private Camera _mainCamera;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _body.velocity = transform.forward * speed;
        
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 cameraDir = _mainCamera.transform.forward;
        
        Vector3 a = Vector3.ProjectOnPlane(Vector3.forward, cameraDir);
        Vector3 b = Vector3.ProjectOnPlane(transform.forward, cameraDir);
        
        float zAngle = Vector3.SignedAngle(a, b, -cameraDir);
        
        spriteTransform.rotation = Quaternion.Euler(30f, 0f, -zAngle);
    }
}
