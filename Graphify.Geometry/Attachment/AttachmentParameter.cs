namespace Graphify.Geometry.Attaching;

public class AttachmentParameter
{
    public float T
    {
        get;
        set
        {
            if (value is < 0 or > 1)
            {
                throw new ArgumentException();
            }

            field = value;
        }
    }
}
