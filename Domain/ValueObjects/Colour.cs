using Domain.Common;
using Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class Colour : ValueObject
    {
        private Colour(string code)
        {
            Code = code;
        }

        public string Code { get; private set; }

        public static Colour White => new Colour("#FFFFFF");
        public static Colour Red => new Colour("#FF5733");
        public static Colour Orange => new Colour("#FFC300"); 

        public static Colour From(string code)
        {
            var colour = new Colour(code);

            if (!SupportedColours.Contains(colour))
            {
                throw new UnsupportedColourException(code);
            }
            return colour;
        }

        public static implicit operator string(Colour colour)//implicit convertion
        {
            return colour.ToString();
        }

        public static explicit operator Colour(string code)//explicit convertion
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code;
        }

        protected static IEnumerable<Colour> SupportedColours
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange; 
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
