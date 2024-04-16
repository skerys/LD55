using UnityEngine;


public class SpaceIndicator : MonoBehaviour
{
    [SerializeField] private GameObject arrowIndicator;

    [SerializeField] private GameObject spaceIndicator;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DemonController>())
        {
            spaceIndicator.SetActive(true);
            arrowIndicator.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DemonController>())
        {
            spaceIndicator.SetActive(false);
            arrowIndicator.SetActive(true);
        }
    }
}