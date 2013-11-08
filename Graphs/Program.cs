using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    class Program{
    
        static void Main(string[] args)
        {

            IGraph<int> myGraf;

            myGraf = new MatrixGraph<int>(true);
            //myGraf = new ListGraph<int>(false);
            myGraf.AddVertex(new Vertex<int>(0, 3));
            myGraf.AddVertex(new Vertex<int>(1, 2));
            myGraf.AddVertex(new Vertex<int>(2, 3));
            myGraf.AddVertex(new Vertex<int>(3, 2));
            myGraf.AddVertex(new Vertex<int>(4, 3));
            myGraf.AddVertex(new Vertex<int>(5, 3));
            myGraf.AddVertex(new Vertex<int>(6, 3));
            


           


            myGraf.AddEdge(new Edge<int>(myGraf.getVertexById(0), myGraf.getVertexById(4)));

            myGraf.AddEdge(new Edge<int>(myGraf.getVertexById(1), myGraf.getVertexById(3)));

            Edge<int> myEdge = new Edge<int>(myGraf.getVertexById(0), myGraf.getVertexById(1));

            Console.WriteLine(myGraf.ToString());


            Console.Write("\nTEST USUWANIE KRAWEDZI \n");
            myGraf.AddEdge(myEdge);
            Console.WriteLine(myGraf.ToString());
            myGraf.RemoveEdge(myEdge);
            Console.WriteLine(myGraf.ToString());


            myGraf.AddEdge(myEdge);
            Console.Write("\nTEST SASIEDZI WIERZCHOLKA 0  \n");
            List<Vertex<int>> zeroVertexNeighbours = myGraf.getVertexById(0).Neighbors;
            foreach (var vertex in zeroVertexNeighbours)
            {
                Console.Write(vertex.Id + ", ");
            }

            int t = Console.Read();
        }
    }
}
