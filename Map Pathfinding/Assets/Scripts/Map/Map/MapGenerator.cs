using System.Collections.Generic;

public class MapGenerator {
  private Matrix2d<Location> locationsAndCrossroads;
  private Matrix2d<Location> locations;
  private Matrix2d<Location> crossroads;
  private List<Path> pathsList;

  public Matrix2d<Location> LocationsGrid { get { return locations; } }
  public Matrix2d<Location> CrossroadsGrid { get { return crossroads; } }
  public Path[] PathsList { get { return pathsList.ToArray(); } }

  public MapGenerator() {
    locationsAndCrossroads = new Matrix2d<Location>(MapMetrics.width, MapMetrics.height);
    locations = new Matrix2d<Location>(MapMetrics.width, MapMetrics.height);
    crossroads = new Matrix2d<Location>(MapMetrics.width, MapMetrics.height);
    pathsList = new List<Path>();

    GenerateLocations();
    GenerateCrossroads();
    GeneratePaths();
    RemoveLocationsWithoutPaths();
  }

  // Locations
  private void GenerateLocations() {
    for (int zone_x = 0; zone_x < MapMetrics.nbZones; zone_x++)
      for (int zone_y = 0; zone_y < MapMetrics.nbZones; zone_y++)
        GenerateZone(zone_x, zone_y, zone_x * MapMetrics.nbZones + zone_y);
  }

  private void GenerateZone(int zone_x, int zone_y, int zone) {
    GenerateCapital(zone_x, zone_y, 6, zone);
    GenerateCities(zone_x, zone_y, 3, 2, zone);
    GenerateForests(zone_x, zone_y, 0.3f, zone);
    GenerateMountains(zone_x, zone_y, 3, zone);
  }

  private void GenerateCapital(int zone_x, int zone_y, int nbFarmsAroundCapital, int zone) {
    Coordinates capitalCoordinates = SelectCoordinateInRange(zone_x, zone_y, true);
    AddLocation("Capital", capitalCoordinates, LocationType.CAPITAL, zone);

    GenerateFarmland(capitalCoordinates, nbFarmsAroundCapital, zone);
  }

  private void GenerateFarmland(Coordinates point, int nb, int zone) {
    for (int i = 0; i < nb; i++) {
      Coordinates farmlandCoordinates = SelectCoordinateAround(point.x, point.y);
      AddLocation("Farmland", farmlandCoordinates, LocationType.FARMLAND, zone);
    }
  }

  private void GenerateCities(int zone_x, int zone_y, int nbCities, int nbFarmsAroundCity, int zone) {
    for (int i = 0; i < nbCities; i++) {
      Coordinates cityCoordinates;
      do {
        cityCoordinates = SelectCoordinateInRange(zone_x, zone_y);
      } while (CountEmptySpaceInRange(cityCoordinates.x - 1, cityCoordinates.x + 1, cityCoordinates.y - 1, cityCoordinates.y + 1) < 2);
      AddLocation("City", cityCoordinates, LocationType.CITY, zone);

      GenerateFarmland(cityCoordinates, nbFarmsAroundCity, zone);
    }
  }

  private void GenerateForests(int zone_x, int zone_y, float coverPerc, int zone) {
    int nbFreeSpace = CountEmptySpaceInRange(
      zone_x * MapMetrics.zoneWidth, zone_x * MapMetrics.zoneWidth + MapMetrics.zoneWidth,
      zone_y * MapMetrics.zoneHeight, zone_y * MapMetrics.zoneHeight + MapMetrics.zoneHeight
    );
    int nbForests = (int)(nbFreeSpace * coverPerc);

    for (int i = 0; i < nbForests; i++) {
      Coordinates forestCoordinates = SelectCoordinateInRange(zone_x, zone_y);
      AddLocation("Forest", forestCoordinates, LocationType.FOREST, zone);
    }
  }

  private void GenerateMountains(int zone_x, int zone_y, int nbMountains, int zone) {
    for (int i = 0; i < nbMountains; i++) {
      Coordinates mountainCoordinates = SelectCoordinateInRange(zone_x, zone_y);
      AddLocation("Mountain", mountainCoordinates, LocationType.MOUNTAIN, zone);
    }
  }

  private Coordinates SelectCoordinateInRange(int zone_x, int zone_y, bool padding = false) {
    int min_x = zone_x * MapMetrics.zoneWidth + (padding ? MapMetrics.zonePadding : 0);
    int max_x = zone_x * MapMetrics.zoneWidth + MapMetrics.zoneWidth - (padding ? MapMetrics.zonePadding : 0);
    int min_y = zone_y * MapMetrics.zoneHeight + (padding ? MapMetrics.zonePadding : 0);
    int max_y = zone_y * MapMetrics.zoneHeight + MapMetrics.zoneHeight - (padding ? MapMetrics.zonePadding : 0);

    List<Coordinates> coordinates = new List<Coordinates>();
    for (int x = min_x; x < max_x; x++)
      for (int y = min_y; y < max_y; y++)
        if (locations[x, y] == null)
          coordinates.Add(new Coordinates(x, y));

    CollectionsHelper<Coordinates>.Shuffle(coordinates);
    return coordinates[0];
  }

  private Coordinates SelectCoordinateAround(int posX, int posY) {
    List<Coordinates> coordinates = new List<Coordinates>();
    for (int dx = -1; dx <= 1; dx++)
      for (int dy = -1; dy <= 1; dy++)
        if (dx != 0 || dy != 0)
          if (posX + dx >= 0 && posX + dx < MapMetrics.width && posY + dy >= 0 && posY + dy < MapMetrics.height)
            if (locations[posX + dx, posY + dy] == null)
              coordinates.Add(new Coordinates(posX + dx, posY + dy));

    CollectionsHelper<Coordinates>.Shuffle(coordinates);
    return coordinates[0];
  }

  private int CountEmptySpaceInRange(int minx, int maxx, int miny, int maxy) {
    List<Coordinates> coordinates = new List<Coordinates>();
    for (int x = minx; x < maxx; x++)
      for (int y = miny; y < maxy; y++)
        if (x >= 0 && x < MapMetrics.width && y >= 0 && y < MapMetrics.height)
          if (locations[x, y] == null)
            coordinates.Add(new Coordinates(x, y));

    return coordinates.Count;
  }

  private void AddLocation(string locationName, Coordinates coordinates, LocationType type, int zone) {
    Location location = new Location(locationName, coordinates, type, zone);
    locations[coordinates.x, coordinates.y] = location;
    locationsAndCrossroads[coordinates.x, coordinates.y] = location;
  }

  // Crossroads
  private void GenerateCrossroads() {
    foreach (Location l in locations) {
      int x = l.Coordinates.x;
      int y = l.Coordinates.y;

      // North
      if (y + 2 < MapMetrics.height)
        if (locations[x, y + 2] != null && locations[x, y + 1] == null)
          AddCrossroad(x, y + 1, l.Zone);

      // West
      if (x - 2 >= 0)
        if (locations[x - 2, y] != null && locations[x - 1, y] == null)
          AddCrossroad(x - 1, y, l.Zone);

      // NorthWest
      if (y + 2 < MapMetrics.height && x - 2 > 0)
        if (locations[x - 1, y + 1] == null && locations[x - 2, y + 2] != null)
          AddCrossroad(x - 1, y + 1, l.Zone);

      // NorthEast
      if (y - 2 >= 0 && x - 2 > 0)
        if (locations[x - 1, y - 1] == null && locations[x - 2, y - 2] != null)
          AddCrossroad(x - 1, y - 1, l.Zone);
    }
  }

  private void AddCrossroad(int x, int y, int zone) {
    Coordinates coordinates = new Coordinates(x, y);
    Location location = new Location("X", coordinates, LocationType.CROSSROAD, zone);
    crossroads[coordinates.x, coordinates.y] = location;
    locationsAndCrossroads[coordinates.x, coordinates.y] = location;
  }

  // Paths
  private void GeneratePaths() {
    foreach (Location l in locationsAndCrossroads) {
      for (int dx = -1; dx <= 1; dx++) {
        for (int dy = -1; dy <= 1; dy++) {
          if (dx == 0 && dy == 0)
            continue;

          int x = l.Coordinates.x + dx;
          int y = l.Coordinates.y + dy;
          if (x >= 0 && y >= 0 && x < MapMetrics.width && y < MapMetrics.height) {
            Location n = locationsAndCrossroads[x, y];
            if (n != null && l.Index > n.Index) {
              AddPath(l, n);
            }
          }
        }
      }
    }
  }

  private void AddPath(Location l, Location n) {
    Path path = new Path(l, n);
    pathsList.Add(path);
  }

  // Fixes
  private void RemoveLocationsWithoutPaths() {
    Matrix2d<bool> hasPathTo = new Matrix2d<bool>(MapMetrics.width, MapMetrics.height);
    foreach (Path path in pathsList) {
      hasPathTo[path.a.Coordinates.x, path.a.Coordinates.y] = true;
      hasPathTo[path.b.Coordinates.x, path.b.Coordinates.y] = true;
    }

    for (int x = 0; x < hasPathTo.GetLength(0); x++) {
      for (int y = 0; y < hasPathTo.GetLength(1); y++) {
        if (!hasPathTo[x, y] && locationsAndCrossroads[x, y] != null) {
          // UnityEngine.Debug.Log("Removed " + locationsAndCrossroads[x, y].Type + " at " + locationsAndCrossroads[x, y].Coordinates);
          locationsAndCrossroads[x, y] = null;
          locations[x, y] = null;
          crossroads[x, y] = null;
        }
      }
    }

    FixIndexes();
  }

  private void FixIndexes() {
    int i = 0;
    foreach (Location location in locationsAndCrossroads) {
      location.Index = i++;
    }
  }
}
