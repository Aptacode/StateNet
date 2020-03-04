namespace Aptacode.StateNet.Tests.Helpers
{
    public static class StateDistributionGenerator
    {
        public static StateDistribution Generate(params int[] nodeWeights)
        {
            var output = new StateDistribution();
            
            for(var i = 0; i < nodeWeights.Length; i++)
            {
                output.UpdateWeight(new State(i.ToString()), nodeWeights[i]);
            }

            return output;
        }
    }
}