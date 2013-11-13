using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
       enum AvalibleVertexStates
        { 
            NotVisited, Visited, Completed
        }

       enum AvalibleEdgeTypes
        { 
            BackEdge, ForwardEdge, CrossEdge
        }

    class DFS<T>
    {

        private int time = 0;
        private Dictionary<Vertex<T>, AvalibleVertexStates> VertexStates;
        private Dictionary<Vertex<T>, int> TimeVisitingVertex;
        private Dictionary<Vertex<T>, int> TimeLeavingVertex;

        private List<Vertex<T>> VisitingOrder;

        private IGraph<T> graph;





        public DFS(IGraph<T> g)
        {
            time = 0;
            VertexStates = new Dictionary<Vertex<T>, AvalibleVertexStates>();
            TimeLeavingVertex = new Dictionary<Vertex<T>, int>();
            TimeVisitingVertex = new Dictionary<Vertex<T>, int>();
            VisitingOrder = new List<Vertex<T>>();
            graph = g;
        }
     

        public List<Vertex<T>> ExecuteDFS(Vertex<T> startingVertex)
        {
            
            time = 0;
            //inicializujemy  tablice
            foreach (var vertex in graph.Vertexes)
            {
                VertexStates[vertex] = AvalibleVertexStates.NotVisited;
            }
            //przechodzimy
            foreach (var vertex in graph.Vertexes)
            {
                if (VertexStates[vertex] == AvalibleVertexStates.NotVisited)
                {
                    DFSVisit(vertex);
                }
            }

            return VisitingOrder;
        }

        public void DFSVisit(Vertex<T> vertex)
        {
            //zwiekszamy czas
            time++;
            TimeVisitingVertex[vertex] = time;

            VisitingOrder.Add(vertex);
            VertexStates[vertex] = AvalibleVertexStates.Visited;

            foreach (var vertexNeighbour in vertex.Neighbors)
            {
                if (VertexStates[vertexNeighbour] == AvalibleVertexStates.NotVisited)
                {
                    DFSVisit(vertexNeighbour);
                }
            }
            VertexStates[vertex] = AvalibleVertexStates.Completed;
            time++;
            TimeLeavingVertex[vertex] = time;

        }

        public Dictionary<Edge<T>, AvalibleEdgeTypes> ClasifyEdges()
        {

            Dictionary<Edge<T>, AvalibleEdgeTypes> classification = new Dictionary<Edge<T>, AvalibleEdgeTypes>();
            //collect edges
            foreach (var edge in graph.Edges)
            {
                Vertex<T> from = edge.FromVertex;
                Vertex<T> to = edge.ToVertex;

                if (TimeVisitingVertex[from] > TimeVisitingVertex[to] && TimeLeavingVertex[from] <= TimeLeavingVertex[to])
                {
                    classification[edge] = AvalibleEdgeTypes.ForwardEdge;
                }
                else if (TimeVisitingVertex[from] >= TimeVisitingVertex[to] && TimeLeavingVertex[from] > TimeLeavingVertex[to])
                {
                    classification[edge] = AvalibleEdgeTypes.BackEdge;
                }
                else 
                {
                    classification[edge] = AvalibleEdgeTypes.CrossEdge;
                }
               
            }
            return classification;

        }

        public string PrintVertexStates()
        {
            String tmp = "";
            foreach (var vertex in VertexStates)
            {
                tmp += vertex.Key.Id + " - " + vertex.Value.ToString() + "\n";
            }
            return tmp;

        }
    }
}
