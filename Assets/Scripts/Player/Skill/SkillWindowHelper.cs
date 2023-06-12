using Services;

public static class SkillWindowHelper
{
    public static SkillManager GetSkillManager()
    {
        return ServiceLocator.Get<SkillManager>();
    }
}
