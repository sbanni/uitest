namespace UITest.Core
{
    public interface IElement : IElementQueryable
    {
        void Click();
        void SendKeys(string text);
        void Clear();
    }
}
