public void AñadeAlPrincipio(NodoListaDoblementeEnlazada<T> nuevo)
{
    nuevo.Siguiente = Primero;
    nuevo.Anterior = null;
    Primero = nuevo;
    if (Longitud == 0) Ultimo = nuevo;
    Longitud++;
}
public void AñadeAlPrincipio(T dato)
{
    NodoListaDoblementeEnlazada<T> nuevo = new NodoListaDoblementeEnlazada<T>(dato);
    nuevo.Siguiente = Primero;
    nuevo.Anterior = null;
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
    Ultimo = nuevo;
    Longitud++;
}