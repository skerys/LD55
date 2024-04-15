using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyGame : MonoBehaviour
{
    [SerializeField] private LayerMask babyMouseLayermask;
    [SerializeField] private SpriteRenderer babySprite;
    [SerializeField] private Sprite calmSprite;
    [SerializeField] private GameObject mouthGO;
    [SerializeField] private GameObject binkyGO;
    [SerializeField] private float calmingRequired;
    
    private Vector3 _anchorPosition;
    private Vector3 _targetPostion;
    
    private bool _mousedOver;
    private bool _pickedUp;
    private Camera _mainCam;

    private Vector3 _previousPos;
    private Queue<float> _speed = new Queue<float>(50);
    private float _averageSpeed;

    private float _calming = 0f;
    
    private static readonly int Tint = Shader.PropertyToID("_Tint");

    void Start()
    {
        _anchorPosition = transform.position;
        _mainCam = Camera.main;
    }
    
    void Update()
    {
        if (_mousedOver && Input.GetMouseButtonDown(0))
        {
            _pickedUp = true;
        }

        if (_pickedUp && Input.GetMouseButtonUp(0))
        {
            _pickedUp = false;
        }

        if (_pickedUp)
        {
            Ray mouseRay = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(mouseRay, out hitInfo, 20f, babyMouseLayermask))
            {
                _targetPostion = hitInfo.point;
            }

            Vector3 projected = Vector3.ProjectOnPlane(transform.position, babySprite.transform.forward);

            if (_speed.Count == 50) _speed.Dequeue();
            
            _speed.Enqueue(Vector3.Distance(projected, _previousPos) / Time.deltaTime);

            _averageSpeed = 0f;
            foreach (var s in _speed)
            {
                _averageSpeed += s;
            }

            _averageSpeed /= _speed.Count;

            if (_averageSpeed > 1f && _averageSpeed < 3f)
            {
                _calming += Time.deltaTime;
                if (_calming > calmingRequired)
                {
                    CalmBaby();
                }
            }
            
            _previousPos = projected;

        }
        else
        {
            _targetPostion = _anchorPosition;
        }
        transform.position += (_targetPostion - transform.position) * (10f * Time.deltaTime);
    }

    private void CalmBaby()
    {
        mouthGO.SetActive(false);
        binkyGO.SetActive(true);

        babySprite.sprite = calmSprite;
    }
    
    private void OnMouseEnter()
    { 
        Debug.Log("mouseEnter");
        //GetComponent<SpriteRenderer>().material.SetColor(Tint, Color.gray);
        _mousedOver = true;
    }

    private void OnMouseExit()
    {
        Debug.Log("mouseExit");
        //GetComponent<SpriteRenderer>().material.SetColor(Tint, Color.black);
        _mousedOver = false;
    }
    
}