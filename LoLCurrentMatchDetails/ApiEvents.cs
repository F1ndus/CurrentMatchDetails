using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLCurrentMatchDetails
{
    interface ApiEvents
    {
        void onLeagueStarted();
        void onLeagueStopped();
        void onStartedUpdate();
        void onFinishedUpdate(string data);
    }
}
