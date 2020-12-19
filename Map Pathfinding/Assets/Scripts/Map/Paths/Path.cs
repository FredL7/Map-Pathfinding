public class Path : GraphConnection {
  private static int INDEXER = 0;
  private int index;
  public int Index { get { return index; } set { index = value; } }
  public int GetIndex() { return index; }

  public Location a { get; private set; }
  public Location b { get; private set; }

  private PathVue vue;
  public PathVue Vue { set { vue = value; } }

  public Path(Location a, Location b) {
    index = INDEXER++;
    this.a = a; this.b = b;
  }

  public int GetIndexA() { return a.Index; }
  public int GetIndexB() { return b.Index; }
  public float GetWeight() {
    return (a.Coordinates - b.Coordinates).Magnitude;
  }

  public void HighlightForMovement() { vue.Highlight(); }
}
