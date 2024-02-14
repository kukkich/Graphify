namespace Graphify.Geometry.Styling;

public interface IStyled<TStyle>
    where TStyle : IStyle
{
    public TStyle Style { get; set; }
}
