using System.Collections;

public class NodoListaDoblementeEnlazada<T> : IDisposable where T : IComparable<T>
{
    public NodoListaDoblementeEnlazada<T>? Siguiente { get; set; }
    public NodoListaDoblementeEnlazada<T>? Anterior { get; set; }
    public T Dato { get; }

    public NodoListaDoblementeEnlazada(T dato)
    {
        Siguiente = null;
        Dato = dato;
        Anterior = null;
    }

    public void Dispose()
    {
        Siguiente = null;
        Anterior = null;
        if (Dato is IDisposable d)
            d.Dispose();
    }
}

class ListaDoblementeEnlazada<T> : IDisposable, IEnumerable<T> where T : IComparable<T>
{
    public NodoListaDoblementeEnlazada<T>? Primero { get; private set; }
    public NodoListaDoblementeEnlazada<T>? Ultimo { get; private set; }
    public int Longitud { get; private set; }
    public bool Vacia => Longitud == 0;

    public ListaDoblementeEnlazada()
    {
        Primero = Ultimo = null;
        Longitud = 0;
    }
    public ListaDoblementeEnlazada(IEnumerable<T> secuencia)
    {
        foreach (T Dato in secuencia)
        {
            AñadeAlFinal();
        }
    }

    public void Dispose()
    {
        Primero = null;
        Ultimo = null;

        if (Primero is IDisposable d)
            d.Dispose();

        if (Ultimo is IDisposable u)
            u.Dispose();
    }

    public IEnumerator<T> GetEnumerator() => new Enumerador(Primero);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class Enumerador : IEnumerator<T>, IDisposable
    {

        private NodoListaDoblementeEnlazada<T>? It { get; set; }

        private NodoListaDoblementeEnlazada<T>? Primero { get; set; }

        public Enumerador(NodoListaDoblementeEnlazada<T>? primero)
        {
            Primero = primero;
            Reset();
        }

        public void Reset() => It = null;

        public T Current => It!.Dato;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            bool puedoIterar = It == null && Primero != null || It != null && It.Siguiente != null;
            if (puedoIterar) It = It == null ? Primero : It.Siguiente;
            return puedoIterar;
        }
        public void Dispose()
        {
            Primero = null;
            It = null;
        }
    }
    void AñadeDespuesDe(NodoListaDoblementeEnlazada<T> nodo, NodoListaDoblementeEnlazada<T> nuevo)
    {
        nuevo.Siguiente = nodo.Siguiente;
        nuevo.Anterior = nodo;
        if (Longitud == 0)
        {
            Ultimo = nuevo;
        }
        Longitud++;
    }
    void AñadeDespuesDe(NodoListaDoblementeEnlazada<T> nodo, T dato)
    {
        NodoListaDoblementeEnlazada<T> nuevo = new NodoListaDoblementeEnlazada<T>(dato);
        nuevo.Siguiente = nodo.Siguiente;
        nuevo.Anterior = nodo;
        if (Longitud == 0)
        {
            Ultimo = nuevo;
        }
        Longitud++;
    }
    void AñadeAntesDe(NodoListaDoblementeEnlazada<T> nodo, NodoListaDoblementeEnlazada<T> nuevo)
    {
        nuevo.Anterior = nodo.Anterior;
        nuevo.Siguiente = nodo;
        if (Longitud == 0)
        {
            Ultimo = nuevo;
        }
        Longitud++;
    }
    void AñadeAntesDe(NodoListaDoblementeEnlazada<T> nodo, T dato)
    {
        NodoListaDoblementeEnlazada<T> nuevo = new NodoListaDoblementeEnlazada<T>(dato);
        nuevo.Anterior = nodo.Anterior;
        nuevo.Siguiente = nodo;
        if (Longitud == 0)
        {
            Ultimo = nuevo;
        }
        Longitud++;
    }

}

internal class Program
{
    private static void Main(string[] args)
    {

    }
}