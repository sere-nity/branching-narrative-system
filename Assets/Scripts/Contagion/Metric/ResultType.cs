namespace Contagion.Metric
{
    public enum ResultType
    {
        CRITICAL_SUCCESS,  // Optional - only used when configured
        SUCCESS,          // Always available
        PARTIAL_SUCCESS,  // Optional - only used when configured
        FAILURE           // Always available but can be "interesting failure"
    }
}