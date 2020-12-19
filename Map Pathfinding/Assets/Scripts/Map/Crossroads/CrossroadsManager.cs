using System.Collections;

public class CrossroadsManager : IEnumerable {
  private Matrix2d<Location> _crossroads;
  public int Count { get { return _crossroads.Count(); } }

  public CrossroadsManager(Matrix2d<Location> locations) {
    _crossroads = locations;
  }

  IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
  public MatrixEnum<Location> GetEnumerator() { return _crossroads.GetEnumerator(); }
}
