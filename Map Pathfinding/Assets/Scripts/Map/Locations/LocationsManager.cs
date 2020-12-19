using System.Collections;

public class LocationsManager : IEnumerable {
  private Matrix2d<Location> _locations;
  public int Count { get { return _locations.Count(); } }

  public LocationsManager(Matrix2d<Location> locations) {
    _locations = locations;
  }

  IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
  public MatrixEnum<Location> GetEnumerator() { return _locations.GetEnumerator(); }
}
