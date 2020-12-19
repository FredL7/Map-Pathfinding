public class GraphNode {
  public int Index { get { return element.GetIndex(); } }

  private GraphElement element;
  public GraphElement Element { get { return element; } }

  private int[] jumpTable;

  public GraphNode(GraphElement element, int sizeElements) {
    this.element = element;

    jumpTable = new int[sizeElements];
    for (int i = 0; i < sizeElements; i++)
      jumpTable[i] = -1;
  }

  public void AddJump(int destination, int nextJump) {
    jumpTable[destination] = nextJump;
  }

  public int GetJumpTo(int destination) {
    return jumpTable[destination];
  }

  public string JumpsToString() {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    foreach(int jump in jumpTable) {
      sb.Append(jump + " ");
    }
    return sb.ToString();
  }

}
