using UnityEngine;

[System.Serializable]
public struct Coordinates {
  public int x { get; private set; }
  public int y { get; private set; }
  // public float SquaredMagnitude { get { return x * x + y * y; } }
  public float Magnitude { get { return Mathf.Sqrt(x * x + y * y); } }

  public Coordinates(int x, int y) {
    this.x = x; this.y = y;
  }

  public Coordinates(Coordinates coordinates, int dx, int dy) {
    this.x = coordinates.x + dx; this.y = coordinates.y + dy;
  }

  /*public int ManhattanDistanceTo(Coordinates other) {
    return Mathf.Abs(other.x - x) + Mathf.Abs(other.y - y);
  }*/

  public override string ToString() {
    return "(" + x + "," + y + ")";
  }

  /*public float SquareDistanceTo(Coordinates other) {
    return (other.x - x) * (other.x - x) + (other.y - y) * (other.y - y);
  }*/

  public static Coordinates operator -(Coordinates a, Coordinates b) {
    return new Coordinates(b.x - a.x, b.y - a.y);
  }
}
