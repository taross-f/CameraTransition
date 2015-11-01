using UnityEngine;

// Attach to camera to be transitable
public class CameraId : MonoBehaviour {
    [SerializeField]int id;
    public int Id { get { return id; } }
}
