using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionsHelper<T> {
  public static T RandomInList(List<T> list) { return list[Random.Range(0, list.Count)]; }
  public static int RandomIndexInList(List<T> list) { return Random.Range(0, list.Count); }
  public static void Shuffle(List<T> list) {
    for (int i = 0; i < 3; i++) {
      int n = list.Count;
      while (n > 1) {
        n--;
        int k = RandomIndexInList(list);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }
}
