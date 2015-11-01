using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// control camera transition
public class CameraTransition : MonoBehaviour {
    // timespan to transition
    [SerializeField]float transitionSecond = 1;
    Dictionary<int, Camera> cameras = new Dictionary<int, Camera>();
    int currentId;
    // default camera id
    const int DefaultId = 0;
    bool transiting;
	void Start () 
    {
        // ensure that camera id does not same each other
        cameras = GameObject.FindObjectsOfType<CameraId>()
            .ToDictionary(x => x.Id, x => x.GetComponent<Camera>());
        
        // ensure default id camera exist
        cameras.First(x => x.Key == DefaultId)
            .Value
            .gameObject
            .SetActive(true);
        
        cameras.Where(x => x.Key != DefaultId)
            .Select(x => x.Value)
            .ToList()
            .ForEach(x => x.gameObject.SetActive(false));
    }

    public void Transit(int nextId)
    {
        if (nextId == currentId) return;
        if (transiting) return;
        
        StartCoroutine(TransitCore(nextId));
    }
    
    private IEnumerator TransitCore(int nextId)
    {
        transiting = true;
        
        var currentCamera = cameras[currentId];
        var originPosition = currentCamera.transform.position;
        var originRotation = currentCamera.transform.rotation;
        
        var nextCamera = cameras[nextId];
        
        var elapsedTime = 0f;
        while (elapsedTime < transitionSecond)
        {
            elapsedTime += Time.deltaTime;
            currentCamera.transform.position = Vector3.Lerp(originPosition, nextCamera.transform.position, elapsedTime / transitionSecond);
            currentCamera.transform.rotation = Quaternion.Slerp(originRotation, nextCamera.transform.rotation, elapsedTime / transitionSecond);
            yield return null;
        }
        transiting = false;
        
        currentId = nextId;
        nextCamera.gameObject.SetActive(true);
        currentCamera.gameObject.SetActive(false);
        currentCamera.transform.position = originPosition;
        currentCamera.transform.rotation = originRotation;
    }
}
