using NUnit.Framework;
using UITest.Core;

namespace SampleTestLibrary
{
    public class SampleTestClassTwo : SampleBaseClass
    {
        public SampleTestClassTwo(TestDevice testDevice) 
            : base(testDevice)
        {
        }

        [Test]
        public void Test()
        {
            Assert.Pass();
        }
    }
}
