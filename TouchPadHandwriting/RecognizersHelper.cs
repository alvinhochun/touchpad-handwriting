using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Ink;

namespace TouchPadHandwriting
{
    internal static class RecognizersHelper
    {
        internal static Recognizers recognizers;
        internal static Recognizer[] suitableRecognizers;

        static RecognizersHelper()
        {
            recognizers = new Recognizers();
            suitableRecognizers = GetSuitableRecognizersInternal().ToArray();
        }

        private static IEnumerable<Recognizer> GetSuitableRecognizersInternal()
        {
            foreach (Recognizer recognizer in recognizers)
            {
                if (recognizer.Languages.Length > 0)
                {
                    yield return recognizer;
                }
            }
        }

        internal static Recognizer[] GetSuitableRecognizers()
        {
            return suitableRecognizers;
        }

        internal static Recognizer GetFirstRecognizer()
        {
            if (suitableRecognizers.Length > 0)
            {
                return suitableRecognizers[0];
            }
            return null;
        }

        internal static Recognizer GetRecognizer(Guid guid)
        {
            foreach (Recognizer recognizer in GetSuitableRecognizers())
            {
                if (recognizer.Id == guid)
                {
                    return recognizer;
                }
            }
            return null;
        }
    }
}
