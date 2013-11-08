using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{

    //grafy z krawedziami wazonymi i zwyklymi
    //grafy na matrixie i liscie
    //

     


    class Vertex<T>
    {
        private int id;
        public int Id { get { return id; } }

        private T value;
        public T Value { get { return value; } }
        public List<Vertex<T>> neighbors;
        public List<Vertex<T>> Neighbors
        {
            get
            {
                if (neighbors == null)
                {
                    neighbors = new List<Vertex<T>>();
                    return neighbors;
                }
                else return neighbors;
            }
        }

        public Vertex(int _id, T _val)
        {

            id = _id;
            value = _val;
        }


    }


    class Edge<T>
    {
        private Vertex<T> fromVertex;
        public Vertex<T> FromVertex { get { return fromVertex; } }

        private Vertex<T> toVertex;
        public Vertex<T> ToVertex { get { return toVertex; } }

        public Edge(Vertex<T> from, Vertex<T> to)
        {
            fromVertex = from;
            toVertex = to;
        }


    }

    class WeightedEdge<T> : Edge<T>
    {
        private double weight;
        public double Weight { get { return weight; } }

        public WeightedEdge(Vertex<T> from, Vertex<T> to, double _weight)
            : base(from, to)
        {
            weight = _weight;
        }


    }

    interface IGraph<T>
    {
         void AddEdge(Edge<T> edge);

         void RemoveEdge(Edge<T> edge);

         void AddVertex(Vertex<T> vertex);

         void RemoveVertex(Vertex<T> vertex);
         string printVertexes();
         Vertex<T> getVertexById(int id);
         
         

      //  Edge<T> getEdge(Vertex<T> from, Vertex<T> to);

          


    }



    class MatrixGraph<T> : IGraph<T>
    {

        List<Vertex<T>> vertexList;
       // List<Edge<T>> edgeList;
        bool digraf;
        int[,] aMatrix;
        int highestIndex = 0;

        public MatrixGraph(bool _digraph)
        {
            digraf = _digraph;
            vertexList = new List<Vertex<T>>();
            aMatrix = new int[,]{};

        }

        public void AddVertex(Vertex<T> _vertex)
        {
            //if (_vertex.Id >= highestIndex)
            //{
            //    highestIndex = _vertex.Id;
            //    aMatrix = _resizeArray(aMatrix, highestIndex + 1, highestIndex + 1);
                
            //}

            vertexList.Add(_vertex);
        }

        public void AddEdge(Edge<T> edge)
        {
            int from = edge.FromVertex.Id;
            int to = edge.ToVertex.Id;
            if (!digraf)
            {


                if (highestIndex <= Math.Max(to, from))
                {
                    highestIndex = Math.Max(to, from);

                    aMatrix = _resizeArray(aMatrix, highestIndex + 1, highestIndex + 1);
                    aMatrix[from, to] = 1;
                }
                else { aMatrix[from, to] = 1; }

                //dodajemy sasiadow dla wierzcholka

                edge.FromVertex.Neighbors.Add(edge.ToVertex);
               


             
            }
            else
            {
                if (highestIndex <= Math.Max(to, from))
                {
                    highestIndex = Math.Max(to, from);

                    aMatrix = _resizeArray(aMatrix, highestIndex + 1, highestIndex + 1);
                    aMatrix[from, to] = 1;
                    aMatrix[to, from] = 1;
                }
                else
                { 
                    aMatrix[from, to] = 1;
                    aMatrix[to, from] = 1; 
                }

                //dodajemy sasiadow dla wierzcholka

                edge.FromVertex.Neighbors.Add(edge.ToVertex);
                edge.ToVertex.Neighbors.Add(edge.FromVertex);
               
            }

        }

        public Vertex<T> getVertexById(int id)
        {
            return vertexList.FirstOrDefault(x => x.Id == id);
        }

        public void RemoveVertex(Vertex<T> vertex)
        {
            vertexList.Remove(vertex);
        }

        public void RemoveEdge(Edge<T> edge)
        {
            int from = edge.FromVertex.Id;
            int to = edge.ToVertex.Id;

            if (!digraf)
            {
                //jezeli krawedz istieje
                if (aMatrix[from, to] == 1)
                {
                    aMatrix[from, to] = 0;
                }
                //usun sasiada wierzcholka
                edge.FromVertex.Neighbors.Remove(edge.ToVertex);

            }
            else
            {
                if (aMatrix[from, to] == 1 && aMatrix[to, from] == 1)
                {
                    aMatrix[from, to] = 0;
                    aMatrix[to, from] = 0;
                }

                //usun sasiadow
                edge.FromVertex.Neighbors.Remove(edge.ToVertex);
                edge.ToVertex.Neighbors.Remove(edge.FromVertex);
            }


        }


      ////  public Edge<T> getEdge(Vertex<T> from, Vertex<T> to)
      //  { 
      //      int fromId 
      //  }

        public string printVertexes()
        {
            string tmp="";
            for (int i = 0; i < highestIndex; i++)
            {
                for (int j = 0; j < highestIndex; j++)
                {
                    System.Console.Write(aMatrix[i, j]);

                }

            }



            foreach (var vertex in vertexList)
            {
                tmp += vertex.Id;
            }
            return tmp;
        }

        public override string ToString()
        {
            

            string grapRep = "";
            grapRep += "\t";
            for (int i = 0; i < aMatrix.GetLength(1); i++)
                grapRep += "\t(" + i + ")";
            grapRep += "\n";

            for (int i = 0; i < aMatrix.GetLength(1); i++)
            {
                grapRep += "\t(" + i + ")";

                for (int j = 0; j < aMatrix.GetLength(1); j++)
                {

                    int val = aMatrix[i, j];
                    grapRep += "\t"+val;
                }
                grapRep += "\n";
            }

            return grapRep;
        }


        protected int[,] _resizeArray(int[,] original, int x, int y)
        {
            int[,] newArray = new int[x, y];
            int minX = Math.Min(original.GetLength(0), newArray.GetLength(0));
            int minY = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minY; ++i)
                Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);

            return newArray;
        }
        








    }
}

    //class ListGraph<T> : IGraph<T>{}

        
    //}
