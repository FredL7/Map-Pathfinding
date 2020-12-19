public class GraphLink {
  public int Index { get { return connection.GetIndex(); } }

  private GraphConnection connection;
  private GraphNode a;
  private GraphNode b;
  public int IndexA { get { return a.Index; } }
  public int IndexB { get { return b.Index; } }
  public float Weight { get { return connection.GetWeight(); } }

  public GraphLink(GraphConnection connection, GraphNode a, GraphNode b) {
    this.connection = connection;
    this.a = a;
    this.b = b;
  }
}
