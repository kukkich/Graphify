namespace Graphify.Geometry.Drawing;

public interface IStyled<TStyle>
    where TStyle : IStyle
{
    public TStyle Style { get; set; }
}
