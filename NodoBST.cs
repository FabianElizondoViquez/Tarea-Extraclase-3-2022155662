public class BSTNode
{
    public int key;       // Clave del nodo
    public long left;     // Posición en el archivo del hijo izquierdo
    public long right;    // Posición en el archivo del hijo derecho
    public bool deleted;  // Indica si el nodo ha sido eliminado

    public BSTNode(int key)
    {
        this.key = key;
        this.left = -1;   // -1 indica que no hay hijo
        this.right = -1;
        this.deleted = false;
    }
}
