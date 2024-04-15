using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenGame : MonoBehaviour
{
    [SerializeField] private HouseSceneController hsc;
    [SerializeField] private ImprovementLibrary improvementLibrary;
    [SerializeField] private Transform cabinetDoorLeft;
    [SerializeField] private Transform cabinetDoorRight;
    [SerializeField] private float doorOpenSpeedPercentage;
    [SerializeField] private float doorOpenAngle;
    [SerializeField] private Transform potLocation;

    [SerializeField] private Color uncookedColor;
    [SerializeField] private Color cookedColor;
    [SerializeField] private Material soupMat;
    [SerializeField] private Material particleMat;
    [SerializeField] private GameObject cookParticles;

    [SerializeField] private BabyTurretAnimation ladleAnim;
    [SerializeField] private float cookAnimTime = 3f;

    [SerializeField] private List<OvenIngredient> _ingredients = new List<OvenIngredient>();

    private List<Vector3> _initialPositions;
    
    private OvenIngredient _selectedIngredient = null;
    
    private Quaternion _doorLeftStart;
    private Quaternion _doorLeftEnd;
    private Quaternion _doorRightStart;
    private Quaternion _doorRightEnd;

    private float _targetRotation = 0f;
    private float _currentRotation = 0f;

    private bool _cookAnimStarted = false;
    private float _cookAnimTimeElapsed = 0f;

    private void Start()
    {
        particleMat.color = soupMat.color = uncookedColor;
        
        
        _doorLeftStart = cabinetDoorLeft.rotation;
        _doorRightStart = cabinetDoorRight.rotation;

        _doorLeftEnd = _doorLeftStart * Quaternion.Euler(Vector3.forward * -doorOpenAngle);
        _doorRightEnd = _doorRightStart * Quaternion.Euler(Vector3.forward * doorOpenAngle);

        _initialPositions = new List<Vector3>(new Vector3[_ingredients.Count]);
        for (int i = 0; i < _ingredients.Count; ++i)
        {
            _initialPositions[i] = _ingredients[i].transform.position;
        }
    }

    public void Update()
    {
        if (Mathf.Abs(_targetRotation - _currentRotation) > 0.01f)
        {
            _currentRotation += (_targetRotation - _currentRotation) * Mathf.Min(doorOpenSpeedPercentage * Time.deltaTime, 0.2f);
            cabinetDoorLeft.rotation = Quaternion.Lerp(_doorLeftStart, _doorLeftEnd, _currentRotation);
            cabinetDoorRight.rotation = Quaternion.Lerp(_doorRightStart, _doorRightEnd, _currentRotation);
        }

        if (_cookAnimStarted)
        {
            _cookAnimTimeElapsed += Time.deltaTime;

            particleMat.color = soupMat.color = Color.Lerp(uncookedColor, cookedColor, _cookAnimTimeElapsed / cookAnimTime);
            
            if (_cookAnimTimeElapsed > cookAnimTime)
            {
                ladleAnim.enabled = false;
                cookParticles.SetActive(false);
                hsc.SwitchToDefault();
                _cookAnimTimeElapsed = 0f;
                _cookAnimStarted = false;
            }
        }
    }
    public void StartOvenGame()
    {
        OpenCupboard();
    }

    public void EndOvenGame()
    {
        CloseCupboard();
        _cookAnimTimeElapsed = 0f;
    }

    void OpenCupboard()
    {
        _targetRotation = 1f;
    }

    void CloseCupboard()
    {
        _targetRotation = 0f;
    }

    public void PickIngredient(OvenIngredient ingredient)
    {
        if (!_selectedIngredient)
        {
            ingredient.MoveTo(potLocation.position);
            _selectedIngredient = ingredient;
            return;
        }

        if (_selectedIngredient != ingredient)
        {
            _selectedIngredient.MoveTo(_initialPositions[_ingredients.IndexOf(_selectedIngredient)]);
            ingredient.MoveTo(potLocation.position);
            _selectedIngredient = ingredient;
        }
    }

    private void OnDisable()
    {
        particleMat.color = soupMat.color = uncookedColor;
    }

    [ContextMenu("Do Cooking")]
    public void DoCooking()
    {
        improvementLibrary.EnableImprovement(_selectedIngredient.improvement);
        CloseCupboard();

        _cookAnimStarted = true;
        ladleAnim.enabled = true;
        cookParticles.SetActive(true);
        
        Destroy(_selectedIngredient.gameObject);
    }
}
