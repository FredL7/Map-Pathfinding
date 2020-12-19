using UnityEngine;

[RequireComponent(typeof(MapVue))]
public class MapManager : MonoBehaviour, IGame, MouvementObserver {
  private GraphManager graphManager;
  private Map map;
  private MapVue vue;

  void Awake() {
    vue = GetComponent<MapVue>();
    MouvementManager.Instance.AddObserver(this);
  }

  void Start() {
    // TODO: TEMPORARY
    NewGame();
  }

  public void NewGame() {
    MapGenerator generator = new MapGenerator();
    Matrix2d<Location> locationsGrid = generator.LocationsGrid;
    Matrix2d<Location> crossroadsGrid = generator.CrossroadsGrid;
    Path[] pathsList = generator.PathsList;

    map = new Map(locationsGrid, crossroadsGrid, pathsList);

    vue.DrawMap(map);

    graphManager = new GraphManager((locationsGrid + crossroadsGrid).ToArray(), pathsList, vue);
  }

  public void UpdateMovement() {
    vue.ResetLocationsHighlight();

    if (MouvementManager.Instance.Origin != null)
      MouvementManager.Instance.Origin.SelectAsMouvementOrigin();

    if (MouvementManager.Instance.Destination != null)
      MouvementManager.Instance.Destination.SelectAsMouvementDestination();

    if (MouvementManager.Instance.Origin != null && MouvementManager.Instance.Destination != null) {
      Location origin = MouvementManager.Instance.Origin;
      bool valid = true;
      do {
        Location nextJump = (Location)graphManager.NextJump(origin.Index, MouvementManager.Instance.Destination.Index);
        if (nextJump == null) {
          valid = false;
        } else {
          Path nextPath = map.Paths.GetPath(origin, nextJump);
          nextPath.HighlightForMovement();
          origin = nextPath.a == origin ? nextPath.b : nextPath.a;
        }
      } while (origin != MouvementManager.Instance.Destination & valid);
    }
  }
}
