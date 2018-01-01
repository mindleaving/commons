using System.Collections.Generic;

namespace Commons
{
    public class GraphMergeInfo
    {
        public GraphMergeInfo(Dictionary<uint, uint> vertexIdMap, Dictionary<ulong, ulong> edgeIdMap)
        {
            VertexIdMap = vertexIdMap;
            EdgeIdMap = edgeIdMap;
        }

        public Dictionary<uint, uint> VertexIdMap { get; }
        public Dictionary<ulong, ulong> EdgeIdMap { get; }

    }
}