using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.ConnectionWeight
{
    public static class ConnectionWeightParser
    {
        public static readonly char ConnectionWeightNameDelimiter = ':';
        public static readonly char ConnectionWeightParameterDelimiter = ',';

        /// <summary>
        ///     Parse a string ConnectionWeight description with the format:
        ///     type:parameter1,parameter2,...
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static IConnectionWeight FromString(string description)
        {
            //If the description is just an integer parse and return it as static weight
            if (int.TryParse(description, out var weight))
            {
                return new StaticWeight(weight);
            }

            //Call the relevant parse methods based on the ConnectionWeightName
            switch (GetConnectionWeightName(description))
            {
                case nameof(StaticWeight):
                    return ParseStaticWeight(description);
                case nameof(VisitCountWeight):
                    return ParseVisitCountWeight(description);
                default:
                    return DefaultWeight();
            }
        }

        public static IConnectionWeight DefaultWeight()
        {
            return new StaticWeight(0);
        }

        public static IConnectionWeight ParseStaticWeight(string description)
        {
            var parameters = GetConnectionWeightParameters(description);
            if (parameters.Length == 0)
            {
                return DefaultWeight();
            }

            int.TryParse(parameters[0], out var weight);
            return new StaticWeight(weight);
        }

        public static IConnectionWeight ParseVisitCountWeight(string description)
        {
            var parameters = GetConnectionWeightParameters(description);
            if (parameters.Length >= 5 &&
                int.TryParse(parameters[1], out var comparisonVisitCount) &&
                int.TryParse(parameters[2], out var lessThenWeight) &&
                int.TryParse(parameters[3], out var equalToWeight) &&
                int.TryParse(parameters[4], out var greaterThenWeight))
            {
                return new VisitCountWeight(parameters[0], comparisonVisitCount, lessThenWeight, equalToWeight,
                    greaterThenWeight);
            }

            return DefaultWeight();
        }

        /// <summary>
        ///     Returns the first substring of the description split by a colon.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private static string GetConnectionWeightName(string description)
        {
            return description?.Split(ConnectionWeightNameDelimiter)[0];
        }

        private static string[] GetConnectionWeightParameters(string description)
        {
            var descriptionComponents = description.Split(ConnectionWeightNameDelimiter);
            return descriptionComponents.Length < 2
                ? new string[0] { }
                : descriptionComponents[1].Split(ConnectionWeightParameterDelimiter);
        }
    }
}