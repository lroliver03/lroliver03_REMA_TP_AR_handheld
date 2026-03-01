using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BoardPlacement : MonoBehaviour
{
  [SerializeField]
  public GameObject gameBoardPrefab;

  private GameObject gameBoard = null;
  private ARPlaneManager m_ARPlaneManager;
  private ARRaycastManager m_ARRaycastManager;
  private List<ARRaycastHit> m_ARRaycastHits = new();

  void Start()
  {
    m_ARPlaneManager = GetComponentInParent<ARPlaneManager>();
    m_ARRaycastManager = GetComponentInParent<ARRaycastManager>();

    if (!m_ARPlaneManager || !m_ARRaycastManager)
      throw new Exception($"Managers not found in parent GameObject. Plane Manager is {m_ARPlaneManager} and Raycast Manager is {m_ARRaycastManager}.");

    EnhancedTouchSupport.Enable();
  }

  void HandleRaycast(ARRaycastHit hit)
  {
    if (!gameBoard)
      gameBoard = Instantiate(gameBoardPrefab);

    gameBoard.transform.position = hit.pose.position;
  }

  void Update()
  {
    if (Touch.activeTouches.Count <= 0)
      return;
    
    Touch touch = Touch.activeTouches[0];
    Debug.Log(touch);
    
    if (m_ARRaycastManager.Raycast(Touch.activeTouches[0].screenPosition, m_ARRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
    {
      HandleRaycast(m_ARRaycastHits[0]);
    }
  }
}
