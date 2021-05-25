using Domain.Exceptions;
using Domain.ValueObjects;
using NUnit.Framework;

namespace Domain.UnitTests.ValueObjects
{
    [TestFixture]
    public class ColourTests
    {  
        [Test]
        public void From_WhenInitializeNewInstanceOfColourWithUnsupportedCodeIsGettingException_MustBeThrowUnsupportedColourException()
        {
            var blueHexCode = "#0000FF";

            Assert.Throws<UnsupportedColourException>(() => Colour.From(blueHexCode));
        }

        [Test]
        public void From_SuccessfullyInitializeNewInstanceOfColourWithSupportedCode_MustNotThrowAnyEx()
        {
            var whiteHexCode = "#FFFFFF";

            Assert.DoesNotThrow(() => Colour.From(whiteHexCode));
            Assert.IsTrue(Colour.From(whiteHexCode).Equals(Colour.White));
        }

        [Test]
        public void String_ImplicitConvertionOfColourObject_ReturnTrue()
        {
            var whiteHexCode = "#FFFFFF";
            string implicitConvertionAppliedStr = Colour.White;
            Assert.IsTrue(whiteHexCode == implicitConvertionAppliedStr);
        }

        [Test]
        public void Colour_ExplicitConvertionOfColorHexCodeAndEqualityCheck_ReturnTrue()
        {
            var whiteHexCode = "#FFFFFF";
            var whiteColourObj = (Colour)whiteHexCode;
            Assert.IsTrue(Colour.White.Equals(whiteColourObj));
        }

        [Test]
        public void String_TranformColorObjToHexCode_ReturnTrue()
        {
            var whiteHexCode = "#FFFFFF";
            var transformedPresentation = Colour.White.ToString();
            Assert.IsTrue(whiteHexCode == transformedPresentation);
        } 
    }
}
