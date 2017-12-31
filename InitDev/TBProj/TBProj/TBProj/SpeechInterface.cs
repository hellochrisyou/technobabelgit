using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TBProj
{
    public interface SpeechInterface
    {
        void toggleSpeech();

        string SpeechToTextAsync();

        void clearMainActivityHit();

        void loginstartcache();

        void sleeplistening();
    }
}
