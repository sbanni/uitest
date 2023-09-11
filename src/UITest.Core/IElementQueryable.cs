namespace UITest.Core
{
    public interface IElementQueryable
    {
        IElement ById(string id);
        IElement ByName(string name);
        IElement ByClass(string className);
        IElement ByAccessibilityId(string name);
        IElement ByQuery(string query);
    }
}
