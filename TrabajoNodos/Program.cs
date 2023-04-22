using System.Collections;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

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
    class ListaException : Exception
        {
            public ListaException(string message) : base(message) { }
        }
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
    public void Clear() => Dispose();

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
    public void AñadeAlPrincipio(NodoListaDoblementeEnlazada<T> nuevo)
    {
        nuevo.Siguiente = Primero;
        nuevo.Anterior = null;
        Primero.Anterior = nuevo;
        Primero = nuevo;
        if (Longitud == 0) Ultimo = nuevo;
        Longitud++;
    }
    public void AñadeAlPrincipio(T dato)
    {
        NodoListaDoblementeEnlazada<T> nuevo = new NodoListaDoblementeEnlazada<T>(dato);
        nuevo.Siguiente = Primero;
        nuevo.Anterior = null;
        Primero.Anterior = nuevo;
        Primero = nuevo;
        if (Longitud == 0) Ultimo = nuevo;
        Longitud++;

    }
    public void AñadeAlFinal(NodoListaDoblementeEnlazada<T> nuevo)
    {
        if (Longitud == 0)
            Primero = nuevo;
        else
            Ultimo!.Siguiente = nuevo;
        nuevo.Anterior = Ultimo;
        Ultimo.Siguiente = nuevo;
        Ultimo = nuevo;
        Longitud++;
    }
    public void AñadeAlFinal(T dato)
    {
        NodoListaDoblementeEnlazada<T> nuevo = new NodoListaDoblementeEnlazada<T>(dato);
        if (Longitud == 0)
            Primero = nuevo;
        else
            Ultimo!.Siguiente = nuevo;
        nuevo.Anterior = Ultimo;
        Ultimo.Siguiente = nuevo;
        Ultimo = nuevo;
        Longitud++;
    }
    public void AñadeDespuesDe(NodoListaDoblementeEnlazada<T> nodo, NodoListaDoblementeEnlazada<T> nuevo)
    {
        nuevo.Siguiente = nodo.Siguiente;
        nuevo.Anterior = nodo;
        if (Longitud == 0)
        {
            Ultimo = nuevo;
        }
        Longitud++;
    }
    public void AñadeDespuesDe(NodoListaDoblementeEnlazada<T> nodo, T dato)
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
    public void AñadeAntesDe(NodoListaDoblementeEnlazada<T> nodo, NodoListaDoblementeEnlazada<T> nuevo)
    {
        nuevo.Anterior = nodo.Anterior;
        nuevo.Siguiente = nodo;
        if (Longitud == 0)
        {
            Ultimo = nuevo;
        }
        Longitud++;
    }
    public void AñadeAntesDe(NodoListaDoblementeEnlazada<T> nodo, T dato)
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
    public void Borra(NodoListaDoblementeEnlazada<T> nodo)
        {
            if (Longitud == 0)
                throw new ListaException("Para borrar en una lista tiene que tener algún elemento.");
            if (Longitud == 1)
                Primero = Ultimo = null;
            else if (Primero == nodo)
            {
                Primero = nodo.Siguiente;
                nodo.Siguiente.Anterior = null;
            }
            else if (Ultimo == nodo)
            {
                Ultimo = nodo.Anterior;
                nodo.Anterior.Siguiente = null;
            }
            else
            {
                NodoListaDoblementeEnlazada<T> a = Busca(nodo.Dato);
                if (a == null)
                    throw new Exception("El nodo a borrar no pertenece a la lista.");
                a.Anterior.Siguiente = nodo.Siguiente;
                a.Siguiente.Anterior = nodo.Anterior;
            }
            nodo.Siguiente = null;
            nodo.Anterior = null;
            Longitud--;
        }
        public void ImprimeLista(ListaDoblementeEnlazada<T> l)
        {
            foreach (var a in l)
                Console.WriteLine(a);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (NodoListaDoblementeEnlazada<T>? n = Primero; n != null; n = n.Siguiente)
            {
                sb.Append($"[{n.ToString()}]");
            }
            sb.Append(" - ");
            for (NodoListaDoblementeEnlazada<T>? n = Ultimo; n != null; n = n.Anterior)
            {
                sb.Append($"[{n.ToString()}]");
            }
            return new string(sb.ToString());
        }
        public void EditaNodo(T datoAnterior, T datoNuevo, string direccion)
        {
            NodoListaDoblementeEnlazada<T> nodo = Busca(datoAnterior);
            if (nodo == null)
                throw new Exception("El nodo a editar no pertenece a la lista.");
            NodoListaDoblementeEnlazada<T> nuevo = new NodoListaDoblementeEnlazada<T>(datoNuevo);
            nuevo.Anterior = nodo.Anterior;
            if(nodo.Siguiente != null)
            nodo.Siguiente.Anterior = nuevo;
            if(nodo.Anterior != null)
            nodo.Anterior.Siguiente = nuevo;
            nodo = nuevo;
        }
        

}

internal class Program
{
    private static void Main(string[] args)
    {
        ListaDoblementeEnlazada<int> ld = new ListaDoblementeEnlazada<int>();
            ld.AñadeAlPrincipio(4);
            ld.AñadeAlPrincipio(3);
            Console.WriteLine(ld);
            Console.ReadLine();
            ld.Clear();
            ld.AñadeAlFinal(6);
            ld.AñadeAlFinal(9);
            ld.AñadeAlPrincipio(3);
            Console.WriteLine(ld);
            Console.ReadLine();
            NodoListaDoblementeEnlazada<int> nodo = ld.Busca(6);
            ld.AñadeAntesDe(nodo, 5);
            ld.AñadeAntesDe(ld.Primero, 1);
            ld.AñadeDespuesDe(nodo, 7);
            ld.AñadeDespuesDe(ld.Ultimo, 12);
            Console.WriteLine(ld);
            ld.Borra(nodo);
            ld.Borra(ld.Primero);
            ld.Borra(ld.Ultimo);
            Console.WriteLine(ld);
            Console.ReadLine();
    }
}