public class Heir : Prop
{
    public float Heal { get; private set; }

    public void Initialize(float heal)
    {
        Heal = heal;
    }

    protected override void ChangeValue()
    {
        playerStatusInfo.GetHealed(Heal);
    }
}
