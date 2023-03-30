namespace DaprWorkflowAnalysis.Exceptions;

[Serializable]
public class CustomExceptionDTF : Exception
{
    public CustomExceptionDTF() : base()
    {
        
    }
    public CustomExceptionDTF(String message): base(message)
    {
        
    }
}