using LiveSplit.Model;
using System;
using LiveSplit.UI.Components;

[assembly: ComponentFactory(typeof(ServerFactory))]

namespace LiveSplit.UI.Components
{
    public class ServerFactory : IComponentFactory
    {
        public string ComponentName => "LiveSplit Server for Trackmania";

        public string Description => "Allows a remote connection and control of LiveSplit with the Speedrun plugin on Openplanet by starting a server within LiveSplit.";

        public ComponentCategory Category => ComponentCategory.Control;

        public IComponent Create(LiveSplitState state) => new ServerComponent(state);

        public string UpdateName => ComponentName;

        public string UpdateURL => "https://github.com/GreepTheSheep/LiveSplit.TMServer/releases/latest";

        public Version Version => Version.Parse("1.8.19");

        public string XMLURL => "http://livesplit.org/update/Components/update.LiveSplit.TMServer.xml";
    }
}
