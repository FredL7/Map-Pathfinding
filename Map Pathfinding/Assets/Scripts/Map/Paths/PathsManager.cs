using System.Collections;

public class PathManager : IEnumerable {
  private Path[] _paths;
  public int Count { get { return _paths.Length; } }

  public PathManager(Path[] paths) {
    _paths = paths;
  }

  IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
  public IEnumerator GetEnumerator() { return _paths.GetEnumerator(); }

  public Path GetPath(Location origin, Location destination) {
    foreach (Path path in _paths) {
      if ((path.a == origin && path.b == destination) || (path.a == destination && path.b == origin)) {
        return path;
      }
    }

    return null;
  }
}
