using System;
using System.IO;

public class BinarySearchTreeDisk
{
    private FileStream fileStream;
    private const int NodeSize = 25; // Tamaño de cada nodo en bytes (incluyendo el flag de eliminación)

    public BinarySearchTreeDisk(string filePath)
    {
        // Abrir o crear archivo donde se guardarán los nodos
        fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    // Escribe un nodo en el archivo y devuelve la posición en disco
    public long WriteNode(BSTNode node)
    {
        long position = fileStream.Length; // Escribir al final del archivo
        fileStream.Seek(position, SeekOrigin.Begin);

        byte[] data = SerializeNode(node); // Serializar nodo
        fileStream.Write(data, 0, data.Length);
        
        return position;
    }

    // Escribe un nodo en una posición específica
    public void WriteNode(BSTNode node, long position)
    {
        fileStream.Seek(position, SeekOrigin.Begin);
        byte[] data = SerializeNode(node);
        fileStream.Write(data, 0, data.Length);
    }

    // Leer un nodo de una posición específica en disco
    public BSTNode ReadNode(long position)
    {
        if (position == -1) return null;

        fileStream.Seek(position, SeekOrigin.Begin);
        byte[] data = new byte[NodeSize]; 
        fileStream.Read(data, 0, NodeSize);

        return DeserializeNode(data); // Deserializar nodo
    }

    // Serializa un nodo a un array de bytes
    private byte[] SerializeNode(BSTNode node)
    {
        byte[] data = new byte[NodeSize];
        BitConverter.GetBytes(node.key).CopyTo(data, 0);
        BitConverter.GetBytes(node.left).CopyTo(data, 8);
        BitConverter.GetBytes(node.right).CopyTo(data, 16);
        data[24] = (byte)(node.deleted ? 1 : 0); // Flag de eliminación
        return data;
    }

    // Deserializa un array de bytes a un nodo
    private BSTNode DeserializeNode(byte[] data)
    {
        int key = BitConverter.ToInt32(data, 0);
        long left = BitConverter.ToInt64(data, 8);
        long right = BitConverter.ToInt64(data, 16);
        bool deleted = data[24] == 1;

        BSTNode node = new BSTNode(key) { left = left, right = right, deleted = deleted };
        return node;
    }

    // Función para insertar un nodo en el árbol
    public long Insert(int key, long position)
    {
        if (fileStream.Length == 0 && position == 0) 
        {
            // Si el archivo está vacío y es la primera inserción
            return WriteNode(new BSTNode(key));  // Crear la raíz
        }

        BSTNode currentNode = ReadNode(position);

        if (currentNode == null) 
        {
            // Si la posición está vacía, insertar el nuevo nodo
            return WriteNode(new BSTNode(key));
        }

        if (key < currentNode.key)
        {
            if (currentNode.left == -1) 
            {
                // Si no hay hijo izquierdo, insertar ahí
                currentNode.left = WriteNode(new BSTNode(key));
            } 
            else 
            {
                // Si ya hay un hijo, insertar recursivamente
                currentNode.left = Insert(key, currentNode.left);
            }
        }
        else if (key > currentNode.key)
        {
            if (currentNode.right == -1) 
            {
                // Si no hay hijo derecho, insertar ahí
                currentNode.right = WriteNode(new BSTNode(key));
            } 
            else 
            {
                // Si ya hay un hijo, insertar recursivamente
                currentNode.right = Insert(key, currentNode.right);
            }
        }

        // Actualizar el nodo actual en el archivo
        WriteNode(currentNode, position);
        return position;
    }

    // Función para eliminar un nodo (marcándolo como eliminado)
    public long Delete(int key, long position)
    {
        BSTNode currentNode = ReadNode(position);

        if (currentNode == null)
        {
            // Nodo no encontrado
            return position;
        }

        if (key < currentNode.key)
        {
            currentNode.left = Delete(key, currentNode.left);  // Buscar en subárbol izquierdo
        }
        else if (key > currentNode.key)
        {
            currentNode.right = Delete(key, currentNode.right); // Buscar en subárbol derecho
        }
        else
        {
            // Nodo encontrado, se marca como eliminado
            Console.WriteLine("Marcando nodo como eliminado: " + currentNode.key);
            currentNode.deleted = true;
            WriteNode(currentNode, position);  // Guardar nodo con la marca de eliminación
        }

        return position;
    }

    // Buscar un nodo
    public BSTNode Search(int key, long position)
    {
        BSTNode node = ReadNode(position);
        
        if (node == null || node.key == key)
        {
            return node; // Retorna el nodo encontrado o null si no existe
        }
        
        if (key < node.key)
        {
            return Search(key, node.left);
        }
        else
        {
            return Search(key, node.right);
        }
    }

    // Cierra el archivo
    public void CloseFile()
    {
        fileStream.Close();
    }
}
