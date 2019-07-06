using JetBrains.Annotations;
#if ZENJECT
using Zenject;

namespace Infrastructure.NoxLog
{
	[UsedImplicitly]
	public class Installer : Installer<Installer>
	{
		public override void InstallBindings()
		{
			Container.BindFactory<object, LogLevel, Logger, Logger.Factory>();
			//Container.BindLogger(LogLevel.Trace);
			//Container.BindLogger(LogLevel.Debug);
		}
	}
}
#endif