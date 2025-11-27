namespace SpaceXProject.api.Shared.Base.ResultPattern.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ResultStatusCustomConnotationAttribute : Attribute
{
    public bool IsPositive { get; set; } = false;
}
