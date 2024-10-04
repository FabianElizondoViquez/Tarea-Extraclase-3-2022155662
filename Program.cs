using System;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "bst_data.dat"; // Archivo en disco donde se guardarán los nodos
        BinarySearchTreeDisk tree = new BinarySearchTreeDisk(filePath);
        
        // Insertar algunos nodos
        Console.WriteLine("Insertando nodos...");
        tree.Insert(50, 0);   // Raíz del árbol
        tree.Insert(30, 0);
        tree.Insert(70, 0);
        tree.Insert(20, 0);
        tree.Insert(40, 0);

        // Buscar un nodo
        BSTNode result = tree.Search(30, 0);
        if (result != null && !result.deleted)
        {
            Console.WriteLine("Nodo encontrado: " + result.key);
        }
        else
        {
            Console.WriteLine("Nodo no encontrado.");
        }

        // Eliminar el nodo con clave 30
        Console.WriteLine("Eliminando el nodo con clave 30...");
        tree.Delete(30, 0);

        // Intentar buscar el nodo eliminado
        result = tree.Search(30, 0);
        if (result != null && !result.deleted)
        {
            Console.WriteLine("Nodo encontrado: " + result.key);
        }
        else
        {
            Console.WriteLine("Nodo no encontrado (marcado como eliminado).");
        }

        // Cerrar archivo
        tree.CloseFile();
    }
}
