using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EarthPlacementWithinBoard : MonoBehaviour
{
  [Header("Setup")]
  public GameObject objectPrefab;
  public float objectScaleFactor;
  public float renderDistanceThreshold;

  private ARTrackedImageManager m_TrackedImageManager;
  private GameObject objectInstance;
  private GameObject gameBoardInstance;

  void Awake()
  {
    m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    if (m_TrackedImageManager == null)
      Debug.LogError("ARTrackedImageManager component NOT FOUND in XROrigin GameObject");
  }
  
  void OnEnable()
  {
    m_TrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
  }
  
  void OnDisable()
  {
    m_TrackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
  }

  void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
  {
    foreach (var newImage in args.added)
      OnTrackedImageChanged(newImage);

    foreach (var newImage in args.updated)
      OnTrackedImageChanged(newImage);
      
    foreach (var newImage in args.removed)
      DestroyObject();
  }

  void OnTrackedImageChanged(ARTrackedImage image)
  {
    if (image.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
    {
      DestroyObject();
      return;
    }

    if (gameBoardInstance == null)
    {
      gameBoardInstance = GameObject.FindGameObjectWithTag("GameBoard");
      if (gameBoardInstance == null)
        return;
    }

    float distanceToBoard = Vector3.Distance(
      image.transform.position, gameBoardInstance.transform.position
    );

    if (distanceToBoard <= renderDistanceThreshold)
    {
      if (objectInstance == null)
      {
        objectInstance = Instantiate(
          objectPrefab,
          image.transform.position,
          image.transform.rotation
        );

        Debug.Log(objectInstance.transform);
        Debug.Log(objectInstance.transform.localScale);
      }
    }
    else
    {
      DestroyObject();
    }
  }

  void DestroyObject()
  {
    if (objectInstance != null)
    {
      Destroy(objectInstance);
      objectInstance = null;
    }
  }
}
