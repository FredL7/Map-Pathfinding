public class GraphManager {
  private Graph graph;

  public GraphManager(GraphElement[] elements, GraphConnection[] connections, MapVue vue) {
    graph = new Graph(elements, connections, vue);
  }

  public GraphElement NextJump(int origin, int destination) {
    GraphNode a = graph.GetNode(origin);
    GraphNode b = graph.GetNode(destination);

    int jump = a.GetJumpTo(b.Index);
    if (jump == -1)
      return null;

    return graph.GetNode(jump).Element;
  }
}
