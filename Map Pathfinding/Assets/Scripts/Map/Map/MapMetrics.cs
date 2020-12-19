public static class MapMetrics {
  public static int zoneWidth = 9;
  public static int zoneHeight = 9;
  public static int zonePadding = 3;
  public static int nbZones = 3;
  public static int nbZonesTotal { get { return nbZones * nbZones; } }
  public static int width { get { return nbZones * zoneWidth; } }
  public static int height { get { return nbZones * zoneHeight; } }
}
