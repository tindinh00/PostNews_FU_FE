namespace Client.Responses
{
    public class ODataResponseModel<T>
    { 
        public List<T> Value {  get; set; }
    }
}
