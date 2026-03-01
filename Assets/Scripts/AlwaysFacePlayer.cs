using UnityEngine;

public class AlwaysFacePlayer : MonoBehaviour
{
  private GameObject mainCamera;

  void Start()
  {
    mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
  }

  void Update()
  {
    // Update local Euler angles to make plane always face player camera
    transform.localEulerAngles = new(90, mainCamera.transform.localEulerAngles.y - 180, 0);
  }
}
