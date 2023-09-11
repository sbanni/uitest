using NUnit.Framework;
using UITest.Core;

namespace SampleTestLibrary
{
    public class SampleTestClassOne : SampleBaseClass
    {
        public SampleTestClassOne(TestDevice testDevice) 
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