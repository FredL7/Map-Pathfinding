using UnityEngine;

public class PathVue : MonoBehaviour {
  [SerializeField] private MeshRenderer meshRenderer;

  [SerializeField] private Color defaultColor;

  /*[Header("Graph Colors")]
  [SerializeField] private Color graphHighlightColor;
  [SerializeField] private Color GraphRelaxingColor;*/

  [Header("Mouvement Colors")]
  [SerializeField] private Color mouvementHiglight;

  /*bool graphHighlighted = false;

  public void GraphHardReset() {
    meshRenderer.material.color = defaultColor;
    graphHighlighted = false;
  }
  public void GraphReset() {
    if (!graphHighlighted) {
      meshRenderer.material.color = defaultColor;
    }
  }
  public void GraphHighlight() {
    meshRenderer.material.color = graphHighlightColor;
    graphHighlighted = true;
  }
  public void GraphRelax() { meshRenderer.material.color = GraphRelaxingColor; }*/

  public void ResetHighlight() { meshRenderer.material.color = defaultColor; }
  public void Highlight() { meshRenderer.material.color = mouvementHiglight; }
}
