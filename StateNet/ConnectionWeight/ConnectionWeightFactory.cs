using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.NodeWeights
{
    public static class ConnectionWeightFactory
    {
        public static IConnectionWeight FromString(string description)
        {
            if (string.IsNullOrEmpty(description))
                return new StaticWeight(1);

            var name = description.Split(':')[0];
            var parameters = description.Split(':')[1].Split(',');

            switch (name)
            {
                case "Static":
                    return new StaticWeight(int.Parse(parameters[0]));

                case "VisitCount":
                    return new VisitCountWeight(parameters[0], int.Parse(parameters[1]), int.Parse(parameters[2]), int.Parse(parameters[3]), int.Parse(parameters[4]));

                default:
                    return new StaticWeight(1);
            }
        }
    }
}