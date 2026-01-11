using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Denemeler.ValueObjects;

namespace RentCarServer.Domain.Denemeler;

public sealed class Deneme : Entity
{
    public Deneme(DenemeStr1 denemeStr1, DenemeStr2 denemeStr2, DenemeInt1 denemeInt1, DenemeInt2 denemeInt2, DenemeBool1 denemeBool1, DenemeBool2 denemeBool2)
    {
        DenemeId = new(Guid.CreateVersion7());
        setDenemeStr1(denemeStr1);
        setDenemeStr2(denemeStr2);
        setDenemeInt1(denemeInt1);
        setDenemeInt2(denemeInt2);
        setDenemeBool1(denemeBool1);
        setDenemeBool2(denemeBool2);
    }

    public Deneme()
    {
    }

    public IdentityId DenemeId { get; private set; } = default!;
    public DenemeStr1 DenemeStr1 { get; private set; } = default!;
    public DenemeStr2 DenemeStr2 { get; private set; } = default!;
    public DenemeInt1 DenemeInt1 { get; private set; } = default!;
    public DenemeInt2 DenemeInt2 { get; private set; } = default!;
    public DenemeBool1 DenemeBool1 { get; private set; } = default!;
    public DenemeBool2 DenemeBool2 { get; private set; } = default!;

    #region Behaviors

    public void setDenemeStr1(DenemeStr1 denemeStr1)
    {
        DenemeStr1 = denemeStr1;
    }
    public void setDenemeStr2(DenemeStr2 denemeStr2)
    {
        DenemeStr2 = denemeStr2;
    }
    public void setDenemeInt1(DenemeInt1 denemeInt1)
    {
        DenemeInt1 = denemeInt1;
    }
    public void setDenemeInt2(DenemeInt2 denemeInt2)
    {
        DenemeInt2 = denemeInt2;
    }

    public void setDenemeBool1(DenemeBool1 denemeBool1)
    {
        DenemeBool1 = denemeBool1;
    }
    public void setDenemeBool2(DenemeBool2 denemeBool2)
    {
        DenemeBool2 = denemeBool2;
    }

    #endregion

}
