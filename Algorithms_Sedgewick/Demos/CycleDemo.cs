using AlgorithmsSW;
using AlgorithmsSW.Graph;
using Support;

class CycleDemo
{
	public void Run()
	{
		var graph = DataStructures.Graph(10);

		graph.AddEdge(0, 1);
		graph.AddEdge(0, 4);
		graph.AddEdge(0, 5);
		graph.AddEdge(0, 6);
		graph.AddEdge(0, 7);
		graph.AddEdge(0, 8);
		graph.AddEdge(0, 9);
		graph.AddEdge(1, 2);
		graph.AddEdge(1, 7);
		graph.AddEdge(2, 3);
		graph.AddEdge(2, 6);
		graph.AddEdge(3, 0);
		graph.AddEdge(3, 4);

		Tracer.Init();
		Tracer.WriteToConsole = true;

		var cycle = new Cycle(graph);
	}
}
