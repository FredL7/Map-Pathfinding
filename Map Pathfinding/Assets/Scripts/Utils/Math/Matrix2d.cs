using System;
using System.Collections;
// using System.Text;

public class Matrix2d<T> : IEnumerable {
  private T[,] storage;

  public Matrix2d(int size) { storage = new T[size, size]; }
  public Matrix2d(int m, int n) { storage = new T[m, n]; }

  public T this[int m, int n] {
    get { return storage[m, n]; }
    set { storage[m, n] = value; }
  }

  IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
  public MatrixEnum<T> GetEnumerator() { return new MatrixEnum<T>(storage); }

  /*public string ToStringDebug() {
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < storage.GetLength(0); i++) {
      for (int j = 0; j < storage.GetLength(1); j++) {
        sb.Append(storage[i, j] == null ? "0" : "1");
      }
      sb.Append("\n");
    }
    return sb.ToString();
  }*/

  public static Matrix2d<T> operator +(Matrix2d<T> a, Matrix2d<T> b) {
    Matrix2d<T> added = new Matrix2d<T>(a.storage.GetLength(0), a.storage.GetLength(1));
    for (int m = 0; m < a.storage.GetLength(0); m++) {
      for (int n = 0; n < a.storage.GetLength(1); n++) {
        if (a.storage[m, n] != null && b.storage[m, n] != null) {
          throw new InvalidOperationException("Matrices have element at same position");
        } else if (a.storage[m, n] != null) {
          added[m, n] = a.storage[m, n];
        } else if (b.storage[m, n] != null) {
          added[m, n] = b.storage[m, n];
        } else {
          added[m, n] = default(T);
        }
      }
    }

    return added;
  }

  public int GetLength(int i) { return storage.GetLength(i); }

  public int Count() {
    int i = 0;

    foreach (T t in this)
      i++;

    return i;
  }

  public T[] ToArray() {
    T[] arr = new T[Count()];

    int i = 0;
    foreach (T t in this) {
      arr[i++] = t;
    }

    return arr;
  }
}

public class MatrixEnum<T> : IEnumerator {
  private T[,] _storage;
  private int m = 0, n = -1;

  public MatrixEnum(T[,] storage) {
    _storage = storage;
  }

  public bool MoveNext() {
    do {
      n++;
      if (n >= _storage.GetLength(1)) {
        n = 0;
        m++;
      }
    } while (m < _storage.GetLength(0) && n < _storage.GetLength(1) && _storage[m, n] == null);

    return m < _storage.GetLength(0) && n < _storage.GetLength(1);
  }

  public void Reset() { m = -1; n = 0; }

  object IEnumerator.Current { get { return Current; } }
  public T Current {
    get {
      try {
        return _storage[m, n];
      } catch (IndexOutOfRangeException) {
        throw new InvalidOperationException("Index out of bounds");
      }
    }
  }
}
