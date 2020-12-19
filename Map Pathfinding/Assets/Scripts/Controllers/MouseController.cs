using UnityEngine;

public class MouseController : MonoBehaviour {
  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit)) {
        LocationVue vue = hit.transform.gameObject.GetComponentInParent<LocationVue>();
        if (vue != null) {
          vue.OnMouseMainClick();
        }
      }
    }

    if (Input.GetMouseButtonDown(1)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit)) {
        LocationVue vue = hit.transform.gameObject.GetComponentInParent<LocationVue>();
        if (vue != null) {
          vue.OnMouseSecondaryClick();
        }
      }
    }
  }
}
