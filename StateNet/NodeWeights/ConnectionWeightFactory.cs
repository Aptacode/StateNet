using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.NodeWeights
{
    public static class ConnectionWeightFactory
    {
        public static INodeWeight FromString(string connectionDescription)
        {
            if(string.IsNullOrEmpty(connectionDescription))
                return new StaticNodeWeight(1);

            var name = connectionDescription.Split(':')[0];
            var parameters = connectionDescription.Split(':')[1].Split(',');

            switch (name)
            {
                case "Static":
                    return new StaticNodeWeight(int.Parse(parameters[0]));
                case "VisitCount":
                    return new VisitCountNodeWeight(parameters[0], int.Parse(parameters[1]), int.Parse(parameters[2]), int.Parse(parameters[3]), int.Parse(parameters[3]));
                default:
                    return new StaticNodeWeight(1);
            }

        }
    }
}
