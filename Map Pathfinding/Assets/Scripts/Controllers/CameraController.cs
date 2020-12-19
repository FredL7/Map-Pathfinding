using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
  [SerializeField] private MapVue map;
  [Header("Position")]
  [SerializeField] private float speed = 80f;
  [Header("Zoom")]
  [SerializeField] private float minZoom = 0.1f;
  [SerializeField] private float maxZoom = 1f;
  [SerializeField] private float zoomModifier = 1f;

  private float zoom = 1f;

  void Update() {
    float xDelta = Input.GetAxis("Horizontal");
    float yDelta = Input.GetAxis("Vertical");
    if (xDelta != 0f || yDelta != 0f) {
      AdjustPosition(xDelta, yDelta);
    }

    float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
    if (zoomDelta != 0f)
      AdjustZoom(zoomDelta);
  }

  private void AdjustPosition(float xDelta, float yDelta) {
    Vector3 direction = -new Vector3(xDelta, yDelta, 0f).normalized;
    float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
    float distance = speed * damping * Time.deltaTime;

    Vector3 position = map.transform.localPosition;
    position += direction * distance;

    map.transform.localPosition = ClampPosition(position);
  }

  private Vector3 ClampPosition(Vector3 position) {
    position.x = Mathf.Clamp(position.x, -map.TopCorner.x, -map.BottomCorner.x);
    position.y = Mathf.Clamp(position.y, -map.TopCorner.y, -map.BottomCorner.y);

    return position;
  }

  private void AdjustZoom(float delta) {
    zoom = Mathf.Clamp01(zoom + delta * zoomModifier);

    float z = Mathf.Lerp(minZoom, maxZoom, zoom);
    map.transform.localScale = new Vector3(z, z, 1f);
  }
}
