using UnityEngine;

public class MapVue : MonoBehaviour {
  [Header("Prefabs")]
  [SerializeField] private LocationVue locationPrefab;
  [SerializeField] private CrossroadVue crossroadPrefab;
  [SerializeField] private PathVue pathPrefab;

  private Transform[] locationsParents;
  private Transform[] crossroadsParents;
  private Transform[] pathsParents;

  private LocationVue[] locationVues;
  // private CrossroadVue[] crossroadVues;
  private PathVue[] pathVues;

  public Vector3 BottomCorner { get { return ToWorldSpace(new Coordinates(0, 0)); } }
  public Vector3 TopCorner { get { return ToWorldSpace(new Coordinates(MapMetrics.width, MapMetrics.height)); } }

  public void DrawMap(Map map) {
    AddParentTransforms();

    locationVues = new LocationVue[map.Locations.Count];
    // crossroadVues = new CrossroadVue[map.Crossroads.Count];
    pathVues = new PathVue[map.Paths.Count];

    DrawLocations(map.Locations);
    DrawCrossroads(map.Crossroads);
    DrawPaths(map.Paths);

    // MouvementManager.Instance.AddObserver(this);
  }

  /*public void GraphReset() {
    foreach (PathVue path in pathVues) {
      path.GraphHardReset();
    }
  }
  public void GraphHighlightPath(int index) {
    foreach (PathVue path in pathVues) {
      path.GraphReset();
    }

    pathVues[index].GraphHighlight();
  }
  public void GraphRelaxPath(int index) { pathVues[index].GraphRelax(); }*/

  private void AddParentTransforms() {
    locationsParents = new Transform[MapMetrics.nbZonesTotal];
    crossroadsParents = new Transform[MapMetrics.nbZonesTotal];
    pathsParents = new Transform[MapMetrics.nbZonesTotal];

    for (int i = 0; i < MapMetrics.nbZonesTotal; i++) {
      Transform parent = AddTransform("Zone #" + i, transform);
      locationsParents[i] = AddTransform("Locations", parent);
      crossroadsParents[i] = AddTransform("Crossroads", parent);
      pathsParents[i] = AddTransform("Paths", parent);
      pathsParents[i].position = new Vector3(0, 0, 5f);
    }
  }

  private Transform AddTransform(string name, Transform parent) {
    GameObject t = new GameObject();
    t.transform.SetParent(parent, false);
    t.name = name;

    return t.transform;
  }

  // Locations
  private void DrawLocations(LocationsManager locations) {
    int i = 0;
    foreach (Location location in locations)
      InstantiateLocation(location, i++);
  }

  private void InstantiateLocation(Location location, int index) {
    LocationVue vue = Instantiate(locationPrefab);
    vue.transform.position = ToWorldSpace(location.Coordinates);
    vue.transform.SetParent(locationsParents[location.Zone], false);
    vue.SetLocation(location);

    location.Vue = vue;
    locationVues[index] = vue;
  }

  // Crossroads
  private void DrawCrossroads(CrossroadsManager crossroads) {
    foreach (Location crossroad in crossroads)
      InstantiateCrossroad(crossroad);
  }

  private void InstantiateCrossroad(Location crossroad) {
    CrossroadVue vue = Instantiate(crossroadPrefab);
    vue.transform.position = ToWorldSpace(crossroad.Coordinates);
    vue.transform.SetParent(crossroadsParents[crossroad.Zone], false);

    // crossroadVues[crossroad.Index] = vue;
  }

  // Paths
  private void DrawPaths(PathManager paths) {
    foreach (Path path in paths)
      InstantiatePath(path);
  }

  private void InstantiatePath(Path path) {
    Vector3 lVector = ToWorldSpace(path.a.Coordinates);
    Vector3 nVector = ToWorldSpace(path.b.Coordinates);
    Vector3 dVector = nVector - lVector;

    PathVue vue = Instantiate(pathPrefab);
    vue.transform.position = Vector3.Lerp(lVector, nVector, 0.5f);
    vue.transform.SetParent(pathsParents[path.a.Zone], false);
    vue.transform.localScale = new Vector3(dVector.magnitude, 1f, 1f);
    vue.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector3.SignedAngle(dVector, Vector3.right, -Vector3.forward)));

    path.Vue = vue;
    pathVues[path.Index] = vue;
  }

  private Vector3 ToWorldSpace(Coordinates c) {
    return new Vector3(c.x - MapMetrics.zoneWidth / 2f + 0.5f, c.y - MapMetrics.zoneHeight / 2f + 0.5f, 0f);
  }

  public void ResetLocationsHighlight() {
    foreach (LocationVue location in locationVues)
      location.ResetMouvementHighlight();

    foreach(PathVue path in pathVues)
      path.ResetHighlight();
  }
}
