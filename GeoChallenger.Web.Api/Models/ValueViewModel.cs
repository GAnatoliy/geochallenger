namespace GeoChallenger.Web.Api.Models
{
    /// <summary>
    /// Wrapper that should be used for return simple types from api, in order to have correct josn.
    /// </summary>
    public class ValueViewModel<T>
    {
        // NOTE: default constructor is required for help page generators.
        public ValueViewModel()
        {

        }

        public ValueViewModel(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}