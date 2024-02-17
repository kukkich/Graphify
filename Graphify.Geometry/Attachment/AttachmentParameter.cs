namespace Graphify.Geometry.Attaching;

public class AttachmentParameter
{
    public float T {
        get => _t;
        set
        {
            if (value is < 0 or > 1)
            {
                throw new ArgumentException();
            }
            
            _t = value;
        }
    }
    private float _t;
}
