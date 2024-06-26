using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class OvenGame : MonoBehaviour
{
    public bool isActive = false;
    public bool isStartable = true;
    
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

    [SerializeField] private Button cookButton;

    [SerializeField] private BabyTurretAnimation ladleAnim;
    [SerializeField] private float cookAnimTime = 3f;

    [SerializeField] private List<OvenIngredient> _ingredients = new List<OvenIngredient>();
    [SerializeField] private List<GameObject> hints = new List<GameObject>();

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
        if (!GameStateManager.instance.OvenGameStartable)
        {
            isStartable = false;
            cookParticles.SetActive(false);
        }
        
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
        cookButton.interactable = _selectedIngredient;
        
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
                
                SoundManager.Instance.PlaySound(OneShotSoundTypes.Slurp);
                
                hsc.SwitchToDefault();
                _cookAnimTimeElapsed = 0f;
                _cookAnimStarted = false;
            }
        }
    }
    public void StartOvenGame()
    {
        OpenCupboard();
        cookButton.gameObject.SetActive(true);
    }

    public void EndOvenGame()
    {
        CloseCupboard();
        cookButton.gameObject.SetActive(false);
        _cookAnimTimeElapsed = 0f;

        isStartable = false;
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
        if (!isActive) return;
        
        SetHintActive(_ingredients.IndexOf(ingredient));
        SoundManager.Instance.PlaySound(OneShotSoundTypes.PepperPlop);
        
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

    private void SetHintActive(int index)
    {
        for (int i = 0; i < hints.Count; i++)
        {
            hints[i].SetActive(i == index);
        }
    }

    private void OnDisable()
    {
        particleMat.color = soupMat.color = uncookedColor;
    }

    [ContextMenu("Do Cooking")]
    public void DoCooking()
    {
        SoundManager.Instance.PlaySound(OneShotSoundTypes.Pop);
        improvementLibrary.EnableImprovement(_selectedIngredient.improvement);
        CloseCupboard();

        SoundManager.Instance.PlaySound(OneShotSoundTypes.StirPot);
        
        _cookAnimStarted = true;
        ladleAnim.enabled = true;
        cookParticles.SetActive(true);
        
        Destroy(_selectedIngredient.gameObject);
    }
}
