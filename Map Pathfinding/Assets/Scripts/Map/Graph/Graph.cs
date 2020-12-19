using System;
// using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
// using UnityEngine;

public class Graph {
  private GraphNode[] nodes;
  private GraphLink[] links;

  public GraphNode GetNode(int index) { return nodes[index]; }

  public Graph(GraphElement[] elements, GraphConnection[] connections, MapVue vue) {
    nodes = new GraphNode[elements.Length];
    for (int i = 0; i < elements.Length; i++)
      nodes[i] = new GraphNode(elements[i], elements.Length);

    links = new GraphLink[connections.Length];
    for (int i = 0; i < connections.Length; i++) {
      links[i] = new GraphLink(
        connections[i],
        nodes[connections[i].GetIndexA()],
        nodes[connections[i].GetIndexB()]
      );
    }

    // GenerateLinkTablesVue(vue);
    // GenerateLinkTables();
    GenerateLinkTablesParallel();
  }

  private void GenerateLinkTablesParallel() {
    Parallel.ForEach(nodes, (origin) => {
      bool[] visited = new bool[nodes.Length];
      visited[origin.Index] = true;

      List<Path> paths = new List<Path>();

      foreach (GraphLink link in links)
        if (link.IndexA == origin.Index || link.IndexB == origin.Index)
          paths.Add(new Path(origin, link));
      paths.Sort();

      while (paths.Count > 0) {
        Path shortest = paths[0];
        paths.RemoveAt(0);
        visited[shortest.Next] = true;
        origin.AddJump(shortest.Next, shortest.NextJump);

        foreach (GraphLink link in links)
          if ((link.IndexA == shortest.Next && !visited[link.IndexB]) || (link.IndexB == shortest.Next && !visited[link.IndexA]))
            paths.Add(new Path(shortest, link));
        paths.Sort();

        // remove potential paths if we've already found its shortest
        List<int> pathsToRemove = new List<int>();
        for (int i = 0; i < paths.Count; i++)
          if (visited[paths[i].Next])
            pathsToRemove.Add(i);
        pathsToRemove.Sort();
        for (int i = 0; i < pathsToRemove.Count; i++) {
          paths.RemoveAt(pathsToRemove[i]);
          for (int j = i; j < pathsToRemove.Count; j++) {
            pathsToRemove[j] -= 1;
          }
        }
      }
    });
  }

  /*private void GenerateLinkTables() {
    foreach (GraphNode origin in nodes) {
      bool[] visited = new bool[nodes.Length];
      visited[origin.Index] = true;

      List<Path> paths = new List<Path>();

      foreach (GraphLink link in links)
        if (link.IndexA == origin.Index || link.IndexB == origin.Index)
          paths.Add(new Path(origin, link));
      paths.Sort();

      while (paths.Count > 0) {
        Path shortest = paths[0];
        paths.RemoveAt(0);
        visited[shortest.Next] = true;

        foreach (GraphLink link in links)
          if ((link.IndexA == shortest.Next && !visited[link.IndexB]) || (link.IndexB == shortest.Next && !visited[link.IndexA]))
            paths.Add(new Path(shortest, link));
        paths.Sort();

        // remove potential paths if we've already found its shortest
        List<int> pathsToRemove = new List<int>();
        for (int i = 0; i < paths.Count; i++)
          if (visited[paths[i].Next])
            pathsToRemove.Add(i);
        pathsToRemove.Sort();
        for (int i = 0; i < pathsToRemove.Count; i++) {
          paths.RemoveAt(pathsToRemove[i]);
          for (int j = i; j < pathsToRemove.Count; j++) {
            pathsToRemove[j] -= 1;
          }
        }
      }
    }
  }*/

  /*private void GenerateLinkTablesVue(MapVue vue) {
    vue.StartCoroutine(Coroutine1(vue));
  }

  private IEnumerator Coroutine1(MapVue vue) {
    yield return new WaitForSeconds(0.5f);
    foreach (GraphNode origin in nodes) {
      bool[] visited = new bool[nodes.Length];
      visited[origin.Index] = true;

      List<Path> paths = new List<Path>();

      foreach (GraphLink link in links) {
        if (link.IndexA == origin.Index || link.IndexB == origin.Index) {
          paths.Add(new Path(origin, link));
          vue.GraphRelaxPath(link.Index);
        }
      }
      paths.Sort();

      while (paths.Count > 0) {
        Path shortest = paths[0];
        paths.RemoveAt(0);
        visited[shortest.Next] = true;

        vue.GraphHighlightPath(shortest.Nextpath);

        foreach (GraphLink link in links) {
          if ((link.IndexA == shortest.Next && !visited[link.IndexB]) || (link.IndexB == shortest.Next && !visited[link.IndexA])) {
            paths.Add(new Path(shortest, link));
            vue.GraphRelaxPath(link.Index);
          }
        }
        paths.Sort();

        // remove potential paths if we've already found its shortest
        List<int> pathsToRemove = new List<int>();
        for (int i = 0; i < paths.Count; i++)
          if (visited[paths[i].Next])
            pathsToRemove.Add(i);
        pathsToRemove.Sort();
        for (int i = 0; i < pathsToRemove.Count; i++) {
          paths.RemoveAt(pathsToRemove[i]);
          for (int j = i; j < pathsToRemove.Count; j++) {
            pathsToRemove[j] -= 1;
          }
        }

        yield return new WaitForSeconds(0.5f);
      }

      vue.GraphReset();
      yield return new WaitForSeconds(0.5f);
    }
  }*/

  private class Path : IComparable {
    private List<int> nodes;
    public List<int> paths;
    private float weight;
    // public float Weight { get { return weight; } }
    public int Next { get { return nodes[nodes.Count - 1]; } }
    public int Nextpath { get { return paths[nodes.Count - 1]; } }
    public int NextJump { get { return nodes[1]; } }

    public Path(GraphNode origin, GraphLink firstJump) {
      nodes = new List<int>();
      nodes.Add(origin.Index);
      paths = new List<int>();
      paths.Add(firstJump.Index);

      AddJump(firstJump);
    }

    public Path(Path path, GraphLink nextJump) {
      nodes = new List<int>();
      foreach (int node in path.nodes)
        nodes.Add(node);
      paths = new List<int>();
      foreach (int p in path.paths)
        paths.Add(p);

      weight = path.weight;

      AddJump(nextJump);
    }

    public void AddJump(GraphLink jump) {
      if (jump.IndexA == Next) {
        nodes.Add(jump.IndexB);
      } else {
        nodes.Add(jump.IndexA);
      }

      paths.Add(jump.Index);

      weight += jump.Weight;
    }

    public int CompareTo(object obj) {
      if (obj == null) return 1;

      Path otherPath = obj as Path;
      if (otherPath != null)
        return this.weight.CompareTo(otherPath.weight);
      else
        throw new ArgumentException("Object is not a Path");
    }

    public bool Contains(int index) {
      foreach (int node in nodes) {
        if (node == index)
          return true;
      }

      return false;
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      sb.Append("weight: " + weight + ", path : ");

      for (int i = 0; i < nodes.Count; i++) {
        sb.Append(nodes[i]);
        if (i < nodes.Count - 1) {
          sb.Append(" -> ");
        }
      }

      return sb.ToString();
    }
  }
}
