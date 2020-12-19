public class Location : GraphElement {
  private static int INDEXER = 0;
  private int index;
  public int Index { get { return index; } set { index = value; } }
  public int GetIndex() { return index; }

  private string name;
  public string Name { get { return name; } }
  private Coordinates coordinates;
  public Coordinates Coordinates { get { return coordinates; } }
  private LocationType type;
  public LocationType Type { get { return type; } }
  private int zone;
  public int Zone { get { return zone; } }

  private LocationVue vue;
  public LocationVue Vue { set { vue = value; } }
  // Not set for crossroads

  public Location(string n, Coordinates c, LocationType t, int z) {
    index = INDEXER++;
    coordinates = c;
    name = n;
    type = t;
    zone = z;
  }

  public void SelectAsMouvementOrigin() { vue.HighlightAsOrigin(); }
  public void SelectAsMouvementDestination() { vue.HighlightAsDestination(); }
}
