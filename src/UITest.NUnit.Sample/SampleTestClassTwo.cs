using NUnit.Framework;
using UITest.Core;

namespace SampleTestLibrary
{
    public class SampleTestClassTwo : SampleBaseClassWindows
    {
        public SampleTestClassTwo(TestDevice testDevice) 
            : base(testDevice)
        {
        }

        [Test]
        public void Test()
        {
        }
    }
}
