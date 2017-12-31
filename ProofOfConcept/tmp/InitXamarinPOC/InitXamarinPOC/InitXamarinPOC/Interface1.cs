using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InitXamarinPOC
{
    public interface Interface1
    {
        Task<string[]> SpeechToTextAsync();
    }
}
