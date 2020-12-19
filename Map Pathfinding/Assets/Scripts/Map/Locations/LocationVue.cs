using UnityEngine;
using UnityEngine.UI;

public class LocationVue : MonoBehaviour {
  [SerializeField] private Text textField;
  [SerializeField] private MeshRenderer meshRenderer;

  [Header("Type Colors")]
  [SerializeField] private Color capitalColor;
  [SerializeField] private Color cityColor, farmlandColor, forestColor, mountainColor, crossroadColor;

  [Header("Mouvement Colors")]
  [SerializeField] private Color mouvementOriginColor;
  [SerializeField] private Color mouvementDestinationColor;

  Location location;

  public void SetLocation(Location location) {
    this.location = location;
    DrawLocation();
  }

  private void DrawLocation() {
    string txt = "#" + location.Index + " " + location.Name + " " + "(" + location.Coordinates.x + "," + location.Coordinates.y + ")";
    name = "Location " + txt;
    textField.text = txt;
    DrawColor();
  }

  private void DrawColor() {
    switch (location.Type) {
      case LocationType.CAPITAL: meshRenderer.material.color = capitalColor; break;
      case LocationType.CITY: meshRenderer.material.color = cityColor; break;
      case LocationType.FARMLAND: meshRenderer.material.color = farmlandColor; break;
      case LocationType.FOREST: meshRenderer.material.color = forestColor; break;
      case LocationType.MOUNTAIN: meshRenderer.material.color = mountainColor; break;
      case LocationType.CROSSROAD: meshRenderer.material.color = crossroadColor; break;
    }
  }

  public void HighlightAsOrigin() { meshRenderer.material.color = mouvementOriginColor; }
  public void HighlightAsDestination() { meshRenderer.material.color = mouvementDestinationColor; }
  public void ResetMouvementHighlight() { DrawColor(); }

  public void OnMouseMainClick() {
    MouvementManager.Instance.Origin = location;
  }

  public void OnMouseSecondaryClick() {
    MouvementManager.Instance.Destination = location;
  }
}
