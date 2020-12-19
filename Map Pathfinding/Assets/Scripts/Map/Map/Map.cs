public class Map {
  private LocationsManager locationsManager;
  private CrossroadsManager crossroadsManager;
  private PathManager pathsManager;

  public LocationsManager Locations { get { return locationsManager; } }
  public CrossroadsManager Crossroads { get { return crossroadsManager; } }
  public PathManager Paths { get { return pathsManager; } }

  public Map(Matrix2d<Location> locations, Matrix2d<Location> crossroads, Path[] paths) {
    locationsManager = new LocationsManager(locations);
    crossroadsManager = new CrossroadsManager(crossroads);
    pathsManager = new PathManager(paths);
  }
}
