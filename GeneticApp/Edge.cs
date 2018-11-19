namespace GeneticApp
{
    public class Edge
    {

        public Edge(int vertexA,int vertexB,int cost)
        {
            Cost = cost;
            Vertices =new int[2] { vertexA, vertexB };

        }
        public int Cost { get; set; }
        public int[] Vertices { get; set; }

        public bool IsNeighbour(Edge edge)
        {
            bool result = false;
            if(this.Vertices[0]==edge.Vertices[1] || this.Vertices[1] == edge.Vertices[0])
            {
                result = true;
            }
            return result;
        }
    }
}