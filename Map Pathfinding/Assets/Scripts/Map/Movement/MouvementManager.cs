using System.Collections.Generic;

public class MouvementManager {
  // Singleton
  private static MouvementManager instance;
  public static MouvementManager Instance {
    get {
      if (instance == null)
        instance = new MouvementManager();
      return instance;
    }
  }

  private MouvementManager() { }

  // Observer
  List<MouvementObserver> observers = new List<MouvementObserver>();
  public void AddObserver(MouvementObserver o) { observers.Add(o); }
  private void Notify() {
    foreach (MouvementObserver observer in observers) {
      observer.UpdateMovement();
    }
  }

  // Model
  private Location origin;
  public Location Origin {
    get { return origin; }
    set {
      origin = (origin == value) ? null : value;

      if (origin == destination)
        destination = null;

      Notify();
    }
  }

  private Location destination;
  public Location Destination {
    get { return destination; }
    set {
      destination = (destination == value) ? null : value;

      if (destination == origin)
        origin = null;

      Notify();
    }
  }
}
