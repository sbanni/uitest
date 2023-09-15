using NUnit.Framework;
using UITest.Core;

namespace SampleTestLibrary
{
    public class SampleTestClassOne : SampleBaseClassWindows
    {
        public SampleTestClassOne(TestDevice testDevice)
            : base(testDevice)
        {
        }

        [Test]
        public void Test()
        {
        }
    }

    public class SampleAndroidTestClassOne : SampleBaseClassAndroid
    {
        public SampleAndroidTestClassOne(TestDevice testDevice)
            : base(testDevice)
        {
        }

        [Test]
        public void Test()
        {
        }
    }
}