using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{






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

        Vertex<T> getVertexById(int id);

        Edge<T> getEdge(int from, int to);

    }



    class MatrixGraph<T> : IGraph<T>
    {

        List<Vertex<T>> vertexList;
        
        bool digraf;
        int[,] aMatrix;
        int highestIndex = 0;

        public MatrixGraph(bool _digraph)
        {
            digraf = _digraph;
            vertexList = new List<Vertex<T>>();
            aMatrix = new int[,] { };

        }

        public void AddVertex(Vertex<T> _vertex)
        {
            

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

        public Edge<T> getEdge(int from, int to)
        {
            if (aMatrix[from, to] == 1)
            {
                Edge<T> edge = new Edge<T>(getVertexById(from), getVertexById(to));
                return edge;
            }
            else
                return null;
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
                    grapRep += "\t" + val;
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


    class ListGraph<T> : IGraph<T>
    {

        List<Vertex<T>> vertexList;
        Dictionary<int, List<Vertex<T>>> aList;
        bool digraph;
        int highestIndex;

        public ListGraph(bool _digraph)
        {
            digraph = _digraph;
            aList = new Dictionary<int, List<Vertex<T>>>();
            vertexList = new List<Vertex<T>>();
            highestIndex = 0;
        }

        public void AddEdge(Edge<T> edge)
        {
            Vertex<T> from = edge.FromVertex;
            Vertex<T> to = edge.ToVertex;

            if (Math.Max(from.Id, to.Id) >= highestIndex)
            {
                highestIndex = Math.Max(from.Id, to.Id);
            }

            if (!digraph)
            {
                if (aList.ContainsKey(from.Id))
                {
                    aList[from.Id].Add(to);


                }
                else
                {
                    aList[from.Id] = new List<Vertex<T>>();
                    aList[from.Id].Add(to);

                }

                //dodaj sasiadow
                from.Neighbors.Add(to);

            }
            else
            {
                //jesli digraf
                if (aList.ContainsKey(from.Id) && aList.ContainsKey(to.Id))
                {
                    aList[from.Id].Add(to);
                    aList[to.Id].Add(from);
                }
                else if (aList.ContainsKey(from.Id) && !aList.ContainsKey(to.Id))
                {
                    aList[to.Id] = new List<Vertex<T>>();
                    aList[from.Id].Add(to);
                    aList[to.Id].Add(from);
                }
                else if (aList.ContainsKey(to.Id) && !aList.ContainsKey(from.Id))
                {
                    aList[from.Id] = new List<Vertex<T>>();
                    aList[from.Id].Add(to);
                    aList[to.Id].Add(from);
                }
                else if (!aList.ContainsKey(to.Id) && !aList.ContainsKey(from.Id))
                {
                    aList[from.Id] = new List<Vertex<T>>();
                    aList[to.Id] = new List<Vertex<T>>();
                    aList[from.Id].Add(to);
                    aList[to.Id].Add(from);
                }

                //dodaj sasiadow
                from.Neighbors.Add(to);
                to.Neighbors.Add(from);
            }


        }

        public void RemoveEdge(Edge<T> edge)
        {
            Vertex<T> from = edge.FromVertex;
            Vertex<T> to = edge.ToVertex;




            if (aList.ContainsKey(from.Id))
            {
                if (!digraph)
                {
                    aList[from.Id].Remove(to);
                    from.neighbors.Remove(to);
                }
                else
                {
                    aList[from.Id].Remove(to);
                    aList[to.Id].Remove(from);
                    from.neighbors.Remove(to);
                    to.neighbors.Remove(from);
                }


            }





        }

        public void AddVertex(Vertex<T> vertex)
        {
            vertexList.Add(vertex);
        }

        public void RemoveVertex(Vertex<T> vertex)
        {
            vertexList.Remove(vertex);
        }

        public Vertex<T> getVertexById(int id)
        {
            return vertexList.FirstOrDefault(x => x.Id == id);
        }

        public Edge<T> getEdge(int from, int to)
        {
            if (aList.ContainsKey(from) && aList[from].Contains(getVertexById(to)))
            {

                Edge<T> theEdge = new Edge<T>(getVertexById(from), getVertexById(to));
                return theEdge;

            }
            else
            {
                throw new Exception("Nie ma takiej krawedzi");
            }
        }

        public override string ToString()
        {
            string tmp = "";
            for (int i = 0; i < highestIndex; i++)
            {
                tmp += "\t " + i + ": \t";
                if (aList.ContainsKey(i))
                {
                    foreach (var vertex in aList[i])
                    {
                        tmp += vertex.Id + ", ";
                    }
                    tmp += "\n";
                }
                else
                    tmp += "-\n";
            }
            return tmp;
        }
    }


}





